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
        /// ���������� ��� �������� ��������, ������� ������ � �����������.
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
                        Title = "�� �������� �������� count � query"
                    }); // ������� 412 � ������
            }

            try
            {
                var subs = SubscriptionService.GetAccountSubscriptions(accountId, count); //�������� ������� � ������������
                
                //������� ����������� ���� �������
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
        /// ���������� �������� �������� �� externalId � �������� ���������� � �����������.
        /// </summary>   
        [HttpGet("{externalId}")]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string externalId)
        {
            int accountId = int.Parse(User.FindFirst("accountId").Value); // id ��������

            List<SubscriptionDTO> sub;
            try
            {
                sub = SubscriptionService.GetSubscription(accountId, externalId); //��������� �� ���� ��������
                                                               //������� ����������� ���� �������
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
                    }); // ������� 412 � ������
            }
        }



        /// <summary>
        /// ���������� �������� �������� �� externalId, id �������� � �������� ���������� � �����������.
        /// </summary>   
        [HttpGet("{externalId}/{subId}")]
        [ProducesResponseType(StatusCodes.Status412PreconditionFailed)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string externalId, int subId)
        {
            int accountId = int.Parse(User.FindFirst("accountId").Value); // id ��������

            SubscriptionDTO sub;

            try
            {
                sub = SubscriptionService.GetSubscription(accountId, externalId, subId); //��������� �� ���� ��������
               
                                                                              //������� ����������� ���� �������
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
                    }); // ������� 404 � ������
            }
            catch (Exception ex)
            {
                return StatusCode(412,
                    new ProblemDetails()
                    {
                        Status = 412,
                        Title = ex.Message
                    }); // ������� 412 � ������
            }
        }



        /// <summary>
        /// ��������� �������� �� ��������, ��������� � �����������
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

                SubscriptionDTO newSubscriptionDTO = SubscriptionService.AddSubscription(subscription); // ����������� �������� � ���� � ������ ������ ������� validationException
                return Ok(newSubscriptionDTO);
            }
            catch(ValidationException ex)
            {
                return StatusCode(412,
                    new ProblemDetails() { 
                        Status = 412, 
                        Title = ex.Message, 
                        Detail = string.Join(string.Empty, ex.Errors)
                    }); // ������� 412 � ������
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
        /// ������� �������� ��������, ���������� � �����������. � ������ �������� ���� IsActual �������� �� false
        /// </summary>
        [HttpDelete("{externalId}/{subId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string externalId, int subId)
        {
            int accountId = int.Parse(User.FindFirst("accountId").Value); // id ��������
            try
            {
                SubscriptionService.DeleteSubscription(accountId, externalId, subId); // ���������� ������� � ������ ���� ��� ����� �������� ������������ NotExistsException
                return Ok();
            }
            catch (NotExistsException ex)
            {
                return StatusCode(412,
                    new ProblemDetails() { 
                        Status = 412, 
                        Title = ex.Message 
                    }); // ������� 404 � ������
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
