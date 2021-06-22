using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Management;
using FlightPlanner.Models;

namespace FlightPlanner.Controllers
{
    public class CustomerController : ApiController
    {
        private static readonly object _locked = new object();

        [Route("api/airports")]
        [HttpGet]
        public IHttpActionResult SearchAirports(string search)
        {
            lock (_locked)
            {
                var theAirport = search.ToLower().Trim();
                var result = new List<Airport>();

                foreach (var flight in FlightStorage.AllFlights)
                {
                    if (flight.From.AirportName.ToLower().Contains(theAirport) ||
                        flight.From.City.ToLower().Contains(theAirport) ||
                        flight.From.Country.ToLower().Contains(theAirport))
                    {
                        result.Add(flight.From);
                    }

                    if (flight.To.AirportName.ToLower().Contains(theAirport) ||
                        flight.To.City.ToLower().Contains(theAirport) ||
                        flight.To.Country.ToLower().Contains(theAirport))
                    {
                        result.Add(flight.To);
                    }
                }

                return result.Count == 0 ? (IHttpActionResult) NotFound() : Ok(result);
            }
        }

        [Route("api/flights/{id}")]
        [HttpGet]
        public IHttpActionResult FindFlightById(int id)
        {
            lock (_locked)
            {
                var flight = FlightStorage.FindFlight(id);
                if (flight == null)
                {
                    return NotFound();
                }

                return Ok(flight);
            }
        }

        [Route("api/flights/search")]
        [HttpPost]
        public IHttpActionResult SearchFlight(SearchFlightsRequest request)
        {
            lock (_locked)
            {
                if (SearchFlightsRequest.IsNotAValidRequest(request))
                {
                    return BadRequest();
                }

                var result = new PageResult();
                
                foreach (var flight in FlightStorage.AllFlights)
                {
                    if (flight.From.AirportName == request.From &&
                        flight.To.AirportName == request.To &&
                        flight.DepartureTime.Substring(0, 10) == request.DepartureDate)
                    {
                        result.TotalItems++;
                        result.Items.Add(flight);
                    }
                }

                return Ok(result);
            }
        }
    }
}
