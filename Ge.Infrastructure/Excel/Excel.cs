using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.Web;
using Aspose.Cells;

namespace Ge.Infrastructure.Excel
{
    public class Excel
    {
        /// <summary>
        /// 将Excel总的数据转化为datatable
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sExcelSql">Excel工作表查询语句</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filePath, string sExcelSql)
        {
            DataTable dt = new DataTable();
            try
            {
                //根据路径打开一个Excel文件并将数据填充到DataSet中
                //var strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + filePath + ";Extended Properties='Excel 8.0;;HDR=YES;IMEX=1'";//HDR=YES 有两个值:YES,表示第一行是否字段名,默认是YES,第一行是字段名
                string strConn = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
                var conn = new OleDbConnection(strConn);
                conn.Open();
                //获取所有表
                var dtt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //得到第一个工作表的名称
                var sheetName = dtt.Rows[0][2].ToString().Trim();
                //cmd查询
                if (sExcelSql == "")
                {
                    sExcelSql = "Select * FROM" + " [" + sheetName + "]";
                }
                else
                {
                    sExcelSql += @" FROM" + " [" + sheetName + "]";

                }

                OleDbDataAdapter myCommand = null;
                DataSet ds = null;
                myCommand = new OleDbDataAdapter(sExcelSql, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");

                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {

            }

            return dt;
        }

        /// <summary>
        /// 将Excel总的数据转化为datatable
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sExcelSql">Excel工作表查询语句</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filePath, string sExcelSql, ref string returnInfo)
        {
            DataTable dt = new DataTable();
            try
            {
                //根据路径打开一个Excel文件并将数据填充到DataSet中
                //var strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + filePath + ";Extended Properties='Excel 8.0;;HDR=YES;IMEX=1'";//HDR=YES 有两个值:YES,表示第一行是否字段名,默认是YES,第一行是字段名
                string strConn = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1'";
                var conn = new OleDbConnection(strConn);
                conn.Open();
                //获取所有表
                var dtt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //得到第一个工作表的名称
                var sheetName = dtt.Rows[0][2].ToString().Trim();
                //cmd查询
                if (sExcelSql == "")
                {
                    sExcelSql = "Select * FROM" + " [" + sheetName + "]";
                }
                else
                {
                    sExcelSql += @" FROM" + " [" + sheetName + "]";

                }

                OleDbDataAdapter myCommand = null;
                DataSet ds = null;
                myCommand = new OleDbDataAdapter(sExcelSql, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");

                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                returnInfo = ex.Message;
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 将DataTable转化为Excel
        /// </summary>
        /// <param name="sFileName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool DataTableToExcel(string sFileName, DataTable dt)
        {
            if (dt == null) return false;
            #region 生成Excel格式的XML头部

            //StreamWriter sw1 = new StreamWriter("bb.txt");
            //StreamReader sr1 = new StreamReader("aa.txt");
            //string sLine = "";
            //while ((sLine = sr1.ReadLine()) != "")
            //{
            //    sLine = sLine.Replace("\"", "\\\"");
            //    sw1.WriteLine("sw.WriteLine(\"" + sLine + "\");");
            //}
            //sw1.Close();
            //sr1.Close();

            #endregion 生成Excel格式的XML头部

            int CurrentCol = 0; //当前列
            int RowCount = dt.Rows.Count + 1; //总行数
            int ColCount = dt.Columns.Count; //总列数

            //if (File.Exists(sFileName))
            //{
            //    File.Delete(sFileName);
            //}
            //实例化一个保存文件对话框
            StreamWriter sw = new StreamWriter(sFileName, false); //文件如果存在，则自动覆盖
            try
            {
                #region XML头部

                sw.WriteLine("<?xml version=\"1.0\"?>");
                sw.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
                sw.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
                sw.WriteLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
                sw.WriteLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
                sw.WriteLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
                sw.WriteLine(" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");
                sw.WriteLine(" <DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">");
                sw.WriteLine(" <Author>Automated Report Generator Example</Author>");
                sw.WriteLine(string.Format(" <Created>{0}T{1}Z</Created>", DateTime.Now.ToString("yyyy-mm-dd"),
                                           DateTime.Now.ToString("HH:MM:SS")));
                sw.WriteLine(" <Company>Your Company Here</Company>");
                sw.WriteLine(" <Version>11.6408</Version>");
                sw.WriteLine(" </DocumentProperties>");
                sw.WriteLine(" <ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\">");
                sw.WriteLine(" <WindowHeight>8955</WindowHeight>");
                sw.WriteLine(" <WindowWidth>11355</WindowWidth>");
                sw.WriteLine(" <WindowTopX>480</WindowTopX>");
                sw.WriteLine(" <WindowTopY>15</WindowTopY>");
                sw.WriteLine(" <ProtectStructure>False</ProtectStructure>");
                sw.WriteLine(" <ProtectWindows>False</ProtectWindows>");
                sw.WriteLine(" </ExcelWorkbook>");
                sw.WriteLine(" <Styles>");
                sw.WriteLine(" <Style ss:ID=\"Default\" ss:Name=\"Normal\">");
                sw.WriteLine("   <Alignment ss:Vertical=\"Bottom\"/>");
                sw.WriteLine("   <Borders/>");
                sw.WriteLine("   <Font/>");
                sw.WriteLine("   <Interior/>");
                sw.WriteLine("   <Protection/>");
                sw.WriteLine(" </Style>");
                sw.WriteLine(" <Style ss:ID=\"s21\">");
                sw.WriteLine("   <Alignment ss:Vertical=\"Bottom\" ss:WrapText=\"1\"/>");
                sw.WriteLine(" </Style>");
                sw.WriteLine(" </Styles>");
                sw.WriteLine(" <Worksheet ss:Name=\"Sheet1\">");
                sw.WriteLine(
                    string.Format(
                        " <Table ss:ExpandedColumnCount=\"{0}\" ss:ExpandedRowCount=\"{1}\" x:FullColumns=\"1\"",
                        ColCount.ToString(), RowCount.ToString()));
                sw.WriteLine("   x:FullRows=\"1\">");

                #endregion XML头部

                #region 写入表头信息

                sw.WriteLine("<Row ss:AutoFitHeight='0' ss:Height='18'>");
                for (CurrentCol = 0; CurrentCol < ColCount; CurrentCol++)
                {
                    sw.Write("<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">");
                    sw.Write(dt.Columns[CurrentCol].ColumnName.ToString().Trim());
                    sw.WriteLine("</Data></Cell>");
                }
                sw.WriteLine("</Row>");

                #endregion 写入表头信息

                #region 写入数据

                foreach (DataRow row in dt.Rows)
                {
                    sw.WriteLine("<Row ss:AutoFitHeight='0'>");

                    for (CurrentCol = 0; CurrentCol < ColCount; CurrentCol++)
                    {
                        sw.Write("<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">");
                        if (row[CurrentCol] != null)
                        {
                            //内容含有html标签会打开失败
                            if (row[CurrentCol].ToString().Trim().Contains("<") && row[CurrentCol].ToString().Trim().Contains(">"))
                                sw.Write(RemoveHTML(row[CurrentCol].ToString().Trim())); //去掉内容里的html标签
                            else
                                sw.Write(row[CurrentCol].ToString().Trim());
                        }
                        else
                        {
                            sw.Write("");
                        }
                        sw.WriteLine("</Data></Cell>");
                    }

                    sw.WriteLine("</Row>");
                }

                #endregion 写入数据

                #region XML尾部

                sw.WriteLine(" </Table>");
                sw.WriteLine(" <WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
                sw.WriteLine("   <Selected/>");
                sw.WriteLine("   <Panes>");
                sw.WriteLine("    <Pane>");
                sw.WriteLine("     <Number>3</Number>");
                sw.WriteLine("     <ActiveRow>1</ActiveRow>");
                sw.WriteLine("    </Pane>");
                sw.WriteLine("   </Panes>");
                sw.WriteLine("   <ProtectObjects>False</ProtectObjects>");
                sw.WriteLine("   <ProtectScenarios>False</ProtectScenarios>");
                sw.WriteLine(" </WorksheetOptions>");
                sw.WriteLine(" </Worksheet>");
                sw.WriteLine(" <Worksheet ss:Name=\"Sheet2\">");
                sw.WriteLine(" <WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
                sw.WriteLine("   <ProtectObjects>False</ProtectObjects>");
                sw.WriteLine("   <ProtectScenarios>False</ProtectScenarios>");
                sw.WriteLine(" </WorksheetOptions>");
                sw.WriteLine(" </Worksheet>");
                sw.WriteLine(" <Worksheet ss:Name=\"Sheet3\">");
                sw.WriteLine(" <WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
                sw.WriteLine("   <ProtectObjects>False</ProtectObjects>");
                sw.WriteLine("   <ProtectScenarios>False</ProtectScenarios>");
                sw.WriteLine(" </WorksheetOptions>");
                sw.WriteLine(" </Worksheet>");
                sw.WriteLine("</Workbook>");

                #endregion XML尾部

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                sw.Close();
                sw = null;
            }
        }

        /// <summary>
        /// 去掉内容里的html标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveHTML(string html)
        {
            html = html.Replace("(<style)+[^<>]*>[^\0]*(</style>)+", "");
            html = html.Replace(@"\<img[^\>] \>", "");
            html = html.Replace(@"<p>", "");
            html = html.Replace(@"</p>", "");


            System.Text.RegularExpressions.Regex regex0 =
            new System.Text.RegularExpressions.Regex("(<style)+[^<>]*>[^\0]*(</style>)+", System.Text.RegularExpressions.RegexOptions.Multiline);
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S] </script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S] </iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S] </frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>] \>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记  
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性   
            html = regex0.Replace(html, ""); //过滤href=javascript: (<A>) 属性   


            //html = regex10.Replace(html, "");  
            html = regex3.Replace(html, "");// _disibledevent="); //过滤其它控件的on...事件  
            html = regex4.Replace(html, ""); //过滤iframe  
            html = regex5.Replace(html, ""); //过滤frameset  
            html = regex6.Replace(html, ""); //过滤frameset  
            html = regex7.Replace(html, ""); //过滤frameset  
            html = regex8.Replace(html, ""); //过滤frameset  
            html = regex9.Replace(html, "");
            //html = html.Replace(" ", "");  
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            html = html.Replace(" ", "");
            return html;
        }

        /********************利用Aspose.Cells导入导出**************************/
        /// <summary>
        /// [Aspose.Cells]导入Excel数据到DataTable
        /// </summary>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string strFileName, ref string returnInfo)
        {
            try
            {
                Workbook book = new Workbook();
                book.Open(strFileName);
                Worksheet sheet = book.Worksheets[0];
                Cells cells = sheet.Cells;

                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
            }
            catch (Exception ex)
            {
                returnInfo = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Aspose.Cells]导入Excel数据到DataTable
        /// </summary>
        /// <param name="file">文件</param>
        public static DataTable ExcelToDataTable(HttpPostedFileBase file, ref string returnInfo)
        {
            try
            {
                if (file != null)
                {
                    bool exportColumnName = true;
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(file.InputStream);
                    //workbook.Open(file.FileName);
                    Aspose.Cells.Worksheet worksheet = workbook.Worksheets[0];
                    Cells cells = worksheet.Cells;

                    return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, exportColumnName);//这里用到Aspose.Cells的ExportDataTableAsString方法来读取excel数据
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                returnInfo = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Aspose.Cells]导入CSV数据到DataTable
        /// </summary>
        /// <param name="file">文件</param>
        public static DataTable CSVToDataTable(HttpPostedFile file, ref string returnInfo)
        {
            try
            {
                if (file != null)
                {
                    TxtLoadOptions lo = new TxtLoadOptions();
                    lo.ConvertNumericData = true;
                    //lo.LoadDataOnly = true;
                    lo.Encoding = Encoding.Default;

                    Workbook awork = new Workbook(file.InputStream, lo);
                    awork.FileFormat = FileFormatType.CSV;
                    Worksheet worksheet = awork.Worksheets[0];

                    Cells cells = worksheet.Cells;
                    bool exportColumnName = true;
                    return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, exportColumnName);//这里用到Aspose.Cells的ExportDataTableAsString方法来读取excel数据
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                returnInfo = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// [Aspose.Cells]导出到Excel
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="path">生成的Excel文件路径</param>
        /// <param name="response"></param>
        public static bool ExportDataToExcel(DataTable dt, string path, ref string returnInfo)
        {
            try
            {
                if (dt == null)
                {
                    returnInfo += "数据为空";
                    return false;
                }

                Workbook workbook = new Workbook();
                Worksheet sheet = (Worksheet)workbook.Worksheets[0];

                int curCol = 0; //当前列
                int rowCount = dt.Rows.Count + 1; //总行数
                int colCount = dt.Columns.Count; //总列数

                for (curCol = 0; curCol < colCount; curCol++) //列头
                {
                    sheet.Cells[0, curCol].PutValue(dt.Columns[curCol].ColumnName.ToString().Trim());
                }
                int i = 1;
                foreach (DataRow row in dt.Rows) //数据
                {
                    for (curCol = 0; curCol < colCount; curCol++)
                    {
                        if (row[curCol] != null)
                        {
                            //内容含有html标签会打开失败
                            if (row[curCol].ToString().Trim().Contains("<") && row[curCol].ToString().Trim().Contains(">"))
                                sheet.Cells[i, curCol].PutValue(RemoveHTML(row[curCol].ToString().Trim())); //去掉内容里的html标签
                            else
                                sheet.Cells[i, curCol].PutValue(row[curCol].ToString().Trim());
                        }
                    }
                    i++;
                }

                //先删除当前目录下的其它文件
                // 清空指定的文件夹，但不删除文件夹
                string directoryPath = path.Substring(0, path.LastIndexOf('\\'));
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                else
                    DeleteFolder(directoryPath);

                File.WriteAllBytes(path, workbook.SaveToStream().ToArray());

                return true;
            }
            catch (Exception ex)
            {
                returnInfo += ex.Message;
                return false;
            }
        }

        /// <summary>
        /// [Aspose.Cells]导出到Excel
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="fileName">生成的Excel文件名</param>
        /// <param name="response"></param>
        public static void ExportDataToExcel(DataTable dt, string fileName, HttpResponseBase response)
        {
            if (dt == null) return;

            Workbook workbook = new Workbook();
            Worksheet sheet = (Worksheet)workbook.Worksheets[0];

            int curCol = 0; //当前列
            int rowCount = dt.Rows.Count + 1; //总行数
            int colCount = dt.Columns.Count; //总列数

            for (curCol = 0; curCol < colCount; curCol++) //列头
            {
                sheet.Cells[0, curCol].PutValue(dt.Columns[curCol].ColumnName.ToString().Trim());
            }
            int i = 1;
            foreach (DataRow row in dt.Rows) //数据
            {
                for (curCol = 0; curCol < colCount; curCol++)
                {
                    if (row[curCol] != null)
                    {
                        //内容含有html标签会打开失败
                        if (row[curCol].ToString().Trim().Contains("<") && row[curCol].ToString().Trim().Contains(">"))
                            sheet.Cells[i, curCol].PutValue(RemoveHTML(row[curCol].ToString().Trim())); //去掉内容里的html标签
                        else
                            sheet.Cells[i, curCol].PutValue(row[curCol].ToString().Trim());
                    }
                }
                i++;
            }

            response.Clear();
            response.Buffer = true;
            response.Charset = "utf-8"; //设置字符集类型                         
            response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            response.ContentEncoding = System.Text.Encoding.UTF8;
            //response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            response.ContentType = "application/ms-excel";
            response.BinaryWrite(workbook.SaveToStream().ToArray());
            response.End();
        }

        /// <summary>
        /// [Aspose.Cells]导出到Excel数据流
        /// </summary>
        /// <param name="dt">数据源</param>
        public static byte[] ExportDataToExcel(DataTable dt)
        {
            if (dt == null) return null;

            Workbook workbook = new Workbook();
            Worksheet sheet = (Worksheet)workbook.Worksheets[0];

            int curCol = 0; //当前列
            int rowCount = dt.Rows.Count + 1; //总行数
            int colCount = dt.Columns.Count; //总列数

            for (curCol = 0; curCol < colCount; curCol++) //列头
            {
                sheet.Cells[0, curCol].PutValue(dt.Columns[curCol].ColumnName.ToString().Trim());
            }
            int i = 1;
            foreach (DataRow row in dt.Rows) //数据
            {
                for (curCol = 0; curCol < colCount; curCol++)
                {
                    if (row[curCol] != null)
                    {
                        //内容含有html标签会打开失败
                        if (row[curCol].ToString().Trim().Contains("<") && row[curCol].ToString().Trim().Contains(">"))
                            sheet.Cells[i, curCol].PutValue(RemoveHTML(row[curCol].ToString().Trim())); //去掉内容里的html标签
                        else
                            sheet.Cells[i, curCol].PutValue(row[curCol].ToString().Trim());
                    }
                }
                i++;
            }

            return workbook.SaveToStream().ToArray();
        }

        /// <summary>
        /// [Aspose.Cells]导出到Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="response"></param>
        public static void ExportDataToExcel<T>(IEnumerable<T> data, HttpResponseBase response)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = (Worksheet)workbook.Worksheets[0];

            PropertyInfo[] ps = typeof(T).GetProperties();
            int index = 0;

            foreach (var p in ps)
            {
                string colIndex = IndexCellName(index++);
                var displayName = p.GetCustomAttribute<DisplayNameAttribute>();

                if (displayName != null && !string.IsNullOrEmpty(displayName.DisplayName))
                    sheet.Cells[colIndex + 1].PutValue(displayName.DisplayName);
                else
                    sheet.Cells[colIndex + 1].PutValue(p.Name);

                int i = 2;
                foreach (var d in data)
                {
                    sheet.Cells[colIndex + i].PutValue(p.GetValue(d, null));
                    i++;
                }

                colIndex = ((char)(colIndex[0] + 1)).ToString();
            }

            response.Clear();
            response.Buffer = true;
            response.Charset = "utf-8";
            response.AppendHeader("Content-Disposition", "attachment;filename=xxx.xls");
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "application/ms-excel";
            response.BinaryWrite(workbook.SaveToStream().ToArray());
            response.End();
        }

        /// <summary>
        /// [Aspose.Cells]导出到Excel数据流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="ignore">忽略的属性</param>
        public static byte[] ExportDataToExcel<T>(IEnumerable<T> data, params string[] ignore)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = (Worksheet)workbook.Worksheets[0];

            PropertyInfo[] ps = typeof(T).GetProperties();
            int index = 0;

            foreach (var p in ps)
            {
                if (ignore != null && ignore.Contains(p.Name))
                    continue;

                string colIndex = IndexCellName(index++);

                var displayName = p.GetCustomAttribute<DisplayNameAttribute>();

                if (displayName != null && !string.IsNullOrEmpty(displayName.DisplayName))
                    sheet.Cells[colIndex + 1].PutValue(displayName.DisplayName);
                else
                    sheet.Cells[colIndex + 1].PutValue(p.Name);

                int i = 2;
                foreach (var d in data)
                {
                    object value = p.GetValue(d, null);
                    if (value != null && value.ToString() != "")
                    {
                        var types = p.PropertyType.GenericTypeArguments;
                        string type = types.Length > 0 ? types[0].Name : p.PropertyType.Name;
                        if (type == "DateTime")
                            value = Convert.ToDateTime(value).ToString();
                    }
                    sheet.Cells[colIndex + i].PutValue(value);
                    i++;
                }
            }

            return workbook.SaveToStream().ToArray();
        }

        /// <summary>
        /// [Aspose.Cells]把dataTable数据通过某列分组，再把分组数据存到LIst<Datatable>中
        /// </summary>
        /// <param name="source">数据中每行数据</param>
        /// <param name="destination">分组后数据存入的Datatable</param>
        /// <param name="groupByFields">分组字段数组</param>
        /// <param name="fieldIndex"></param>
        /// <param name="schema">数据集</param>
        public static void GroupDataRows(IEnumerable<DataRow> source, List<DataTable> destination, string[] groupByFields, int fieldIndex, DataTable schema)
        {
            if (fieldIndex >= groupByFields.Length || fieldIndex < 0)
            {
                DataTable dt = schema.Clone();
                foreach (DataRow row in source)
                {
                    DataRow dr = dt.NewRow();
                    dr.ItemArray = row.ItemArray;
                    dt.Rows.Add(dr);
                }

                destination.Add(dt);
                return;
            }

            var results = source.GroupBy(o => o[groupByFields[fieldIndex]]);
            foreach (var rows in results)
            {
                GroupDataRows(rows, destination, groupByFields, fieldIndex + 1, schema);
            }

            fieldIndex++;
        }
        /********************利用Aspose.Cells导入导出**************************/

        /// <summary>
        /// 清空指定的文件夹，但不删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        DeleteFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }

        private static string IndexCellName(int index)
        {
            char cell = 'A';
            string indexCell = "";
            int count = (int)Math.Ceiling(Math.Log(index + (index == 0 ? 1.5 : 1), 26));
            for (int i = 0; i < count; i++)
            {
                if (i == 0)
                    indexCell = (char)(cell + index % 26) + indexCell;
                else
                    indexCell = (char)(cell + index / 26) + indexCell;

                index -= 25;
            }

            return indexCell;
        }
    }
}
