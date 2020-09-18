using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class RefuelsManager : IRefuelsManager
    {
        private readonly RefuelContext _ctx;

        public RefuelsManager(RefuelContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Refuel> Add(DateTime date, uint kilometers, decimal pricePerLiter, decimal liters, decimal combustion, FuelType fuel, decimal totalPrice, Vehicle vehicle)
        {
            var result = await _ctx.Refuels.AddAsync(new Refuel()
            {
                Date = date,
                Kilometers = kilometers,
                PricePerLiter = pricePerLiter,
                Liters = liters,
                Combustion = combustion,
                Fuel = fuel,
                TotalPrice = totalPrice,
                Vehicle = vehicle
            });

            await _ctx.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<int> Delete(Refuel refuel)
        {
            _ctx.Refuels.Remove(refuel);

            return await _ctx.SaveChangesAsync();
        }

        public Refuel GetRefuelById(int refuelId)
        {
            return _ctx.Refuels.Include(refuel => refuel.Vehicle)
                .FirstOrDefault(refuel => refuel.ID == refuelId);
        }

        public Vehicle GetRefuelsVehicle(int refuelId)
        {
            return GetRefuelById(refuelId)
                .Vehicle;
        }

        public bool IsUserOwnsRefuel(int userId, int refuelId)
        {
            return _ctx.Refuels.Include(refuel => refuel.Vehicle)
                .ThenInclude(vehicle => vehicle.Owner)
                .FirstOrDefault(refuel => refuel.ID == refuelId && refuel.Vehicle.Owner.ID == userId) != null;
        }

        public int SaveChanges()
        {
            return _ctx.SaveChanges();
        }

        public async Task<int> Update(Refuel refuel)
        {
            _ctx.Refuels.Update(refuel);
            
            return await _ctx.SaveChangesAsync();
        }
    }
}
