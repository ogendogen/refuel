using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using Database.Models.NonDb;

namespace Database
{
    public interface IVehiclesManager
    {
        Task<Vehicle> Add(string manufacturer, string model, decimal engine, int horsePower, string description, User owner);
        Task<int> Update(Vehicle vehicle);
        Task<int> Delete(Vehicle vehicle);
        IEnumerable<Refuel> GetAllVehicleRefuelsSortedDescendingByDate(Vehicle vehicle);
        int SaveChanges();
        decimal GetVehicleAverageCombustion(Vehicle vehicle);
        decimal GetVehicleAverageCombustion(Vehicle vehicle, FuelType fuelType);
        decimal GetPriceForNKilometers(Vehicle vehicle, int kilometers = 100);
        decimal GetPriceForNKilometers(Vehicle vehicle, FuelType fuelType, int kilometers = 100);
        decimal GetTotalCost(Vehicle vehicle);
        decimal GetTotalCost(Vehicle vehicle, FuelType fuelType);
        string GetVehicleManufacturerAndModelById(int value);
        Task<int?> GetVehicleOwnerId(int vehicleId);
        Vehicle GetVehicleById(int i_vehicleId);
        IEnumerable<Vehicle> GetAllUserVehicles(int userId);
        bool IsUserOwnsVehicle(int userId, int vehicleId);
        DataPerFuelType GetVehicleDataPerFuelType(Vehicle vehicle);
        IEnumerable<RefuelChartData> GetAllVehicleRefuelsByFuelTypeAndSortedByDate(Vehicle vehicle, FuelType fuelType);
        Task<decimal> GetVehicleTotalCosts(Vehicle vehicle);
    }
}
