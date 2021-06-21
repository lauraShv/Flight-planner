using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace FlightPlanner.Models
{
    public static class FlightStorage
    {
        public static List<Flight> AllFlights = new List<Flight>();
        private static int _id;

        public static Flight AddFlight(Flight newFlight)
        {
            newFlight.Id = _id;
            _id++;
            AllFlights.Add(newFlight);
            AirportStorage.AddAirport(newFlight);
            return newFlight; 
        }

        public static Flight FindFlight(int id)
        {
            return AllFlights.FirstOrDefault(x => x.Id == id);
        }

        public static void IsFlightValid(SearchFlightsRequest request)
        {
            
        }

    }
}