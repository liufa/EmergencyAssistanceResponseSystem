using System.Data.Entity.Spatial;
namespace Core
{
    public static class ExtensionMethods
    {
        public static DbGeography ToDbGeography(this string coordinates, bool longFirst = false)
        {
            var coordinateArray = coordinates.Split(new[] { ',' });
            var location = longFirst ? string.Format("POINT({0} {1})", coordinateArray[1], coordinateArray[0]) : string.Format("POINT({0} {1})", coordinateArray[0], coordinateArray[1]);
            return DbGeography.FromText(location);
        }
    }
}
