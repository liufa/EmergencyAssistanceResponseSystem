using System.Web.Mvc;

namespace Services.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public bool CheckForEmergency(string token, string coordinates)
        {
            var data = new Data.EmergencyService();
            var result = data.CheckForEmergency(token, coordinates);
            return result;
        }
    }
}