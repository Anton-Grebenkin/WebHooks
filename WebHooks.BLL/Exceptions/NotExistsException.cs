using System;
using System.Collections.Generic;
using System.Text;

namespace WebHooks.BLL.Exceptions
{
    public class NotExistsException : Exception
    {
        public NotExistsException() { }
        public NotExistsException(string message) : base(message) { }
    }
}
