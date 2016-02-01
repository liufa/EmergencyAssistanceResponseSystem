namespace Services.Controllers
{
    using Data;
    using System.Web.Mvc;
    public class EmergencyServiceController : Controller
    {
        public string Callout(string token,  string coordinates, string route)
        {
            return new EmergencyService().CreateOrUpdateLocation(token, coordinates, route);
        }

        public void Finished(string token, string route)
        {
            new EmergencyService().FinishCallout(token, route);
        }
    }
}
