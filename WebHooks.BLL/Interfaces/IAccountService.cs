using System;
using System.Collections.Generic;
using System.Text;
using WebHooks.BLL.DTO;

namespace WebHooks.BLL.Interfaces
{
    public interface IAccountService
    {
        AccountDTO LogIn(string username);
        AccountDTO GetAccount(Int64 Id);
    }
}
