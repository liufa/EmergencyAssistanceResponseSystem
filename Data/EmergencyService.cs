namespace Data
{
    using Core;
    using Outgoing;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Data.Entity;
    public class EmergencyService
    {
        public string CreateOrUpdateLocation(string crewToken, string location, string route)
        {
            Callout callout = new Callout();
            using (var db = new EarsEntities())
            {
                var crew = db.Crew.SingleOrDefault(o => o.GoogleUserId == crewToken);
                if (crew != null)
                {

                    if (!string.IsNullOrEmpty(route))
                    {
                        callout = db.Callout.FirstOrDefault(o => o.Route == route);
                        if (callout != null)
                        {
                            callout.Location = location.ToDbGeography();
                            callout.LastSignal = DateTime.Now;
                            db.Entry(callout).Property(o => o.Location).IsModified = true;
                            db.Entry(callout).Property(o => o.LastSignal).IsModified = true;
                        }
                        else
                        {
                            callout = CreateNewCallout(db, crew, location);
                        }
                    }
                    else
                    {
                        callout = CreateNewCallout(db, crew, location);
                    }

                    var users = GetUsersToSendUserNotifications(db, location.ToDbGeography());
                    if (users.Any())
                    {
                        var sender = new GCSNotificationSender();
                        var count = users.Count();
                        for (int i = 0; i < (count / 1000) + 1; i++)
                        {
                            sender.Send(users.Select(o => o.GcmUserId).Skip(i * 1000).Take(1000).ToList());
                        }
                    }

                    callout.LastBroadcast = DateTime.Now;

                    db.SaveChanges();
                }
            }

            return callout.Route;
        }

        public bool CheckForEmergency(string token, string coordinates)
        {
            using (var db = new EarsEntities())
            {
                var coord = coordinates.ToDbGeography();
                var crewsOnCallout = db.Callout.Where(o => o.Location.Distance(coord) < 500000 && !o.IsFinished);
                return crewsOnCallout.Any();
            }
        }

        public List<Users> GetUsersToSendUserNotifications(EarsEntities db, DbGeography location)
        {
            return db.Users.Where(o => o.Location.Distance(location) < 500000).ToList();
        }

        public void FinishCallout(string token, string route)
        {
            using (var db = new EarsEntities())
            {
                var crew = db.Crew.Include(o => o.Callout).SingleOrDefault(o => o.GoogleUserId == token);
                if (crew != null)
                {
                    var callout = crew.Callout.SingleOrDefault(o => o.Route == route);
                    if (callout != null) {
                        callout.IsFinished = true;
                        db.Entry(callout).Property(o => o.IsFinished).IsModified = true;
                        db.SaveChanges();
                    }
                }
            }
        }

        private Callout CreateNewCallout(EarsEntities db, Crew crew, string location)
        {
            var route = Guid.NewGuid().ToString();
            var callout = new Callout { Crew = crew.Id, Location = location.ToDbGeography(), Route = route, IsFinished = false, LastSignal = DateTime.Now };
            db.Callout.Add(callout);
            return callout;
        }

        public bool AddUser(string token, string location)
        {
            using (var db = new EarsEntities())
            {
                var user = db.Users.SingleOrDefault(o => o.GcmUserId == token);
                if (user == null)
                {
                    db.Users.Add(new Users { CreatedOn = DateTime.Now, GcmUserId = token, LastSeenOn = DateTime.Now, Location = location.ToDbGeography() });
                    db.SaveChanges();
                }

                return true;
            }
        }

        public bool AddCrew(string token, string coordinates)
        {
            using (var db = new EarsEntities())
            {
                var user = db.Crew.SingleOrDefault(o => o.GoogleUserId == token);
                if (user == null)
                {
                    db.Crew.Add(new Crew { CreatedOn = DateTime.Now, GoogleUserId = token, LastSeenOn = DateTime.Now, Location = coordinates.ToDbGeography() });
                    db.SaveChanges();
                }

                return true;
            }
        }
    }
}
