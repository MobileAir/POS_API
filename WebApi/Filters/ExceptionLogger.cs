using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Http.Filters;
using Services;
using Services.DTOs;
using Services.Interface;

namespace WebApi.Filters
{
    public class ExceptionLogger : ExceptionFilterAttribute
    {
        private const string ClientUserAgent = "Client-User-Agent";
        public override void OnException(HttpActionExecutedContext context)
        {
            var provider =
                context.ActionContext.ControllerContext.Configuration.DependencyResolver.GetService(
                    typeof(IErrorServices)) as IErrorServices;

            try
            {
                var httpRequestBase = ((HttpContextBase) context.Request?.Properties?["MS_HttpContext"])?.Request;
                if (httpRequestBase?.UserHostAddress != null)
                {
                    IPAddress ip =
                        IPAddress.Parse(
                            httpRequestBase?.UserHostAddress);

                    var errorDto = new ErrorDTO()
                    {
                        Message = context.Exception.Message,
                        StackTrace = context.Exception.StackTrace,
                        Controller = context.ActionContext.ControllerContext.Controller.GetType().Name,
                        TargetResult = context.ActionContext.ActionDescriptor.ActionName?.ToString(),
                        Ip = ip.ToString(),
                        UserAgent = context.Request.Headers.GetValues(ClientUserAgent)?.FirstOrDefault(),
                        Timestamp = DateTime.Now
                    };

                    provider?.Create(errorDto);
                }


                //Send an email notification -> can use smtp4dev
                MailMessage email = new MailMessage();
                email.From = new MailAddress("WebApi@PDV.com");
                email.To.Add(new MailAddress(ConfigurationManager.AppSettings["ErrorEmail"]));
                email.Subject = "Api - An error has occured";
                email.Body = context.Exception.Message + Environment.NewLine
                             + context.Exception.StackTrace;
                SmtpClient client = new SmtpClient("localhost");
                client.Send(email);
            }
            catch (Exception e)
            {
                // log straight to a file..?
            }
        }

        
    }

}