using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Database.Models;

namespace Database
{
    public interface IVehiclesManager
    {
        Task<Vehicle> Add(string manufacturer, string model, decimal engine, int horsePower, string description, User owner);
        Task<int> Update(Vehicle vehicle);
        Task<int> Delete(Vehicle vehicle);
        IEnumerable<Refuel> GetAllVehicleRefuelsSortedDescendingByDate(Vehicle vehicle);
        int SaveChanges();
    }
}
