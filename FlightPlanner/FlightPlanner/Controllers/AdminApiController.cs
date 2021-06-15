using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using FlightPlanner.Attributes;
using FlightPlanner.Models;

namespace FlightPlanner.Controllers
{
    [BasicAuthentification]
    public class AdminApiController : ApiController
    {
        [Route("admin-api/flights/{id}")]
        public IHttpActionResult GetFlights(int id)
        {
            var flight = FlightStorage.FindFlight(id);
            return flight == null ? (IHttpActionResult) NotFound() : Ok(flight);
        }

        [Route("admin-api/flights")]
        public IHttpActionResult PutFlight(AddFlightRequest newFlight)
        {
            if (string.IsNullOrEmpty(newFlight.Carrier) || 
                newFlight.From == null || 
                newFlight.To == null ||
                string.IsNullOrEmpty(newFlight.DepartureTime) || 
                string.IsNullOrEmpty(newFlight.ArrivalTime) ||
                string.IsNullOrEmpty(newFlight.To?.AirportName) || 
                string.IsNullOrEmpty(newFlight.To?.City) || 
                string.IsNullOrEmpty(newFlight.To?.Country) || 
                string.IsNullOrEmpty(newFlight.From?.AirportName)|| 
                string.IsNullOrEmpty(newFlight.From?.City)|| 
                string.IsNullOrEmpty(newFlight.From?.Country))
            {
                return BadRequest();
            }

            var flightExists = FlightStorage.AllFlights.Any(x => x.From.AirportName == newFlight.From.AirportName &&
                                                                 x.To.AirportName == newFlight.To.AirportName &&
                                                                 x.DepartureTime == newFlight.DepartureTime &&
                                                                 x.ArrivalTime == newFlight.ArrivalTime);
            if (flightExists)
            {
                return Conflict();
            }
            else if (newFlight.From.AirportName.ToLower().Trim() == newFlight.To.AirportName.ToLower().Trim())
            {
                return BadRequest();
            }

            if (Convert.ToDateTime(newFlight.DepartureTime) >= Convert.ToDateTime(newFlight.ArrivalTime))
            {
                return BadRequest();
            }

            Flight output = new Flight();
            output.ArrivalTime = newFlight.ArrivalTime;
            output.DepartureTime = newFlight.DepartureTime;
            output.From = newFlight.From;
            output.To = newFlight.To;

            output.Carrier = newFlight.Carrier;

            FlightStorage.AddFlight(output);

            return Created("", output);
        }

        [Route("admin-api/flights/{id}")]
        public IHttpActionResult DeleteFlight(int id)
        {
            var flightToRemove = FlightStorage.AllFlights.SingleOrDefault(x => x.Id == id);
            if (flightToRemove != null)
            {
                FlightStorage.AllFlights.Remove(flightToRemove);
            }

            return Ok();
        }
    }
}
