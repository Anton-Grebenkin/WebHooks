using BonusPlus.WebHook.Client;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebHooks.DAL.EF;
using WebHooks.DAL.Interfaces;
using WebHooks.DAL.Models;
using WebHooks.DAL.Repositories;

namespace WebHooks.Worker.Models
{
    public class AccountTask
    {
        public Task Task { get; set; }
        public String ExternalAccountId { get; set; }
    }
}
