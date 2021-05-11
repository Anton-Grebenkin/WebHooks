using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WebHooks.DAL.EF;
using WebHooks.DAL.Interfaces;
using WebHooks.DAL.Models;

namespace WebHooks.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private WebHooksContext _db;
        private GenericRepository<Account> _accountRepository;
        private GenericRepository<EventHook> _eventHookRepository;
        private GenericRepository<HookTry> _hookTryRepository;
        private GenericRepository<Subscription> _subscriptionRepository;

        public UnitOfWork(String connectionString)
        {
            var contextOptions = new DbContextOptionsBuilder<WebHooksContext>()
            .UseSqlServer(connectionString)
            .Options;
            _db = new WebHooksContext(contextOptions);
        }

        public IRepository<Account> Accounts
        {
            get
            {
                if (_accountRepository == null)
                    _accountRepository = new GenericRepository<Account>(_db);
                return _accountRepository;
            }
        }
        public IRepository<EventHook> EventHookQueue
        {
            get
            {
                if (_eventHookRepository == null)
                    _eventHookRepository = new GenericRepository<EventHook>(_db);
                return _eventHookRepository;
            }
        }
        public IRepository<HookTry> HookTryes
        {
            get
            {
                if (_hookTryRepository == null)
                    _hookTryRepository = new GenericRepository<HookTry>(_db);
                return _hookTryRepository;
            }
        }
        public IRepository<Subscription> Subscriptions
        {
            get
            {
                if (_subscriptionRepository == null)
                    _subscriptionRepository = new GenericRepository<Subscription>(_db);
                return _subscriptionRepository;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
