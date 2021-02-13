using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccessLayer.Model
{
    public class User
    {
        public User()
        {
            Customers = new HashSet<Customer>();
            FollowUps = new HashSet<FollowUp>();
            Orders = new HashSet<Order>();
        }
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime LastAccess { get; set; }
        public string ObjectName { set; get; }
        public string ObjectId { set; get; }
        public bool active { set; get; }
        public int RoleId { set; get; }
        public virtual Roles Roles { set; get; }
        public virtual ICollection<Customer> Customers { set; get; }
        public virtual ICollection<FollowUp> FollowUps { set; get; }
        public virtual ICollection<Order> Orders { set; get; }

    }
}
