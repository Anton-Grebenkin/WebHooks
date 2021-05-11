using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebHooks.BLL.DTO;
using WebHooks.BLL.Exceptions;
using WebHooks.BLL.Interfaces;
using WebHooks.DAL.Interfaces;
using WebHooks.DAL.Models;

namespace WebHooks.BLL.Services
{
    public class EventHookService : IEventHookService
    {
        //IUnitOfWork Database { get; set; }
        IRepository<EventHook> EventHooks;

        private MapperConfiguration _mapConfig;
        private IMapper _mapper;
        public EventHookService(IRepository<EventHook> eventHooks)
        {
            EventHooks = eventHooks;

            _mapConfig = new MapperConfiguration(mc => {
                mc.CreateMap<EventHook, EventHookDTO>();
                mc.CreateMap<EventHookDTO, EventHook>();
                mc.CreateMap<Subscription, SubscriptionDTO>();
                mc.CreateMap<SubscriptionDTO, Subscription>();
            });
            _mapper = _mapConfig.CreateMapper();
        }

        public EventHookDTO AddEventHook(EventHookDTO eventHookDto)
        {
            try
            {
                eventHookDto.Status = "NEW";
                var eventHook = _mapper.Map<EventHookDTO, EventHook>(eventHookDto);
                eventHook.CreateTime = DateTime.UtcNow;
                eventHook.SendTime = eventHook.CreateTime;
                EventHooks.Create(eventHook);
                EventHooks.Save();
                eventHookDto.Id = eventHook.Id;
                
                return eventHookDto;
            }
            catch (ValidationException ex)
            {
                throw ex;
            }
        }
    }
}
