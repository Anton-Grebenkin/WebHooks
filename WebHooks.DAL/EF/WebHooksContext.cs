using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Text;
using WebHooks.DAL.Models;

namespace WebHooks.DAL.EF
{
    public class WebHooksContext : DbContext
    {
        private String _connectionString;
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<EventHook> EventHooks { get; set; }
        public DbSet<HookTry> HookTryes { get; set; }

        public WebHooksContext(DbContextOptions<WebHooksContext> options)
        : base(options)
        {
        }
    }
}
