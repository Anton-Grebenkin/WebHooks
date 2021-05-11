using System;
using System.Collections.Generic;
using System.Text;

namespace WebHooks.Events
{
    public class EntityEvent : AccountEvent
    {
        public string Entity { get; set; }
    }
}
