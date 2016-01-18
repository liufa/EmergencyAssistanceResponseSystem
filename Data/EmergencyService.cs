using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

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
                    callout.Location = location.ToDbGeography();
                    db.Entry(callout).Property(o => o.Location).IsModified = true;
                }
                else {
                    db.Callout.Add(new Callout { Crew = crewId, Location = location.ToDbGeography(), Route = route });
                }
                db.SaveChanges();
            }
        }

        public void CreateOrUpdateLocation(int crewId, object p, string route)
        {
            throw new NotImplementedException();
        }

        public bool AddUser(string token, string location)
        {
            using (var db = new EarsEntities())
            {
                var user = db.Users.SingleOrDefault(o => o.GcmUserId == token);
                if (user != null)
                {
                    db.Users.Add(new Users { CreatedOn = DateTime.Now, GcmUserId = token, LastSeenOn = DateTime.Now, Location = location.ToDbGeography() });
                    db.SaveChanges();
                }

                return true;
            }
        }
    }
}
