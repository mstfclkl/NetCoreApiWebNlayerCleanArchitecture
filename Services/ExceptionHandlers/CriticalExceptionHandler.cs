using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace App.Services.ExceptionHandlers
{
    public class CriticalExceptionHandler: IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is CriticalException)
            {
                Console.WriteLine("Sms sent for critical exception.");

            }
            return ValueTask.FromResult(false);
            // return true; // This is a placeholder implementation. You can add your own logic to handle critical exceptions here.
            // return false; // Business logic exceptions should be handled by the GlobalExceptionHandler, so we return false here to indicate that this handler does not handle the exception.
        }
    }
}
