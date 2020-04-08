using Excel;
//using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace ANZHFR.Web.Helpers
{
    public class ImportHelper
    {
        static string[] exportableAttributes = {"Domain","PageValue","BC","CG","CS","CSC","DPOL","DPOLO","Facets","FI",
                                         "H1","MC","MediaItemid","Meta","MM","MMG","MMM","MMO","MMP","PageTemplateCode",
                                         "PD","PDC","PDO","PDP","Title","Type","Url","WTG","WTGS" };

        #region if the excel read as Html
		//public static string[,] ConvertHtmlDocumentToDataTable(HtmlDocument document)
		//{
		//	string[,] dataTable = null;
		//	int row = 0, column = 0;

		//	HtmlNode tableNode = document.DocumentNode.SelectNodes("//table").FirstOrDefault();
		//	if (tableNode != null)
		//	{
		//		List<HtmlNode> allRows = tableNode.Descendants("tr").ToList();

		//		if (allRows.Any())
		//		{
		//			var colCount = allRows[0].Descendants("th").ToList().Count;
		//			dataTable = new string[allRows.Count(), colCount];
		//			List<HtmlNode> allColumns = new List<HtmlNode>();
		//			foreach (var node in allRows)
		//			{
		//				if (row == 0)
		//					allColumns = node.Descendants("th").ToList();
		//				else
		//					allColumns = node.Descendants("td").ToList();

		//				column = 0;
		//				foreach (var cell in allColumns)
		//				{
		//					dataTable[row, column] = cell.InnerText;
		//					column++;
		//				}
		//				row++;
		//			}
		//		}
		//	}

		//	return dataTable;
		//}

        public static List<int> GetChangePositionForImportRecord(string[,] dataSource, int rowLength)
        {
            List<int> retIndexes = new List<int>();
            string[] dataSourceHeader = new string[rowLength];

            for (int i = 0; i < rowLength; i++)
            {
                dataSourceHeader[i] = dataSource[0, i].ToLower();
            }

            if (dataSourceHeader.Contains("pageid"))
            {
                retIndexes.Add(dataSourceHeader.ToList().FindIndex(p => p.ToLower().Equals("pageid")));
            }

            foreach (var token in exportableAttributes)
            {
                var position = dataSourceHeader.ToList().FindIndex(p => p.ToLower().Equals(token.ToLower()));
                if (position > -1)
                {
                    retIndexes.Add(position);
                }
            }

            return retIndexes;
        }
        #endregion

        #region If excel read as original Format
        public static IEnumerable<K> Parse<K>(string fileName, string workSheetName) where K : class
        {
            IEnumerable<K> list = new List<K>();
            string connectionString = string.Format("provider=Microsoft.Jet.OLEDB.4.0; data source={0};Extended Properties=Excel 8.0;", fileName);
            string query = string.Format("SELECT * FROM [{0}$]", workSheetName);

            DataSet data = new DataSet();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
                adapter.Fill(data);
                list = PopulateData<K>(data);
            }

            return list;
        }

        private static List<T> PopulateData<T>(DataSet data) where T : class
        {
            List<T> dtos = new List<T>();

            foreach (DataRow row in data.Tables[0].Rows)
            {
                T dto = Activator.CreateInstance<T>();

                PopulateFieldsFromDataRows(row, dto);
                dtos.Add(dto);
            }
            return dtos;
        }

        private static void PopulateFieldsFromDataRows(DataRow row, object o)
        {
            foreach (DataColumn col in row.Table.Columns)
            {

                string name = col.ColumnName;
                System.Reflection.FieldInfo field = o.GetType().GetField(name);
                if (field == null)
                {
                    PropertyInfo prop = o.GetType().GetProperty(name);
                    if (prop != null)
                    {
                        if (prop.CanWrite)
                        {
                            if (prop.PropertyType.Equals(typeof(DateTime)))
                            {
                                DateTime d = (DateTime)row[name];
                                if (d.Equals(new DateTime(1900, 1, 1)))
                                {
                                    d = DateTime.MinValue;
                                }

                                prop.SetValue(o, d, null);
                            }
                            else if (prop.PropertyType.Equals(typeof(int)))
                            {
                                prop.SetValue(o, Convert.ToInt32(row[name]), null);
                            }
                            else if (prop.PropertyType.Equals(typeof(decimal)))
                            {
                                prop.SetValue(o, Convert.ToDecimal(row[name]), null);
                            }
                            else
                            {
                                string value = row[name].ToString();
                                prop.SetValue(o, value, null);
                            }
                        }
                    }
                }
                else
                {
                    if (field.FieldType.Equals(typeof(DateTime)))
                    {
                        DateTime d = (DateTime)row[name];
                        if (d.Equals(new DateTime(1900, 1, 1)))
                        {
                            d = DateTime.MinValue;
                        }

                        field.SetValue(o, d);
                    }
                    else if (field.FieldType.Equals(typeof(int)))
                    {
                        field.SetValue(o, Convert.ToInt32(row[name]));
                    }
                    else if (field.FieldType.Equals(typeof(decimal)))
                    {
                        field.SetValue(o, Convert.ToDecimal(row[name]));
                    }
                    else if (field.FieldType.Equals(typeof(bool)))
                    {
                        if (row[name] == DBNull.Value)
                        {
                            field.SetValue(o, false);
                        }
                        else
                        {
                            field.SetValue(o, Convert.ToBoolean(row[name]));
                        }
                    }
                    else
                    {
                        field.SetValue(o, row[name]);
                    }
                }
            }
        }
        #endregion

        #region using Excel Data Reader

        public static string[,] ParseUsingExceldataReader(string filePath)
        {
            string[,] dataTable = null;
            string ext = Path.GetExtension(filePath);

            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = null;

            if(ext == ".xls")
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            else
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            
            excelReader.IsFirstRowAsColumnNames = false;
            DataSet result = excelReader.AsDataSet();

            foreach (DataTable table in result.Tables)
            {
                dataTable = new string[table.Rows.Count, table.Columns.Count];

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                        dataTable[i, j] = table.Rows[i].ItemArray[j].ToString();                        
                }
            }

            excelReader.Close();
            return dataTable;
		}
        
		//public static string[,] ParseUsingExceldataReader(Stream stream, string ext)
		//{
		//	string[,] dataTable = null;
		//	IExcelDataReader excelReader = null;

		//	if (ext == ".xls")
		//		excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
		//	else
		//		excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

		//	excelReader.IsFirstRowAsColumnNames = false;
		//	DataSet result = excelReader.AsDataSet();

		//	foreach (DataTable table in result.Tables)
		//	{
		//		dataTable = new string[table.Rows.Count, table.Columns.Count];

		//		for (int i = 0; i < table.Rows.Count; i++)
		//		{
		//			for (int j = 0; j < table.Columns.Count; j++)
		//				dataTable[i, j] = table.Rows[i].ItemArray[j].ToString();
		//		}
		//	}

		//	excelReader.Close();
		//	return dataTable;
		//}
			
        
        public static List<List<Dictionary<string, string>>> ParseUsingExceldataReader(Stream stream, string ext)
        {
			List<List<Dictionary<string, string>>> dataTable = new List<List<Dictionary<string, string>>>();

			IExcelDataReader excelReader = null;

            if (ext == ".xls")
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            else
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            excelReader.IsFirstRowAsColumnNames = false;
            DataSet result = excelReader.AsDataSet();

			if (result.Tables.Count > 0)
			{
				DataTable table = result.Tables[0];
				DataRow headerRow = table.Rows[int.Parse(ConfigurationManager.AppSettings["HeaderRow"]) - 1];
				int dataRow = int.Parse(ConfigurationManager.AppSettings["DataRow"]) - 1;

				for (int i = dataRow; i < table.Rows.Count; i++)
				{
					List<Dictionary<string, string>> dl = new List<Dictionary<string, string>>();

                    if (!string.IsNullOrWhiteSpace(table.Rows[i].ItemArray[2].ToString()))
                    {
                        for (int j = 0; j < headerRow.ItemArray.Length; j++)
                        {
                            Dictionary<string, string> d = new Dictionary<string, string>();
                            string columnName = headerRow.ItemArray[j].ToString();
                            string columnData = table.Rows[i].ItemArray[j].ToString();

                            d.Add(columnName, columnData);

                            dl.Add(d);
                        }

                        dataTable.Add(dl);
                    }
				}
			}

            excelReader.Close();
            return dataTable;
        }

        #endregion
    }
}
