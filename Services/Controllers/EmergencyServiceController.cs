using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Data.Entity.Spatial;
using Core;

namespace Services.Controllers
{
    public class EmergencyServiceController : Controller
    {
        public ActionResult Callout(int crewId,  string coordinates, string route)
        {

            new EmergencyService().CreateOrUpdateLocation(crewId, coordinates, route);

            return null;
        }
    }
}
