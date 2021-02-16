using ClosedXML.Excel;
using System;
using System.Drawing;
using System.IO;


namespace Akgul_Yemek.Code
{
    class MExcell
    {
        //Excel.Application ExcelUygulama;
        //Excel.Workbook ExcelProje;
        //Excel.Worksheet ExcelSayfa;
        //object Missing = System.Reflection.Missing.Value;
        //Excel.Range ExcelRange;




        XLWorkbook workbook = new XLWorkbook();
        IXLWorksheet ExcelSayfa = null;

        public int column_i { get; set; }
        public int column_j { get; set; }

        public MExcell()
        {
            //ExcelUygulama = new Excel.Application();
            //ExcelProje = ExcelUygulama.Workbooks.Add(Missing);
            //ExcelSayfa = (Excel.Worksheet)ExcelProje.Worksheets.get_Item(1);
            //ExcelSayfa = (Excel.Worksheet)ExcelUygulama.ActiveSheet;

           
            ExcelSayfa = workbook.Worksheets.Add("Sample Sheet");
           

            //ExcelUygulama.Visible = false;
            //ExcelUygulama.AlertBeforeOverwriting = false;

            //ExcelSayfa.PageSetup.TopMargin = 1;
            //ExcelSayfa.PageSetup.BottomMargin = 1;
            //ExcelSayfa.PageSetup.LeftMargin = 1;
            //ExcelSayfa.PageSetup.RightMargin = 1;
            
            //ExcelSayfa.PageSetup.HeaderMargin = 1;
            //ExcelSayfa.PageSetup.FooterMargin = 1;


            column_i = 1;
            column_j = 1;
        }


        public int columnSizeForBorder { get; set; }
        public int startRowIndex { get; set; }
        public int stopRowIndex { get; set; }

        public void SetStartPoint()
        {
            startRowIndex = column_i;
        }

        public void SetStopPointAndDrawBorder(int columnCount)
        {
            stopRowIndex = column_i;

            MakeBordered(startRowIndex, 1, stopRowIndex, columnCount);
        }

        private bool MakeBordered(int i, int j, int ii, int jj)
        {
            try
            {
                IXLRange range = ExcelSayfa.Range(ExcelSayfa.Cell(i, j), ExcelSayfa.Cell(ii, jj));
                //range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium);
                range.Style.Border.InsideBorder = XLBorderStyleValues.Medium;
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                //range.Style.Border.
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddColumn(int j=-1, string name="", int width=-1)
        {
            try
            {
                if (j==-1){
                    ExcelSayfa.Cell(column_i, column_j).Value = name;}
                else{
                    ExcelSayfa.Cell(column_i, j).Value = name;
                    column_j = j;
                }

                if(width!=-1)
                    ExcelSayfa.Cell(column_i, column_j).WorksheetColumn().Width = width;

                ExcelSayfa.Cell(column_i, column_j).Style.Fill.BackgroundColor = XLColor.LightBlue;
                ExcelSayfa.Cell(column_i, column_j).Style.Font.Bold = true;
                
                column_j++;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool MergeTwoCells(int first_j, int second_j)
        {
            try
            {
                ExcelSayfa.Range(ExcelSayfa.Cell(column_i, first_j), ExcelSayfa.Cell(column_i, second_j)).Merge();
                //column_j = second_j;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WriteSpecificAdress(int j, string data, bool isNumber=false)
        {
            try
            {
                ExcelSayfa.Cell(column_i, j).Value = data;
                if (isNumber)
                {
                    ExcelSayfa.Cell(column_i, j).DataType = XLDataType.Number;
                }

                // column_i = i;
                column_j = j;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WriteFormulaToSpecificAdress(int j, string formula)
        {
            try
            {
                ExcelSayfa.Cell(column_i, j).FormulaR1C1 = formula;

                // column_i = i;
                column_j = j;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Double EvaluateFormula(string formula)
        {
            var sum = ExcelSayfa.Evaluate(formula);
            return Convert.ToDouble(sum);
        }

        public void WriteBlankLine()
        {
            column_i+=2;
            column_j = 1;
        }


        public void PassNextLine()
        {
            column_i++;
            column_j=1;
        }



        public bool WriteToFile(string path, string fileName)
        {
            try
            {
                string fileNameWithPath = Path.Combine(path, fileName);

                if (File.Exists(fileNameWithPath))
                    File.Delete(fileNameWithPath);

                // kolonların içeriklere göre yeniden boyutlandırılması:
                ExcelSayfa.Columns().AdjustToContents();

                workbook.SaveAs(fileNameWithPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
