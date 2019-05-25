using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Imports
{
    public class ImportMsgException : Exception
    {
        public virtual string Message { get;}
       
        public ImportMsgException(string message) 
        {
            this.Message = message;
        }
    }
}
