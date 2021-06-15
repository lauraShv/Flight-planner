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

        [System.Web.Http.Route("api/flights/{id}")]
        public Flight FindFlightById (int id)
        {
            if (FlightStorage.AllFlights.Exists(x => x.Id == id))
            {
                return FlightStorage.AllFlights.FirstOrDefault(flight => flight.Id == id);
            }

            return null;
        }

    }
}
