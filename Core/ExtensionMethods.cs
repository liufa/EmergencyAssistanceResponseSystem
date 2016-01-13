using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;
namespace Core
{
   public static class ExtensionMethods
    {

        public static DbGeography ToDbGeography(this string coordinates)
        {
            var coordinateArray = coordinates.Split(new[] { ',' });
            var location = string.Format("POINT({0} {1})", coordinateArray[0], coordinateArray[1]);
            return DbGeography.FromText(location);
        }
    }
}
