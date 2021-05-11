using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WebHooks.BLL.DTO;
using WebHooks.BLL.Services;
using WebHooks.Service.Authentication;
using WebHooks.Service.Models;


namespace WebHooks.Service.Controllers
{

    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {

    }


}
