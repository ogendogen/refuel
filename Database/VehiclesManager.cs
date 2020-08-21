using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;

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
            if (!String.IsNullOrEmpty(manufacturer) ||
                !String.IsNullOrEmpty(model) ||
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

        public int SaveChanges()
        {
            return _ctx.SaveChanges();
        }

        public async Task<int> Update(Vehicle vehicle)
        {
            _ctx.Vehicles.Update(vehicle);

            return await _ctx.SaveChangesAsync();
        }
    }
}
