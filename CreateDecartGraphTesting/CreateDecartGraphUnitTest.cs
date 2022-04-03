using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;

using Graphics;

namespace CreateDecartGraphTesting
{
    [TestClass]
    public class CreateDecartGraphUnitTest
    {
        [TestMethod]
        public void PointsArrayTest()
        {
            // Arrange
            double a = 4;
            double xBorder = 10;
            double step = 0.001;

            PointF[] correctPoints = CorrectPointsArrayForGraphicFoliumOfDescartes(a, xBorder, step);

            // Act
            PointF[] resultPoints = FoliumOfDescartes.PointsArray(a, xBorder, step);

            // Assert
            Assert.AreEqual(correctPoints.Length, resultPoints.Length);

            for (int i = 0; i < resultPoints.Length; i++)
            {
                Assert.AreEqual(correctPoints[i], resultPoints[i]);
            }
        }

        private static PointF[] CorrectPointsArrayForGraphicFoliumOfDescartes(double a = 4, double xBorder = 10, double step = 0.001)
        {
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
                else if (x <= 0 && y >= 0)
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