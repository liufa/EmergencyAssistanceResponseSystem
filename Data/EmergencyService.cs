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
        public string CreateOrUpdateLocation(Guid crewToken, string location, string route)
        {
            Callout callout = new Callout();
            using (var db = new EarsEntities())
            {
                var crew = db.Crew.SingleOrDefault(o => o.Id == crewToken);
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
                            var users1000 = users.Skip(i * 1000).Take(1000).ToList();
                            sender.Send(users1000.Select(o => o.GcmUserId).ToList());
                            users1000.ForEach(o => { o.IsActive = true; db.Entry(o).Property(oo => oo.IsActive).IsModified = true; });
                            db.SaveChanges();
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
                var crewsOnCallout = db.Callout.Where(o => o.Location.Distance(coord) < 260 && !o.IsFinished);
                var raiseAlarm = crewsOnCallout.Any();

                var user = db.Users.Single(o => o.GcmUserId == token);
                if (user.LastNotified.HasValue && user.LastNotified.Value.AddMinutes(1) > DateTime.Now)
                {
                    raiseAlarm = false;
                }
                if(raiseAlarm) {
                    user.LastNotified = DateTime.Now;
                }
                user.Location = coord;
                user.LastSeenOn = DateTime.Now;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return raiseAlarm;
            }
        }

        public List<Users> GetUsersToSendUserNotifications(EarsEntities db, DbGeography location)
        {
          //  var threeMinutesAgo = DateTime.Now.AddMinutes(-3);
            return db.Users/*.Where(o => o.LastSeenOn < threeMinutesAgo)*/.ToList();
        }

        public void FinishCallout(Guid token, string route)
        {
            using (var db = new EarsEntities())
            {
                var crew = db.Crew.Include(o => o.Callout).SingleOrDefault(o => o.Id == token);
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
            var existingRoute = db.Callout.FirstOrDefault(o => o.Crew == crew.Id && !o.IsFinished);
            if (existingRoute!=null)
            {
                FinishCallout(crew.Id, existingRoute.Route);
            }

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

        public Guid AddCrew(string applicationId, string coordinates)
        {
            using (var db = new EarsEntities())
            {
                var user = db.Crew.SingleOrDefault(o => o.ApplicationId == applicationId);
                if (user == null)
                {
                    db.Crew.Add(new Crew { CreatedOn = DateTime.Now, ApplicationId = applicationId, LastSeenOn = DateTime.Now, Location = coordinates.ToDbGeography(), Id = Guid.NewGuid() });
                    db.SaveChanges();
                    user = db.Crew.Single(o => o.ApplicationId == applicationId);
                }
                else {
                    user.Location = coordinates.ToDbGeography();
                    db.Entry(user).Property(o => o.Location).IsModified = true;
                    db.SaveChanges();
                }

                return user.Id;
            }
        }
    }
}
