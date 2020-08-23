using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;

namespace Database
{
    public class RefuelsManager : IRefuelsManager
    {
        private readonly RefuelContext _ctx;

        public RefuelsManager(RefuelContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Refuel> Add(DateTime date, uint kilometers, decimal pricePerLiter, decimal liters, decimal combustion, FuelType fuel, decimal totalPrice)
        {
            if (date == null ||
                kilometers == 0 ||
                pricePerLiter == 0.0M ||
                liters == 0.0M ||
                combustion == 0.0M ||
                totalPrice == 0.0M)
            {
                throw new Exception("Brakujące dane! Wypełnij wszystkie pola");
            }

            if (date > DateTime.Now)
            {
                throw new Exception("Data jest z przyszłości!");
            }

            var result = await _ctx.Refuels.AddAsync(new Refuel()
            {
                Date = date,
                Kilometers = kilometers,
                PricePerLiter = pricePerLiter,
                Liters = liters,
                Combustion = combustion,
                Fuel = fuel,
                TotalPrice = totalPrice
            });

            return result.Entity;
        }

        public async Task<int> Delete(Refuel refuel)
        {
            _ctx.Refuels.Remove(refuel);

            return await _ctx.SaveChangesAsync();
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
