using Data;
using System.Web.Mvc;

namespace Services.Controllers
{
    public class EmergencyServiceController : Controller
    {
        public string Callout(string token,  string coordinates, string route)
        {
            return new EmergencyService().CreateOrUpdateLocation(token, coordinates, route);
        }

        public string Finished(string token, string route)
        {
            return new EmergencyService().FinishCallout(token, route);
        }
    }
}
