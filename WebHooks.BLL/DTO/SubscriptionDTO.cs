using System;
using System.Collections.Generic;
using System.Text;
using WebHooks.BLL.Exceptions;

namespace WebHooks.BLL.DTO
{
    public class SubscriptionDTO : BaseEntityDTO
    {
        public Int64 AccountId { get; set; }
        public String ExternalAccountId { get; set; }
        public String EventId { get; set; }
        public String Url { get; set; }
        public String HttpMethod { get; set; }
        public String ContentType { get; set; }
        public string IsActual { get; set; }

        public override void Validation()
        {
            List<string> errors = new List<string>();
            bool isValid = true;

            if (this.Url == null)
            {
                errors.Add("Url must be not null");
                isValid = false;
            }

            if (this.HttpMethod == null)
            {
                errors.Add("HttpMethod must be not null");
                isValid = false;
            }

            if (this.ExternalAccountId == null)
            {
                errors.Add("ExternalAccountId must be not null");
                isValid = false;
            }

            if (this.ContentType == null)
            {
                errors.Add("ContentType must be not null");
                isValid = false;
            }

            if (this.EventId == null)
            {
                errors.Add("EventId must be not null");
                isValid = false;
            }

            if (!isValid)
            {
                throw new ValidationException(errors);
            }
        }

    }
}
