using System;
using System.Collections.Generic;
using System.Text;
using WebHooks.BLL.DTO;

namespace WebHooks.BLL.Interfaces
{
    public interface ISubscriptionService
    {

        SubscriptionDTO GetSubscription(long accountId, String externalAccountId, String eventId);
        List<SubscriptionDTO> GetSubscription(long accountId, string externalid);
        SubscriptionDTO GetSubscription(long accountId, string externalid, long subId);
        SubscriptionDTO GetSubscription(String externalAccountId, String eventId);
        List<SubscriptionDTO> GetSubscriptions(long accountId, String externalAccountId, String eventId);
        IEnumerable<SubscriptionDTO> GetAccountSubscriptions(Int64 accountId, int count);
        SubscriptionDTO GetSubscriptionsByEvent(string eventId, string externalId);

        SubscriptionDTO AddSubscription(SubscriptionDTO subscriptionDto);
        void DeleteSubscription(long accountId, string externalid, long subId);
       
    }
}
