using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Database.Models.NonDb;
using System.Runtime.InteropServices.ComTypes;

namespace Database
{
    public class VehiclesManager : IVehiclesManager
    {
        private readonly RefuelContext _ctx;

        public VehiclesManager(RefuelContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Vehicle> Add(string manufacturer, string model, decimal engine, int horsePower, string description, User owner)
        {
            if (String.IsNullOrEmpty(manufacturer) ||
                String.IsNullOrEmpty(model) ||
                engine == 0.0M ||
                horsePower == 0)
            {
                throw new Exception("Brakujące dane! Wypełnij wszystkie pola");
            }

            if (manufacturer.Length > 32)
            {
                throw new Exception("Nazwa producenta jest za długa!");
            }

            if (model.Length > 32)
            {
                throw new Exception("Nazwa modelu jest za długa!");
            }
            
            var result = await _ctx.Vehicles.AddAsync(new Vehicle()
            {
                Manufacturer = manufacturer,
                Model = model,
                Engine = engine,
                Horsepower = horsePower,
                Description = description,
                Owner = owner
            });

            await _ctx.SaveChangesAsync();
            
            return result.Entity;
        }

        public async Task<int> Delete(Vehicle vehicle)
        {
            _ctx.Vehicles.Remove(vehicle);

            return await _ctx.SaveChangesAsync();
        }

        public IEnumerable<Vehicle> GetAllUserVehicles(int userId)
        {
            return _ctx.Vehicles.Include(vehicle => vehicle.Owner)
                .Where(vehicle => vehicle.Owner.ID == userId)
                .AsEnumerable();
        }

        public IEnumerable<RefuelChartData> GetAllVehicleRefuelsByFuelTypeAndSortedByDate(Vehicle vehicle, FuelType fuelType)
        {
            var refuels = _ctx.Refuels.Where(refuel => refuel.Vehicle == vehicle && refuel.Fuel == fuelType)
                .OrderBy(refuel => refuel.Date)
                .Select(refuel => 
                    new RefuelChartData
                    {
                        Combustion = refuel.Combustion,
                        RefuelDate = refuel.Date,
                        Price = refuel.PricePerLiter
                    })
                .AsEnumerable();

            return refuels;
        }

        public IEnumerable<Refuel> GetAllVehicleRefuelsSortedDescendingByDate(Vehicle vehicle)
        {
            var collection = _ctx.Refuels.Where(refuel => refuel.Vehicle == vehicle)
                .OrderByDescending(refuel => refuel.Date).AsEnumerable();

            return collection;
        }

        public decimal GetPriceForNKilometers(Vehicle vehicle, int kilometers = 100)
        {
            var summedTotalPrices = _ctx.Refuels.Where(refuel => refuel.Vehicle == vehicle)
                .Select(refuel => refuel.TotalPrice)
                .Sum(x => x);

            var summedKilometers = _ctx.Refuels.Where(refuel => refuel.Vehicle == vehicle)
                .Select(refuel => refuel.Kilometers)
                .Sum(x => x);

            return (summedTotalPrices * kilometers) / summedKilometers;
        }

        public decimal GetPriceForNKilometers(Vehicle vehicle, FuelType fuelType, int kilometers = 100)
        {
            var summedTotalPrices = _ctx.Refuels.Where(refuel => refuel.Vehicle == vehicle && refuel.Fuel == fuelType)
                .Select(refuel => refuel.TotalPrice)
                .Sum(x => x);

            var summedKilometers = _ctx.Refuels.Where(refuel => refuel.Vehicle == vehicle && refuel.Fuel == fuelType)
                .Select(refuel => refuel.Kilometers)
                .Sum(x => x);

            return (summedTotalPrices * kilometers) / summedKilometers;
        }

        public decimal GetTotalCost(Vehicle vehicle)
        {
            return _ctx.Refuels.Where(refuel => refuel.Vehicle == vehicle)
                .Select(refuel => refuel.TotalPrice)
                .Sum(x => x);
        }

        public decimal GetTotalCost(Vehicle vehicle, FuelType fuelType)
        {
            return _ctx.Refuels.Where(refuel => refuel.Vehicle == vehicle && refuel.Fuel == fuelType)
                .Select(refuel => refuel.TotalPrice)
                .Sum(x => x);
        }

        public decimal GetVehicleAverageCombustion(Vehicle vehicle)
        {
            return _ctx.Refuels.Where(refuel => refuel.Vehicle == vehicle)
                .Select(refuel => refuel.Combustion)
                .Average();
        }

        public decimal GetVehicleAverageCombustion(Vehicle vehicle, FuelType fuelType)
        {
            return _ctx.Refuels.Where(refuel => refuel.Vehicle == vehicle && refuel.Fuel == fuelType)
                .Select(refuel => refuel.Combustion)
                .Average();
        }

        public Vehicle GetVehicleById(int i_vehicleId)
        {
            return _ctx.Vehicles.Include(vehicle => vehicle.Owner)
                .Include(vehicle => vehicle.Refuels)
                .FirstOrDefault(vehicle => vehicle.ID == i_vehicleId);
        }

        public string GetVehicleManufacturerAndModelById(int id)
        {   
            var vehicle = _ctx.Vehicles.FirstOrDefault(vehicle => vehicle.ID == id);
            return $"{vehicle.Manufacturer} {vehicle.Model}";
        }

        public async Task<int?> GetVehicleOwnerId(int vehicleId)
        {
            var vehicle = await _ctx.Vehicles.Include(vehicle => vehicle.Owner)
                .FirstOrDefaultAsync(vehicle => vehicle.ID == vehicleId);

            return vehicle?.Owner?.ID;
        }

        public DataPerFuelType GetVehicleDataPerFuelType(Vehicle vehicle)
        {
            var matchingRefuels = _ctx.Refuels.Include(refuel => refuel.Vehicle)
                .Where(refuel => refuel.Vehicle == vehicle);

            var groupedCosts = matchingRefuels.GroupBy(refuel => refuel.Fuel)
                .Select(refuel => new { refuel.Key, Sum = refuel.Sum(refuel => refuel.TotalPrice)})
                .ToDictionary(group => group.Key, group => group.Sum);

            var groupedCombustion = matchingRefuels.GroupBy(refuel => refuel.Fuel)
                .Select(refuel => new { refuel.Key, AvgCombustion = 100 * refuel.Sum(refuel => refuel.Liters) / refuel.Sum(refuel => refuel.Kilometers)})
                .ToDictionary(group => group.Key, group => group.AvgCombustion);

            var groupedPriceFor100Km = matchingRefuels.GroupBy(refuel => refuel.Fuel)
                .Select(refuel => new { refuel.Key, PriceFor100Km = 100 * refuel.Sum(refuel => refuel.TotalPrice) / refuel.Sum(refuel => refuel.Kilometers)})
                .ToDictionary(group => group.Key, group => group.PriceFor100Km);

            return new DataPerFuelType()
            {
                CostsPerFuelType = groupedCosts,
                AverageCombustionPerFuelType = groupedCombustion,
                PriceFor100KmPerFuelType = groupedPriceFor100Km
            };
        }

        public bool IsUserOwnsVehicle(int userId, int vehicleId)
        {
            return _ctx.Vehicles.Include(vehicle => vehicle.Owner)
                .Any(vehicle => vehicle.Owner.ID == userId && vehicle.ID == vehicleId);
        }

        public int SaveChanges()
        {
            return _ctx.SaveChanges();
        }

        public async Task<int> Update(Vehicle vehicle)
        {
            _ctx.Vehicles.Update(vehicle);

            return await _ctx.SaveChangesAsync();
        }

        public async Task<decimal> GetVehicleTotalCosts(Vehicle vehicle)
        {
            return await _ctx.Refuels.Where(dbVehicle => dbVehicle.ID == vehicle.ID)
                .SumAsync(refuel => refuel.TotalPrice);
        }
    }
}
