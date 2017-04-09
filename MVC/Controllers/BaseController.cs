using System;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MVC.Common;

namespace MVC.Controllers
{
    /// <summary>
    /// Use to Call Web api and Store token in Session
    /// </summary>
    public class BaseController : Controller
    {
        public BaseController()
        {
            // TODO move all this to a filter instead of a Base controller... or in global - will always check for Token
            //var singleton = Singleton.Instance; // that's bad idea what the idea when the token expires??

            //var token = System.Web.HttpContext.Current.Session["Token"];
            //var expiresOn = System.Web.HttpContext.Current.Session["TokenExpiresOn"];

            //if 
            //    (token == null || token.ToString().IsNullOrWhiteSpace() ||
            //    expiresOn == null ||
            //    expiresOn.ToString().IsNullOrWhiteSpace() ||
            //    DateTime.Parse(expiresOn.ToString()) < DateTime.Now)
            //{
            //    // get token
            //    var tokenH = new ApiCRUDClient().RequestToken(); 

            //    // set token
            //    System.Web.HttpContext.Current.Session["Token"] = tokenH.Token;
            //    System.Web.HttpContext.Current.Session["TokenExpiresOn"] = tokenH.ExpiresOn;
            //}

            
        }
    }

    public sealed class Singleton
    {
        private static volatile Singleton instance;
        private static object syncRoot = new Object();

        private Singleton()
        {
            ApiRequestManager.RequestToken(); // get token
        }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Singleton();
                    }
                }

                return instance;
            }
        }
    }
}