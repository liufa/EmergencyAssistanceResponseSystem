using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Data.Entity.Spatial;

namespace Services.Controllers
{
    public class EmergencyServiceController : Controller
    {
        public ActionResult Callout(int crewId,  string coordinates, string route)
        {
            var coordinateArray = coordinates.Split(new[] { ',' });
            var location = string.Format("POINT({0} {1})", coordinateArray[0], coordinateArray[1]);
            new EmergencyService().CreateOrUpdateLocation(crewId, location, route);

            return null;
        }
    }
}
