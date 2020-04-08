using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ANZHFR.Services.Patients;
using ANZHFR.Web.Models;
using System.IO;
using System.Web.UI;
using System.Data;
using OfficeOpenXml;

using System.Configuration;

using ANZHFR.Web.ExtensionMethods;

namespace ANZHFR.Web.Helpers
{
    public class ExportHelper
    {
        public static void Export(string fileFormat, List<ExportPatientModel> resultList, HttpResponseBase Response)
        {
            #region code from Ashraf bhai

            //var grid = new System.Web.UI.WebControls.GridView();
            //grid.AllowPaging = false;

            //if (resultList.Count > 0)
            //{
            //	grid.DataSource = resultList;
            //	grid.DataBind();

            //	ExportHelper.ExportUsingExcel(grid, Response);
            //}

            //Response.End();

            #endregion code from Ashraf bhai

            string template = "";
            if (ConfigurationManager.AppSettings["Location"] == "Australian")
                template = string.Format("Templates\\export-patient-{0}.xlsx", fileFormat);
            else
                template = string.Format("Templates\\export-patient-NZ-{0}.xlsx", fileFormat);

            FileInfo tpl = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + template);
            DataTable dt = resultList.ConvertToDatatable(fileFormat);

            using (ExcelPackage xlPackage = new ExcelPackage(tpl, true))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Patients"];
                //const int startRow = 2;
                int startRow = int.Parse(ConfigurationManager.AppSettings["DataRow"]);
                int rowIndex = startRow;

                foreach (DataRow row in dt.Rows)
                {
                    int col = 1;
                    if (rowIndex > startRow)
                    {
                        worksheet.InsertRow(rowIndex, 1);
                    }

                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        if (row.ItemArray.GetValue(i) != null)
                        {
                            worksheet.Cells[rowIndex, col].Value = row.ItemArray.GetValue(i).ToString();
                        }

                        col++;
                    }

                    rowIndex++;
                }

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Patients.xlsx");

                //Write the file
                Response.BinaryWrite(xlPackage.GetAsByteArray());
                Response.End();
            }

            //using (ExcelPackage pck = new ExcelPackage(tpl))
            //{
            //	//Give the worksheet a name
            //	ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");

            //	ws.Cells["A2"].LoadFromDataTable(dt, true);

            //	//End Format the header columns
            //	//Give the file details(ie. filename, etc.)
            //	Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //	Response.AddHeader("content-disposition", "attachment;filename=Patients.xlsx");

            //	//Write the file
            //	Response.BinaryWrite(pck.GetAsByteArray());
            //	Response.End();

            //	pck.Dispose();
            //}
        }

        public static int getAge(DateTime? bday, DateTime? Arriv, DateTime? Inpat, DateTime? Transfer)
        {
            if (bday == null || (Arriv == null & Inpat == null & Transfer == null))
            {
                return 0;
            }
            DateTime now = DateTime.Today;
            DateTime birthday = bday.Value;
            if (Arriv == null || Arriv.Value.Year == 1900)
            {
                if (Inpat == null || Inpat.Value.Year == 1900)
                {
                    now = Transfer.Value;
                }
                else
                {
                    now = Inpat.Value;
                }
            }
            else
            { now = Arriv.Value; }

            int age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age)) age--;

            return age;
        }

        public static string getDate(DateTime? d)
        {
            if (d == null || d.Value.Year == 1900 || d.Value.Year < 100)
            {
                return string.Empty;
            }

            return d.Value.ToString("dd/MM/yyyy");
            //return d.Value.ToString("MM-dd-yyyy");
        }

        public static string getTime(DateTime? d)
        {
            if (d == null || d.Value.Year == 1900)
            {
                return string.Empty;
            }
            // Changed to 24 hour time.
            //return d.Value.ToString("hh:mm tt");
            return d.Value.ToString("HH:mm");
        }

        public static int getDiffDays(DateTime date1, DateTime date2)
        {
            int days = (int)(date1 - date2).TotalDays;

            return days;
        }

        public static short getOHLength(DateTime? _wardDischarge, DateTime? _arrivalDate)
        {
            if (_wardDischarge == null || _arrivalDate == null)
            {
                return 0;
            }

            try
            {
                DateTime wardDischarge = DateTime.Parse(_wardDischarge.ToString());
                DateTime arrivalDate = DateTime.Parse(_arrivalDate.ToString());
                return short.Parse(getDiffDays(wardDischarge, arrivalDate).ToString());
            }
            catch
            {
                return 0;
            }
        }

        public static void ExportUsingExcel(System.Web.UI.WebControls.GridView grid, HttpResponseBase Response)
        {
            grid.AllowPaging = false;
            grid.AllowSorting = false;

            Response.Clear();

            Response.BinaryWrite("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">".ToByteArray());
            Response.BinaryWrite("<head>".ToByteArray());
            Response.BinaryWrite("<!--[if gte mso 9]><xml>".ToByteArray());
            Response.BinaryWrite("<x:ExcelWorkbook>".ToByteArray());
            Response.BinaryWrite("<x:ExcelWorksheets>".ToByteArray());
            Response.BinaryWrite("<x:ExcelWorksheet>".ToByteArray());
            Response.BinaryWrite("<x:Name>PageXML</x:Name>".ToByteArray());
            Response.BinaryWrite("<x:WorksheetOptions>".ToByteArray());
            Response.BinaryWrite("<x:Print>".ToByteArray());
            Response.BinaryWrite("<x:ValidPrinterInfo/>".ToByteArray());
            Response.BinaryWrite("</x:Print>".ToByteArray());
            Response.BinaryWrite("</x:WorksheetOptions>".ToByteArray());
            Response.BinaryWrite("</x:ExcelWorksheet>".ToByteArray());
            Response.BinaryWrite("</x:ExcelWorksheets>".ToByteArray());
            Response.BinaryWrite("</x:ExcelWorkbook>".ToByteArray());
            Response.BinaryWrite("</xml>".ToByteArray());
            Response.BinaryWrite("<![endif]--> ".ToByteArray());
            Response.BinaryWrite("</head>".ToByteArray());
            Response.BinaryWrite("<body>".ToByteArray());
            Response.AddHeader("content-disposition", "attachment; filename=PageXMLData.xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.BinaryWrite(sw.ToString().ToByteArray());
            Response.BinaryWrite("</body>".ToByteArray());
            Response.BinaryWrite("</html>".ToByteArray());
            Response.End();
        }
    }
}