using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class EmergencyService
    {
        public void CreateOrUpdateLocation(int crewId, string location, string route)
        {
            using (var db = new EarsEntities())
            {
                var callout = db.Callout.FirstOrDefault(o => o.Route == route);
                if (callout != null)
                {
                    callout.Location = DbGeography.FromText(location);
                    db.Entry(callout).Property(o => o.Location).IsModified = true;
                }
                else {
                    db.Callout.Add(new Callout { Crew = crewId, Location = DbGeography.FromText(location), Route = route });
                }
                db.SaveChanges();
            }
        }
    }
}
