using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
    public class Vehicle
    {
        public int ID { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public decimal Engine { get; set; }
        public int Horsepower { get; set; }
        public string Description { get; set; }
        public User Owner { get; set; }
        public virtual ICollection<Refuel> Refuels { get; set; }
        [NotMapped]
        public string Name 
        {
            get
            {
                return $"{Manufacturer} {Model}";
            }
        }
        public Vehicle()
        {
            Refuels = new List<Refuel>();
        }
    }
}
