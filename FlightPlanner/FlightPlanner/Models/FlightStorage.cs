using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using FlightPlanner.DbContext;

namespace FlightPlanner.Models
{
    public static class FlightStorage
    {
        private static int _id;

        public static Flight AddFlight(Flight newFlight)
        {  
            using (var ctx = new FlightPlannerDbContext())
            { 
                newFlight.Id = _id; 
                _id++;
                ctx.Flights.Add(newFlight);
            }

            return newFlight;
        }

        public static Flight FindFlight(int id)
        {
            using (var ctx = new FlightPlannerDbContext())
            {
                return ctx.Flights.Include(f => f.From).Include(f => f.To).SingleOrDefault(x => x.Id == id);
            }
        }

      
    }
}