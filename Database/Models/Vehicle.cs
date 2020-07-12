using System;
using System.Collections.Generic;
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
    }
}
