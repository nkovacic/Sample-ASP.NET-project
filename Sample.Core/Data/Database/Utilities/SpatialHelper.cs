using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Data.Database
{
    public class SpatialHelper
    {
        public static DbGeography ConvertToDbGeography(double longitude, double latitude)
        {
            var point = string.Format("POINT({1} {0})", latitude.ToString(CultureInfo.InvariantCulture), 
                longitude.ToString(CultureInfo.InvariantCulture));

            return DbGeography.FromText(point);
        }

        public static double CalculateDistance(double pointOneLongitude, double pointOneLatitude, double pointTwoLongitude, double pointTwoLatitude)
        {
            return CalculateDistance(ConvertToDbGeography(pointOneLongitude, pointOneLatitude), ConvertToDbGeography(pointTwoLongitude, pointTwoLatitude));
        }

        public static double CalculateDistance(DbGeography pointOne, double pointTwoLongitude, double pointTwoLatitude)
        {
            return CalculateDistance(pointOne, ConvertToDbGeography(pointTwoLongitude, pointTwoLatitude));
        }

        public static double CalculateDistance(DbGeography pointOne, DbGeography pointTwo)
        {
            return pointOne.Distance(pointTwo).GetValueOrDefault();
        }
    }
}
