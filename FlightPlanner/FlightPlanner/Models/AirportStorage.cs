using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace FlightPlanner.Models
{
    public class AirportStorage
    {
        public static List<Airport> AirportNames = new List<Airport>();

        public static void AddAirport(Flight airport)
        {
            
            if (!AirportNames.Contains(airport.From))
            {
                AirportNames.Add(airport.From);
            }
            if (!AirportNames.Contains(airport.To))
            {
                AirportNames.Add(airport.To);
            }
        }

        public static List<Airport> FindAirport(string inputAirport)
        {
            var airportToSearch = inputAirport.ToLower().Trim();
            var airport = AirportNames.Where(x => x.AirportName.ToLower().Contains(airportToSearch) ||
                                                           x.Country.ToLower().Contains(airportToSearch) ||
                                                           x.City.ToLower().Contains(airportToSearch)).ToList();

            
            return airport;
        }
    }
}