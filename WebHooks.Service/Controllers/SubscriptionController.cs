using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebHooks.BLL.DTO;
using WebHooks.BLL.Exceptions;
using WebHooks.BLL.Interfaces;
using WebHooks.BLL.Services;

namespace WebHooks.Service.Controllers
{

    [Route("subscription")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        ISubscriptionService SubscriptionService { get; set; }
        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            SubscriptionService = subscriptionService;
        }

        /// <summary>
        /// Возвращает все подписки аккаунта, который указан в авторизации.
        /// </summary>   
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] int count)
        {
            int accountId = int.Parse(User.FindFirst("accountId").Value); // id 

            if (count == 0)
            {
                return StatusCode(412,
                    new ProblemDetails()
                    {
                        Status = 412,
                        Title = "Не передано значение count в query"
                    }); // вернули 412 и ошибку
            }

            try
            {
                var subs = SubscriptionService.GetAccountSubscriptions(accountId, count); //получили аккаунт с подписчиками
                
                //Убираем бесконечный цикл объекта
                var list = JsonConvert.SerializeObject(subs, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                return Ok(list);
            }
            catch(NotExistsException ex)
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

        /// <summary>
        /// Возвращает подписки аккаунта по externalId и аккаунту указанному в регистрации.
        /// </summary>   
        [HttpGet("{externalId}")]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string externalId)
        {
            int accountId = int.Parse(User.FindFirst("accountId").Value); // id подписки

            List<SubscriptionDTO> sub;
            try
            {
                sub = SubscriptionService.GetSubscription(accountId, externalId); //получение из базы подписки
                                                               //Убираем бесконечный цикл объекта
                var list = JsonConvert.SerializeObject(sub, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                return Ok(list);

            }
            catch(NotExistsException ex)
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
                    }); // вернули 412 и ошибку
            }
        }



        /// <summary>
        /// Возвращает подписку аккаунта по externalId, id подписки и аккаунту указанному в регистрации.
        /// </summary>   
        [HttpGet("{externalId}/{subId}")]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string externalId, int subId)
        {
            int accountId = int.Parse(User.FindFirst("accountId").Value); // id подписки

            SubscriptionDTO sub;

            try
            {
                sub = SubscriptionService.GetSubscription(accountId, externalId, subId); //получение из базы подписки
               
                                                                              //Убираем бесконечный цикл объекта
                var list = JsonConvert.SerializeObject(sub, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                return Ok(list);

            }
            catch (NotExistsException ex)
            {
                return StatusCode(412,
                    new ProblemDetails()
                    {
                        Status = 412,
                        Title = ex.Message
                    }); // вернули 404 и ошибку
            }
            catch (Exception ex)
            {
                return StatusCode(412,
                    new ProblemDetails()
                    {
                        Status = 412,
                        Title = ex.Message
                    }); // вернули 412 и ошибку
            }
        }



        /// <summary>
        /// Добавляет подписку на аккаунта, указанный в авторизации
        /// </summary>    
        /// <remarks>
        ///  <p>request must contain the following fields</p>
        ///  
        ///     POST /subscripton
        ///     {
        ///        "AccountId": 1,
        ///        "Url": "testIrl",
        ///        "HttpMethod": "testMethod",
        ///        "ExternalAccountId": "testId",
        ///        "ContentType": "application\json",
        ///        "EventId": "testEvent"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Post([FromBody] SubscriptionDTO subscription)
        {
            try
            {
 
                subscription.AccountId = int.Parse(User.FindFirst("accountId").Value);

                SubscriptionDTO newSubscriptionDTO = SubscriptionService.AddSubscription(subscription); // попробовали добавить в базу в случае ошибки выкинет validationException
                return Ok(newSubscriptionDTO);
            }
            catch(ValidationException ex)
            {
                return StatusCode(412,
                    new ProblemDetails() { 
                        Status = 412, 
                        Title = ex.Message, 
                        Detail = string.Join(string.Empty, ex.Errors)
                    }); // вернули 412 и ошибку
            }
            catch(Exception ex)
            {
                return StatusCode(412,
                    new ProblemDetails()
                    {
                        Status = 412,
                        Title = ex.Message
                    });
            }
        }

        /// <summary>
        /// Удаляет подписку аккаунта, указанного в авторизации. В случае удаления поле IsActual меняется на false
        /// </summary>
        [HttpDelete("{externalId}/{subId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string externalId, int subId)
        {
            int accountId = int.Parse(User.FindFirst("accountId").Value); // id подписки
            try
            {
                SubscriptionService.DeleteSubscription(accountId, externalId, subId); // попытались удалить в случае если нет такой подписки выкидывается NotExistsException
                return Ok();
            }
            catch (NotExistsException ex)
            {
                return StatusCode(412,
                    new ProblemDetails() { 
                        Status = 412, 
                        Title = ex.Message 
                    }); // вернули 404 и ошибку
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
