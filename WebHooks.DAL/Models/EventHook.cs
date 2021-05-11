using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebHooks.DAL.Models
{
    [Index(nameof(IsActual), nameof(Status), nameof(TryCount), nameof(SubscriptionId))]
    [Index(nameof(IsActual), nameof(SubscriptionId), nameof(SendTime))]
    [Index(nameof(IsActual), nameof(SendTime), nameof(SubscriptionId))]

    public class EventHook : BaseEntity
    {
        public Int64 SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public ICollection<HookTry> HookTryes { get; set; }
        public String Event { get; set; }
        [MaxLength(50)]
        public String Status { get; set; } = "NEW";
        public int TryCount { get; set; } = 0;
        public DateTime? SendTime { get; set; }
        public EventHook()
        {
            SendTime = this.CreateTime;
            HookTryes = new List<HookTry>();
        }
    }
}
