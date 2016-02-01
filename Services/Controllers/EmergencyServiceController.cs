namespace Services.Controllers
{
    using Data;
    using System;
    using System.Web.Mvc;
    public class EmergencyServiceController : Controller
    {
        public string Callout(Guid token, string coordinates, string route)
        {
            return new EmergencyService().CreateOrUpdateLocation(token, coordinates, route);
        }

        public void Finished(Guid token, string route)
        {
            new EmergencyService().FinishCallout(token, route);
        }
    }
}
