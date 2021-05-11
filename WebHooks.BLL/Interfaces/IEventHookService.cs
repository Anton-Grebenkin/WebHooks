using System;
using System.Collections.Generic;
using System.Text;
using WebHooks.BLL.DTO;

namespace WebHooks.BLL.Interfaces
{
    public interface IEventHookService
    {
        EventHookDTO AddEventHook(EventHookDTO eventHookDto);
    }
}
