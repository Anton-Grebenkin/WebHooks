using System;
using System.Collections.Generic;
using System.Text;
using WebHooks.BLL.Exceptions;

namespace WebHooks.BLL.DTO
{
    public class EventHookDTO : BaseEntityDTO
    {
        public Int64 SubscriptionId { get; set; }
        public SubscriptionDTO Subscription { get; set; }
        public ICollection<HookTryDTO> HookTryes { get; set; }
        public String Event { get; set; }
        public String Status { get; set; }
        public EventHookDTO()
        {
            HookTryes = new List<HookTryDTO>();
            Subscription = new SubscriptionDTO();
        }

        public override void Validation()
        {

            List<string> errors = new List<string>();
            bool isValid = true;

            if (this.Event == null)
            {
                isValid = false;
            }

            if (this.Subscription.ExternalAccountId == null)
            {
                isValid = false;
            }

            if (this.Subscription.EventId == null)
            {
                isValid = false;
            }

            if (!isValid)
            {
                throw new ValidationException(errors);
            }
        }
    }
}
