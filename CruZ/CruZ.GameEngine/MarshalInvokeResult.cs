using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruZ.GameEngine
{
    public class MarshalInvokeResult
    {
        public MarshalInvokeResult(Exception? exception)
        {
            Exception = exception;
        }

        public void ThrowIfNeeded()
        {
            if(Exception != null)
            {
                throw Exception;
            }
        }

        public Exception? Exception
        {
            get;
            private set;
        }
    }
}
