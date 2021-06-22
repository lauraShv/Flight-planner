using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace FlightPlanner.Models
{
    public class SearchFlightsRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public string DepartureDate { get; set; }


        public static bool IsNotAValidRequest(SearchFlightsRequest flight)
        {
            var result = flight == null ||
                         flight.From == flight.To ||
                         string.IsNullOrEmpty(flight.From) ||
                         string.IsNullOrEmpty(flight.To) ||
                         string.IsNullOrEmpty(flight.DepartureDate);
            return result;
        }

    }
}