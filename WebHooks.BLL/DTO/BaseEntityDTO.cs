using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace WebHooks.BLL.DTO
{
    public class BaseEntityDTO
    {
        public Int64 Id { get; set; }
        public DateTime CreateTime { get; set; }
        public virtual void Validation() { }
    }
}
