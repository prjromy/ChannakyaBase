using ChannakyaBase.Model.ViewModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace ExportExcel.Code
{
    public class ExcelExportHelper
    {
        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        public static DataTable ListToDataTable<T>(IEnumerable<T> data, params string[] columnsToTake)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(columnsToTake[i], Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static byte[] ExportExcel(DataTable dataTable, CompanyViewModel heading, string[] parameterSearch)
        {
            
            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("Sheet1"));
                int startRowFrom = 1;
                if (parameterSearch.Count() == 1)
                {
                    startRowFrom = 7;
                }
                else if(parameterSearch.Count() == 2)
                {
                    startRowFrom = 8;
                }
                else
                {
                    startRowFrom = 9;
                }
                //  workSheet.Cells["A1:G1"].Merge = true;



                DataColumn dataColumn = dataTable.Columns.Add("S.N", typeof(int));
                dataColumn.SetOrdinal(0);
                int index = 1;
                foreach (DataRow item in dataTable.Rows)
                {
                    item[0] = index;
                    index++;
                }



                // add the content into the Excel file
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    if (column.ToString() == "Date")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }
                    if (column.ToString() == "Transaction Date")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }
                    if (column.ToString()== "MDate")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }
                    if (column.ToString() == "Amount")
                    {
                        columnCells.Style.Numberformat.Format = "#,##0.00";
                    }
                    if (column.ToString() == "SchDate")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }
                    if (column.ToString() == "VDate")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }
                    if (column.ToString() == "Register Date")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }
                    if (column.ToString() == "TDate")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }

                    if (column.ToString() == "ChangeOn")
                    {
                        columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                    }
                    //int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());

                    //if (maxLength < 150)
                    //{
                    workSheet.Column(columnIndex).AutoFit();
                    //}


                    columnIndex++;
                }

                // format header - bold, yellow on black

                //// format cells - add borders
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns
                //for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                //{
                //    if (i == 0 && showSrNo)
                //    {
                //        continue;
                //    }
                //    if (dataTable.Columns[i].ColumnName=="Date")
                //    {
                //        workSheet.Cells[2, dc, rowCount + 2, dc].Style.Numberformat.Format = "mm/dd/yyyy hh:mm:ss AM/PM";
                //    }

                //}




                workSheet.Cells["A1"].Value = heading.BranchName;

                workSheet.Cells["A2"].Value = heading.Address + "," + heading.Region + "," + heading.State;
                workSheet.Cells["A3"].Value = "Phone No.:" + heading.PhoneNo + "," + "Fax No .:" + heading.FaxNo;
                workSheet.Cells["A4"].Value = "Email.:" + heading.Email;
                // workSheet.Cells["A1"].Style.Font.Size = 20;
                if (parameterSearch.Length == 1)
                {
                    workSheet.Cells["A6"].Value = parameterSearch[0];
                   
                }
                else if(parameterSearch.Length == 2)
                {
                    workSheet.Cells["A6"].Value = parameterSearch[0];
                    workSheet.Cells["A7"].Value = parameterSearch[1];
                }
                else if (parameterSearch.Length == 3)
                {
                    workSheet.Cells["A6"].Value = parameterSearch[0];
                    workSheet.Cells["A7"].Value = parameterSearch[1];
                    workSheet.Cells["A8"].Value = parameterSearch[2];
                }

                workSheet.InsertColumn(1, 1);
                workSheet.InsertRow(1, 1);




                result = package.GetAsByteArray();
            }

            return result;
        }

        public static byte[] ExportExcel<T>(List<T> data, CompanyViewModel Heading, string[] parameterSearch, params string[] ColumnsToTake)
        {
            return ExportExcel(ListToDataTable<T>(data, ColumnsToTake), Heading, parameterSearch);
        }

        //public static byte[] ExportExcel2Columns<T>(List<T> data, List<T> data2, CompanyViewModel Heading, string[] parameterSearch, params string[] ColumnsToTake)
        //{
        //    return ExportExcel(ListToDataTable<T>(data, ColumnsToTake), Heading, parameterSearch);
        //}

        //public static byte[] ExportExcel2Columns(DataTable dataTable, CompanyViewModel heading, string[] parameterSearch)
        //{

        //    byte[] result = null;
        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("Sheet1"));
        //        int startRowFrom = 1;
        //        if (parameterSearch.Count() == 1)
        //        {
        //            startRowFrom = 7;
        //        }
        //        else if (parameterSearch.Count() == 2)
        //        {
        //            startRowFrom = 8;
        //        }
        //        else
        //        {
        //            startRowFrom = 9;
        //        }
        //        //  workSheet.Cells["A1:G1"].Merge = true;



        //        DataColumn dataColumn = dataTable.Columns.Add("S.No.", typeof(int));
        //        dataColumn.SetOrdinal(0);
        //        int index = 1;
        //        foreach (DataRow item in dataTable.Rows)
        //        {
        //            item[0] = index;
        //            index++;
        //        }



        //        // add the content into the Excel file
        //        workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

        //        // autofit width of cells with small content
        //        int columnIndex = 1;
        //        foreach (DataColumn column in dataTable.Columns)
        //        {
        //            ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
        //            if (column.ToString() == "Date")
        //            {
        //                columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
        //            }
        //            if (column.ToString() == "MDate")
        //            {
        //                columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
        //            }
        //            if (column.ToString() == "Amount")
        //            {
        //                columnCells.Style.Numberformat.Format = "#,##0.00";
        //            }
        //            //int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());

        //            //if (maxLength < 150)
        //            //{
        //            workSheet.Column(columnIndex).AutoFit();
        //            //}


        //            columnIndex++;
        //        }

        //        // format header - bold, yellow on black

        //        //// format cells - add borders
        //        using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
        //        {
        //            r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

        //            r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
        //            r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
        //            r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
        //            r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
        //        }

        //        // removed ignored columns
        //        //for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
        //        //{
        //        //    if (i == 0 && showSrNo)
        //        //    {
        //        //        continue;
        //        //    }
        //        //    if (dataTable.Columns[i].ColumnName=="Date")
        //        //    {
        //        //        workSheet.Cells[2, dc, rowCount + 2, dc].Style.Numberformat.Format = "mm/dd/yyyy hh:mm:ss AM/PM";
        //        //    }

        //        //}




        //        workSheet.Cells["A1"].Value = heading.BranchName;

        //        workSheet.Cells["A2"].Value = heading.Address + "," + heading.Region + "," + heading.State;
        //        workSheet.Cells["A3"].Value = "Phone No.:" + heading.PhoneNo + "," + "Fax No .:" + heading.FaxNo;
        //        workSheet.Cells["A4"].Value = "Email.:" + heading.Email;
        //        // workSheet.Cells["A1"].Style.Font.Size = 20;
        //        if (parameterSearch.Length == 1)
        //        {
        //            workSheet.Cells["A6"].Value = parameterSearch[0];

        //        }
        //        else if (parameterSearch.Length == 2)
        //        {
        //            workSheet.Cells["A6"].Value = parameterSearch[0];
        //            workSheet.Cells["A7"].Value = parameterSearch[1];
        //        }
        //        else if (parameterSearch.Length == 3)
        //        {
        //            workSheet.Cells["A6"].Value = parameterSearch[0];
        //            workSheet.Cells["A7"].Value = parameterSearch[1];
        //            workSheet.Cells["A8"].Value = parameterSearch[2];
        //        }

        //        workSheet.InsertColumn(1, 1);
        //        workSheet.InsertRow(1, 1);




        //        result = package.GetAsByteArray();
        //    }

        //    return result;
        //}
    }
}