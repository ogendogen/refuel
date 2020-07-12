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
        public string Engine { get; set; }
        public int Horsepower { get; set; }
        public string Description { get; set; }
        public uint OwnerID { get; set; }
        public virtual ICollection<Refuel> Refuels { get; set; }
    }
}
