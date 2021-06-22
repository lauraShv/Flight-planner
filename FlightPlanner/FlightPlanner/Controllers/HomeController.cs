using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Description;
using System.Web.Mvc;
using FlightPlanner.Attributes;
using FlightPlanner.Models;

namespace FlightPlanner.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


    }
}
