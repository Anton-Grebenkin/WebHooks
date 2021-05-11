using System;
using System.Collections.Generic;
using System.Text;

namespace WebHooks.Events
{
    public class Event
    {
        public DateTime Timestamp { get; set; }

        public Event()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
