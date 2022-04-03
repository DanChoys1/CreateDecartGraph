using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CreateDecartGraph
{
    internal static class FoliumOfDescartes
    {
        public static PointF[] PointsArray(double a = 4, double xBorder = 10, double step = 0.001)
        {
            if (step <= 0)
            {
                throw new ArgumentException("Step cannot be less than or equal to zero.");
            }

            List<PointF> firstQuarterChart = new List<PointF>();
            List<PointF> secondQuarterChart = new List<PointF>();
            List<PointF> thirdQuarterChart = new List<PointF>();
            List<PointF> fourthQuarterChart = new List<PointF>();

            for (double angle = 0; angle < Math.PI; angle += step)
            {
                double r = (2 * a * Math.Cos(angle) * Math.Sin(angle)) /
                    (Math.Pow(Math.Cos(angle), 3) + Math.Pow(Math.Sin(angle), 3));

                float x = (float)(r * Math.Cos(angle));
                float y = (float)(r * Math.Sin(angle));

                if (Math.Abs(x) > xBorder)
                {
                    continue;
                }

                if (x >= 0 && y >= 0)
                {
                    firstQuarterChart.Add(new PointF(x, y));
                }
                else if(x <= 0 && y >= 0)
                {
                    secondQuarterChart.Add(new PointF(x, y));
                }         
                else if (x >= 0 && y <= 0)
                {
                    fourthQuarterChart.Add(new PointF(x, y));
                }
                else if (y <= 0 && x <= 0)
                {
                    thirdQuarterChart.Add(new PointF(x, y));
                }
            }

            List<PointF> graphicPoints;

            if (a >= 0)
            {
                graphicPoints = new List<PointF>(secondQuarterChart);
                graphicPoints.AddRange(firstQuarterChart);
                graphicPoints.AddRange(fourthQuarterChart);
            }
            else
            {
                graphicPoints = new List<PointF>(fourthQuarterChart);
                graphicPoints.AddRange(thirdQuarterChart);
                graphicPoints.AddRange(secondQuarterChart);
            }

            return graphicPoints.ToArray();
        }
    }
}
