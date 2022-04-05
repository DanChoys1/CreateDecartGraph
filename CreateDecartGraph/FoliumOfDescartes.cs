using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Graphics
{
    public static class FoliumOfDescartes
    {
        public static PointD[] PointsArray(double a = 4, double xBorder = 10, double step = 0.001)
        {
            if (step <= 0)
            {
                throw new ArgumentException("Step cannot be less than or equal to zero.");
            }

            List<PointD> firstQuarterChart = new List<PointD>();
            List<PointD> secondQuarterChart = new List<PointD>();
            List<PointD> thirdQuarterChart = new List<PointD>();
            List<PointD> fourthQuarterChart = new List<PointD>();

            for (double angle = 0; angle < Math.PI; angle += step)
            {
                double r = (2 * a * Math.Cos(angle) * Math.Sin(angle)) /
                    (Math.Pow(Math.Cos(angle), 3) + Math.Pow(Math.Sin(angle), 3));

                double x = Math.Round(r * Math.Cos(angle), 2);
                double y = Math.Round(r * Math.Sin(angle), 2);

                if (Math.Abs(x) > xBorder)
                {
                    continue;
                }

                if (x >= 0 && y >= 0)
                {
                    firstQuarterChart.Add(new PointD(x, y));
                }
                else if(x <= 0 && y >= 0)
                {
                    secondQuarterChart.Add(new PointD(x, y));
                }         
                else if (x >= 0 && y <= 0)
                {
                    fourthQuarterChart.Add(new PointD(x, y));
                }
                else if (y <= 0 && x <= 0)
                {
                    thirdQuarterChart.Add(new PointD(x, y));
                }
            }

            List<PointD> graphicPoints;

            if (a >= 0)
            {
                graphicPoints = new List<PointD>(secondQuarterChart);
                graphicPoints.AddRange(firstQuarterChart);
                graphicPoints.AddRange(fourthQuarterChart);
            }
            else
            {
                graphicPoints = new List<PointD>(fourthQuarterChart);
                graphicPoints.AddRange(thirdQuarterChart);
                graphicPoints.AddRange(secondQuarterChart);
            }

            return graphicPoints.ToArray();
        }
    }
}
