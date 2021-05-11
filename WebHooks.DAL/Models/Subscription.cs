using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebHooks.DAL.Models
{
    [Index(nameof(IsActual), nameof(AccountId), nameof(ExternalAccountId), nameof(EventId))]
    [Index(nameof(IsActual), nameof(ExternalAccountId))]
    public class Subscription : BaseEntity
    {
        public Account Account { get; set; }
        public Int64 AccountId { get; set; }
        [MaxLength(50)]
        public String ExternalAccountId { get; set; }
        [MaxLength(50)]
        public String EventId { get; set; }
        public String Url { get; set; }
        [MaxLength(50)]
        public String HttpMethod { get; set; }
        [MaxLength(50)]
        public String ContentType { get; set; }
        public String SecretKey { get; set; }

    }
}
