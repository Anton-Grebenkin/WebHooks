using System;
using System.Collections.Generic;
using System.Text;
using WebHooks.DAL.Models;

namespace WebHooks.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Account> Accounts { get; }
        IRepository<EventHook> EventHookQueue { get; }
        IRepository<HookTry> HookTryes { get; }
        IRepository<Subscription> Subscriptions { get; }
        void Save();
    }
}
