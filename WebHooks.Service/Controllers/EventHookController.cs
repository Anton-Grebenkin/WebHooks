using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebHooks.BLL.DTO;
using WebHooks.BLL.Exceptions;
using WebHooks.BLL.Interfaces;

namespace WebHooks.Service.Controllers
{

    [Route("eventhook")]
    [ApiController]
    public class EventHookController : Controller
    {
        //IMainService MainService { get; set; }

        ISubscriptionService SubscriptionService { get; set; }
        IEventHookService EventHookService { get; set; }

        public EventHookController(IEventHookService eventHookService, ISubscriptionService subscriptionService)
        {
            EventHookService = eventHookService;
            SubscriptionService = subscriptionService;
        }


    
/// <summary>
/// Добавляет новый хук для указанной подписки и типа события
/// </summary>    
/// <remarks>
///  <p>request must contain the following fields</p>
///  
///     POST /eventhook
///     {
///        "Event": "testEvent",
///        "subscription": {
///                         "externalAccountId": "string",
///                         "eventId": "string"
///        }
///     }
///
/// </remarks>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Post([FromBody] EventHookDTO eventHook)
        {
            try
            {
                long accountId = long.Parse(User.FindFirst("accountId").Value); // id подписки

                //получаем массив подписок для возможной ситуации что на 1 событие 1 аккаунт с разными url
                List<SubscriptionDTO> subscriptionsOnevents = SubscriptionService.GetSubscriptions(accountId, eventHook.Subscription.ExternalAccountId, eventHook.Subscription.EventId); 
                // для каждой подписки добавляем событие
                foreach (var subscription in subscriptionsOnevents) 
                {
                    EventHookDTO createHook = new EventHookDTO();
                    createHook.Subscription.EventId = eventHook.Subscription.EventId;
                    createHook.Subscription.ExternalAccountId = eventHook.Subscription.ExternalAccountId;
                    createHook.Event = eventHook.Event;
                    createHook.SubscriptionId = subscription.Id; // добавили id
                    createHook.Validation(); //проверили валидность данных
                    createHook.Subscription = null;
                    _ = EventHookService.AddEventHook(createHook); // попробовали добавить в базу в случае ошибки выкинет validationException
                }
                return Ok();
            }
            catch (ValidationException ex)
            {
                return StatusCode(412, new ProblemDetails()
                {
                    Status = 412,
                    Title = ex.Message,
                    Detail = string.Join(string.Empty, ex.Errors)
                }); // вернули 412 и ошибку
            }
            catch (NotExistsException ex)
            {
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(412,
                    new ProblemDetails()
                    {
                        Status = 412,
                        Title = ex.Message
                    });
            }
        }
    }
}
