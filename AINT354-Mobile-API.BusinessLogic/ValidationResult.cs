using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AINT354_Mobile_API.BusinessLogic
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Success = false;
        }
        public bool Success { get; set; }

        public string Error { get; set; }
    }
}
