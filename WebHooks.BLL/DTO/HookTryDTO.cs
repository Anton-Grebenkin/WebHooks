using System;
using System.Collections.Generic;
using System.Text;

namespace WebHooks.BLL.DTO
{
    public class HookTryDTO : BaseEntityDTO
    {
        public Int64 HookId { get; set; }
        public EventHookDTO EventHook { get; set; }
        public String Response { get; set; }
        public String Request { get; set; }
        public Int32 HttpStatus { get; set; }
    }
}
