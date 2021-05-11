using System;
using System.Collections.Generic;
using System.Text;

namespace WebHooks.BLL.Exceptions
{
    public class ValidationException: Exception
    {
        public ValidationException(IEnumerable<string> errors = null) { this.Errors = errors; }
        public IEnumerable<string> Errors { get; set; }

        public override string Message => "Validation exception"; 
    }
}
