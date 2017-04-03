using System;
using System.Web.Mvc;
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
            //Session["CurrentUser"] = "";
            //var singleton = Singleton.Instance;
        }
    }

    public sealed class Singleton
    {
        private static volatile Singleton instance;
        private static object syncRoot = new Object();

        private Singleton()
        {
            new WebApiClient().RequestToken(); // get token
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