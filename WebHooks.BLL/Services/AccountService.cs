using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebHooks.BLL.DTO;
using WebHooks.BLL.Interfaces;
using WebHooks.DAL.Interfaces;
using WebHooks.DAL.Models;
using WebHooks.DAL.Repositories;

namespace WebHooks.BLL.Services
{
    public class AccountService : IAccountService
    {

        IRepository<Account> Accounts;


        private MapperConfiguration _mapConfig;
        private IMapper _mapper;
        public AccountService(IRepository<Account> account)
        {
            Accounts = account;
            _mapConfig = new MapperConfiguration(mc => {
                mc.CreateMap<Account, AccountDTO>();
                mc.CreateMap<Subscription, SubscriptionDTO>();
                });
            _mapper = _mapConfig.CreateMapper();
        }

        public AccountDTO LogIn(string username)
        {
            var account = Accounts.Find(account => account.Login == username).FirstOrDefault();
            return _mapper.Map<Account, AccountDTO>(account);
        }

        public AccountDTO GetAccount(Int64 Id)
        {
            var account = Accounts.Get(Id, a => a.Subscriptions.Where(s => s.IsActual));
            return _mapper.Map<Account, AccountDTO>(account);
        }
    }
}
