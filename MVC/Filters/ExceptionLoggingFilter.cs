using System;
using System.Configuration;
using System.Net.Mail;
using System.Web.Mvc;

namespace MVC.Filters
{
    public class ExceptionLoggingFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            //Send ajax response, for ajax request error 
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        Message = "Exception filtered - An error has occured. Please try again later.",
                    }
                };
            }

            // THIS BREAK THE STACK FLOW OF EXCEPTIONS THERE for the VIEW /result returned. 
            // Without it, it will continue up the stack
            // Without... it will carry on and expose the real exception error where throw E happened to the end user
            // Choose carefully
            filterContext.ExceptionHandled = true;

            // if above set to TRUE control what code is return and results
            filterContext.HttpContext.Response.StatusCode = 500;
            //filterContext.Result = // return an action/view
            
            // post to WEB api? maybe later

            try
            {
                //Send an email notification -> can use smtp4dev
                MailMessage email = new MailMessage();
                email.From = new MailAddress("Client@PDV.com");
                email.To.Add(new MailAddress(ConfigurationManager.AppSettings["ErrorEmail"]));
                email.Subject = $"Error @ {ConfigurationManager.AppSettings["ClientName"]}";
                email.Body = filterContext.Exception.Message + Environment.NewLine
                             + filterContext.Exception.StackTrace;
                SmtpClient client = new SmtpClient("localhost");
                client.Send(email);
            }
            catch (Exception e)
            {
                // email could not be sent...
            }
        }
    }

}