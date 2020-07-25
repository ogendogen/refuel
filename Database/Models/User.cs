using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }

        public User()
        {
            Vehicles = new List<Vehicle>();
        }
    }
}
