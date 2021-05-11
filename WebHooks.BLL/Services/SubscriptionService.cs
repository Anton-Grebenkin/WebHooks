using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebHooks.BLL.DTO;
using WebHooks.BLL.Exceptions;
using WebHooks.BLL.Interfaces;
using WebHooks.DAL.Interfaces;
using WebHooks.DAL.Models;

namespace WebHooks.BLL.Services
{
    public class SubscriptionService : ISubscriptionService
    {

        IRepository<Subscription> Subscriptions; 

        private MapperConfiguration _mapConfig;
        private IMapper _mapper;
        public SubscriptionService(IRepository<Subscription> subscription)
        {
            Subscriptions = subscription;

            _mapConfig = new MapperConfiguration(mc => {
                mc.CreateMap<Account, AccountDTO>();
                mc.CreateMap<Subscription, SubscriptionDTO>().ReverseMap();
            });
            _mapper = _mapConfig.CreateMapper();
        }
       
        public IEnumerable<SubscriptionDTO> GetAccountSubscriptions(Int64 accountId, int count)
        {
            var subscriptions = Subscriptions.Find(s => s.AccountId == accountId && s.IsActual).Take(count);
            if (subscriptions.Count() == 0)
            {
                throw new NotExistsException("This account have not subscriptions");
            }
            return _mapper.Map<IEnumerable<Subscription>, List<SubscriptionDTO>>(subscriptions);
        }

        public SubscriptionDTO GetSubscription(String externalAccountId, String eventId)
        {
            var subscription = Subscriptions.Find(s => s.IsActual && s.ExternalAccountId == externalAccountId && s.EventId == eventId).FirstOrDefault();
            if (subscription == null)
            {
                throw new NotExistsException();
            }
            return _mapper.Map<Subscription, SubscriptionDTO>(subscription);
        }


        public SubscriptionDTO GetSubscriptionsByEvent(string eventId, string externalId)
        {
            var subscription = Subscriptions.Find(s => s.EventId == eventId && s.ExternalAccountId == externalId).FirstOrDefault();
            if (subscription == null)
            {
                throw new NotExistsException("This account have not subscriptions on this event");
            }
            return _mapper.Map<Subscription, SubscriptionDTO>(subscription);
        }


        public SubscriptionDTO GetSubscription(long accountId, String externalAccountId, String eventId)
        {
            var subscription = Subscriptions.Find(s => s.IsActual && s.Account.Id == accountId && s.ExternalAccountId == externalAccountId && s.EventId == eventId, s => s.Account).FirstOrDefault();
            if (subscription == null)
            {
                throw new NotExistsException();
            }
            return _mapper.Map<Subscription, SubscriptionDTO>(subscription);
        }


        public SubscriptionDTO AddSubscription(SubscriptionDTO subscriptionDto)
        {
            try
            {
                subscriptionDto.Validation();
                var subscription = _mapper.Map<SubscriptionDTO, Subscription>(subscriptionDto);
                subscription.CreateTime = DateTime.UtcNow;

                if(Subscriptions.GetQuery().Where(
                    s => s.ExternalAccountId == subscription.ExternalAccountId
                    && s.AccountId == subscription.AccountId
                    && s.EventId == subscription.EventId
                    && s.Url == subscription.Url
                    && s.HttpMethod == subscription.HttpMethod
                    && s.IsActual).Any())
                {
                    throw new ValidationException(new List<string>() {"Subscription already exists."});
                }

                Subscriptions.Create(subscription);
                Subscriptions.Save();
                subscriptionDto.Id = subscription.Id;
                return subscriptionDto;
            }
            catch (ValidationException ex)
            {
                throw ex;
            }
        }

        public List<SubscriptionDTO> GetSubscription(long accountId ,string externalid)
        {
            var subscription = Subscriptions.Find(s=> s.ExternalAccountId == externalid && s.AccountId == accountId && s.IsActual == true);
            if (subscription.Count() == 0)
            {
                throw new NotExistsException("Subscription not exist");
            }
            return _mapper.Map<IEnumerable<Subscription>, List<SubscriptionDTO>>(subscription);
        }

        public List<SubscriptionDTO> GetSubscriptions(long accountId, String externalAccountId, String eventId)
        {
            var subscription = Subscriptions.Find(s => s.IsActual && s.Account.Id == accountId && s.ExternalAccountId == externalAccountId && s.EventId == eventId, s => s.Account).ToList();
            if (subscription == null)
            {
                throw new NotExistsException();
            }

            return _mapper.Map<IEnumerable<Subscription>, List<SubscriptionDTO>>(subscription);
        }
        public SubscriptionDTO GetSubscription(long accountId, string externalid, long subId)
        {
            var subscription = Subscriptions.Find(s => s.ExternalAccountId == externalid && s.AccountId == accountId && s.Id == subId && s.IsActual == true).FirstOrDefault();
            if (subscription == null)
            {
                throw new NotExistsException("Subscription not exist");
            }
            return _mapper.Map<Subscription, SubscriptionDTO>(subscription);
        }

        public void DeleteSubscription(long accountId, string externalid, long subId)
        {
            var subscription = Subscriptions.Find(s => s.ExternalAccountId == externalid && s.AccountId == accountId && s.Id == subId).FirstOrDefault();
            if (subscription == null)
            {
                throw new NotExistsException("This subscription not exists");
            }
            Subscriptions.Deactualize(subscription);
            Subscriptions.Save();
        }

        
    }

}
