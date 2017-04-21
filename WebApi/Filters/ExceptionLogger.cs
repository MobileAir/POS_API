using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http.Filters;

namespace WebApi.Filters
{
    public class ExceptionLogger : ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext context)
        {
            //var error = new ErrorLog()
            //{
            //    Message = filterContext.Exception.Message,
            //    StackTrace = filterContext.Exception.StackTrace,
            //    ControllerName = filterContext.Controller.GetType().Name,
            //    TargetedResult = filterContext.Result.GetType().Name,
            //    SessionId = (string)filterContext.HttpContext.Request["LoanId"],
            //    UserAgent = filterContext.HttpContext.Request.UserAgent,
            //    Timestamp = DateTime.Now
            //};


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

        
    }

}