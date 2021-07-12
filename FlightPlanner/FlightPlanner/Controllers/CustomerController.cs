using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using FlightPlanner.DbContext;
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
                using (var ctx = new FlightPlannerDbContext())
                {
                    foreach (var airport in ctx.Airports)
                    {
                        if (airport.AirportName.ToLower().Contains(theAirport) ||
                            airport.City.ToLower().Contains(theAirport) ||
                            airport.Country.ToLower().Contains(theAirport))
                        {
                            result.Add(airport);
                        }
                    }

                    return result.Count == 0 ? (IHttpActionResult)NotFound() : Ok(result);
                }
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
                using (var ctx = new FlightPlannerDbContext())
                {
                    foreach (Flight flight in ctx.Flights.Include(f => f.From).Include(f => f.To).ToList())
                    {
                        if (flight.From.AirportName == request.From &&
                            flight.To.AirportName == request.To &&
                            flight.DepartureTime.Substring(0, 10) == request.DepartureDate)
                        {
                            result.TotalItems++;
                            result.Items.Add(flight);
                        }
                    }
                }

                return Ok(result);
            }
        }
    }
}
