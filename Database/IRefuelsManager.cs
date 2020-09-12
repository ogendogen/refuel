using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Database.Models;

namespace Database
{
    public interface IRefuelsManager
    {
        Task<Refuel> Add(DateTime date, uint kilometers, decimal pricePerLiter, decimal liters, decimal combustion, FuelType fuel, decimal totalPrice);
        Task<int> Update(Refuel refuel);
        Task<int> Delete(Refuel refuel);
        int SaveChanges();
        bool IsUserOwnsRefuel(int userId, int refuelId);
        Refuel GetRefuelById(int refuelId);
        Vehicle GetRefuelsVehicle(int refuelId);
    }
}
