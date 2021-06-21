using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FlightPlanner.Models;

namespace FlightPlanner.Controllers
{
    public class CustomerController : ApiController
    {
        private static readonly object _locker = new object();

        [Route("api/airports")]
        [HttpGet]
        public IHttpActionResult SearchAirports(string search)
        {
            var theAirport = AirportStorage.FindAirport(search);
            if (theAirport == null)
            {
                return NotFound();
            }

            return Ok(theAirport);
        }

        [Route("api/flights/{id}")]
        [HttpGet]
        public IHttpActionResult FindFlightById(int id)
        {
            var flight = FlightStorage.FindFlight(id);
            if (flight == null )
            {
                return NotFound();
            }

            return Ok(flight);
        }

        [Route("api/flights/search")]
        [HttpPost]
        public IHttpActionResult SearchFlight(SearchFlightsRequest request)
        {
            lock (_locker)
            {
                if (request == null ||
                    string.IsNullOrEmpty(request.From) ||
                    string.IsNullOrEmpty(request.To) ||
                    string.IsNullOrEmpty(request.DepartureDate) ||
                    request.From == request.To)
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
