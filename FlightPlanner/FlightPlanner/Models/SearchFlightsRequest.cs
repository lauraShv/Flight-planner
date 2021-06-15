using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightPlanner.Models
{
    public class SearchFlightsRequest
    {
        public string From { get; }
        public string To { get; }
        public string DepartureDate { get; }

    }
}