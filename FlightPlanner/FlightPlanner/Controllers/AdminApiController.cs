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
        private static readonly object _locker = new object();

        [Route("admin-api/flights/{id}")]
        public IHttpActionResult GetFlights(int id)
        {
            lock (_locker)
            {
                var flight = FlightStorage.FindFlight(id);
                return flight == null ? (IHttpActionResult) NotFound() : Ok(flight);
            }
        }

        [Route("admin-api/flights")]
        [HttpPut]
        public IHttpActionResult PutFlight(AddFlightRequest newFlight)
        {
            lock (_locker)
            {
                if (IsFlightNullOrEmpty(newFlight))
                {
                    return BadRequest();
                }

                if (FlightAlreadyExists(newFlight))
                {
                    return Conflict();
                }
                else if (ToAndFromAirportsAreTheSame(newFlight))
                {
                    return BadRequest();
                }

                if (ArrivalTimeIsAfterDeparture(newFlight))
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
        }

        [Route("admin-api/flights/{id}")]
        public IHttpActionResult DeleteFlight(int id)
        {
            lock (_locker)
            {
                var flightToRemove = FlightStorage.AllFlights.SingleOrDefault(x => x.Id == id);
                if (flightToRemove != null)
                {
                    FlightStorage.AllFlights.Remove(flightToRemove);
                }

                return Ok();
            }
        }

        public bool FlightAlreadyExists(AddFlightRequest newFlight)
        {
            return FlightStorage.AllFlights.Any(x => x.From.AirportName == newFlight.From.AirportName &&
                                                                 x.To.AirportName == newFlight.To.AirportName &&
                                                                 x.DepartureTime == newFlight.DepartureTime &&
                                                                 x.ArrivalTime == newFlight.ArrivalTime);
        }

        public bool IsFlightNullOrEmpty(AddFlightRequest newFlight)
        {
            return string.IsNullOrEmpty(newFlight.Carrier) ||
                newFlight.From == null ||
                newFlight.To == null ||
                string.IsNullOrEmpty(newFlight.DepartureTime) ||
                string.IsNullOrEmpty(newFlight.ArrivalTime) ||
                string.IsNullOrEmpty(newFlight.To?.AirportName) ||
                string.IsNullOrEmpty(newFlight.To?.City) ||
                string.IsNullOrEmpty(newFlight.To?.Country) ||
                string.IsNullOrEmpty(newFlight.From?.AirportName) ||
                string.IsNullOrEmpty(newFlight.From?.City) ||
                string.IsNullOrEmpty(newFlight.From?.Country);
        }

        public bool ToAndFromAirportsAreTheSame(AddFlightRequest newFlight)
        {
            return newFlight.From.AirportName.ToLower().Trim() == newFlight.To.AirportName.ToLower().Trim();
        }

        public bool ArrivalTimeIsAfterDeparture(AddFlightRequest newFlight)
        {
            return Convert.ToDateTime(newFlight.DepartureTime) >= Convert.ToDateTime(newFlight.ArrivalTime);
        }

    }
}
