using System;
using System.Collections.Generic;
using System.Text;

namespace WebHooks.BLL.DTO
{
    public class AccountDTO : BaseEntityDTO
    {
        public String CompanyName { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public ICollection<SubscriptionDTO> Subscriptions { get; set; }
        public AccountDTO()
        {
            Subscriptions = new List<SubscriptionDTO>();
        }
    }
}
