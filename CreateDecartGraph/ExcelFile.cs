using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;
using Graphics;

namespace Files
{
    internal static class ExcelFile
    {
        private readonly static int _startRowForPoints = 3;
        private readonly static int _xColumnIndex = 2;
        private readonly static int _yColumnIndex = 5;
                       
        private readonly static int _rowParametrsIndex = 1;
        private readonly static int _aColumnIndex = 1;
        private readonly static int _xBorderColumnIndex = 4;
        private readonly static int _stepColumnIndex = 7;

        public static bool Read(string path, out double a, out double xBorder, out double step)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                FileInfo fi = new FileInfo(path);

                using (ExcelPackage excelPackage = new ExcelPackage(fi))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                    a = (double)worksheet.Cells[_rowParametrsIndex, _aColumnIndex + 1].Value;
                    xBorder = (double)worksheet.Cells[_rowParametrsIndex, _xBorderColumnIndex + 1].Value;
                    step = (double)worksheet.Cells[_rowParametrsIndex, _stepColumnIndex + 1].Value;

                    excelPackage.Save();
                }
            }
            catch (Exception)
            {
                a = 0;
                xBorder = 0;
                step = 0;

                return false;
            }

            return true;
        }
  
        public static bool Write(string path, PointD[] points, double a, double xBorder, double step)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "DanChoys";
                    excelPackage.Workbook.Properties.Title = "Графики";
                    excelPackage.Workbook.Properties.Subject = "Построение графиков";
                    excelPackage.Workbook.Properties.Created = DateTime.Now;
                    
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Декартов лист");
                    
                    worksheet.Cells[_rowParametrsIndex, _aColumnIndex].Value = "a =";
                    worksheet.Cells[_rowParametrsIndex, _aColumnIndex + 1].Value = a;
                    worksheet.Cells[_rowParametrsIndex, _xBorderColumnIndex].Value = "xBorder =";
                    worksheet.Cells[_rowParametrsIndex, _xBorderColumnIndex + 1].Value = xBorder;
                    worksheet.Cells[_rowParametrsIndex, _stepColumnIndex].Value = "step =";
                    worksheet.Cells[_rowParametrsIndex, _stepColumnIndex + 1].Value = step;

                    worksheet.Cells[_startRowForPoints, _xColumnIndex].Value = "X";
                    worksheet.Cells[_startRowForPoints, _yColumnIndex].Value = "Y";

                    for (int i = 0; i < points.Length; i++)
                    {
                        worksheet.Cells[_startRowForPoints + i + 1, _xColumnIndex].Value = points[i].X;
                        worksheet.Cells[_startRowForPoints + i + 1, _yColumnIndex].Value = points[i].Y;
                    }

                    /* var capitalizationChart = worksheet.Drawings.AddChart("FindingsChart", OfficeOpenXml.Drawing.Chart.eChartType.Line);
                     capitalizationChart.Title.Text = "Декартов Граф";
                     capitalizationChart.SetPosition(10, 0, 5, 0);
                     capitalizationChart.SetSize(800, 400);
                     capitalizationChart.Series.Add(worksheet.Cells[_startRowForPoints + 1, _xColumnIndex, _startRowForPoints + points.Length, _yColumnIndex]);
 */

                    OfficeOpenXml.Drawing.Chart.ExcelLineChart lineChart = 
                        worksheet.Drawings.AddChart("lineChart", OfficeOpenXml.Drawing.Chart.eChartType.Line) as OfficeOpenXml.Drawing.Chart.ExcelLineChart;

                    lineChart.Title.Text = "LineChart Example";

                    var rangeLabel = worksheet.Cells["A1:A1"];
                    var range1 = worksheet.Cells[_startRowForPoints + 1, _xColumnIndex];
                    var range2 = worksheet.Cells[_startRowForPoints + 100, _yColumnIndex];

                    lineChart.Series.Add(range1, rangeLabel);
                    lineChart.Series.Add(range2, rangeLabel);

                    /*lineChart.Series[0].Header = worksheet.Cells["A2"].Value.ToString();
                    lineChart.Series[1].Header = worksheet.Cells["A3"].Value.ToString();*/
                    
                    //lineChart.Legend.Position = OfficeOpenXml.Drawing.Chart.eLegendPosition.Right;
                    
                    lineChart.SetSize(600, 300);
                    lineChart.SetPosition(20, 20, 20, 20);


                    FileInfo fi = new FileInfo(path);
                    excelPackage.SaveAs(fi);
                }
            }
            catch (IOException)
            {
                return false;
            }

            return true;
        }
    }
}
