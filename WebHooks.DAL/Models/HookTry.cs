using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebHooks.DAL.Models
{
    public class HookTry : BaseEntity
    {
        public EventHook EventHook { get; set; }
        public Int64 EventHookId { get; set; }
        public String Response { get; set; }
        public String Request { get; set; }
        [MaxLength(50)]
        public String HttpStatus { get; set; }
    }
}
