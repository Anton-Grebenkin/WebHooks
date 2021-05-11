using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebHooks.DAL.Models
{
    public class Account : BaseEntity
    {
        [MaxLength(200)]
        public String CompanyName { get; set; }
        [MaxLength(50)]
        public String Login { get; set; }
        [MaxLength(50)]
        public String Password { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public Account()
        {
            Subscriptions = new List<Subscription>();
        }
    }
}
