using System;
using System.Collections.Generic;
using System.Text;

namespace WebHooks.DAL.Models
{
    public class BaseEntity
    {
        public Int64 Id { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public Boolean IsActual { get; set; } = true;
        public DateTime? DeactualizeTime { get; set; }
    }
}
