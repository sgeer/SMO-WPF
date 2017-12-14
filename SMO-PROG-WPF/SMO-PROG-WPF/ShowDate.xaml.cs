using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Forms;
using Cursors = System.Windows.Input.Cursors;
using Excel = Microsoft.Office.Interop.Excel;
using MessageBox = System.Windows.MessageBox;

namespace SMO_PROG_WPF
{
    /// <summary>
    /// Логика взаимодействия для ShowDate.xaml
    /// </summary>
    public partial class ShowDate : Window
    {
        public List<string> Week;
        public List<double> Time;
        public List<double> Quantity;
        private Excel.Application ExcelApp;
        private Excel.Workbook WorkBookExcel;
        private Excel.Worksheet WorkSheetExcel;
        private Excel.Range RangeExcel;

        public ShowDate()
        {
            InitializeComponent();
            Week = new List<string>();
            Time = new List<double>();
            Quantity = new List<double>();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataFile file = new DataFile();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("showData.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length == 0) return;
                file = (DataFile)formatter.Deserialize(fs);
            }
            Grid.ItemsSource = file.Data;
            string[] names = new[] { "№", "День недели", "Время", "Посетители" }; ;
            for (int column = 0; column < names.Length; column++)
            {
                Grid.Columns[column].Header = names[column];
            }
        }

       

        private void Button_Load_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.Filter = "Файл Excel|*.XLSX;*.XLS";
            var result = openDialog.ShowDialog();
            if (result == false)
            {
                MessageBox.Show("Файл не выбран!", "Информация", MessageBoxButton.YesNo, MessageBoxImage.Information);
                return;
            }
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            string fileName = System.IO.Path.GetFileName(openDialog.FileName);

            ExcelApp = new Excel.Application();
            //Книга.
            WorkBookExcel = ExcelApp.Workbooks.Open(openDialog.FileName);
            //Таблица.
            //WorkSheetExcel = ExcelApp.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
            //  RangeExcel = null;
            WorkSheetExcel = (Excel.Worksheet)WorkBookExcel.Sheets[1];

            var lastCell = WorkSheetExcel.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);

            List <Line> res = new List<Line>();
            Line line;
            int id = 0;
            for (int j = 0; j < (int) lastCell.Row; j++)
            {
                line = new Line();
                for (int i = 0; i < (int) lastCell.Column; i++)
                {
                    // if (WorkSheetExcel.Cells[j + 1, i + 1].Text == "")
                    if (WorkSheetExcel.Cells[j + 1, i + 1].Value == null)
                        break;

                    if (i == 0)
                        line.Week = WorkSheetExcel.Cells[j + 1, i + 1].Text.ToString();
                    else if (i == 1)
                        line.Time = WorkSheetExcel.Cells[j + 1, i + 1].Text.ToString();
                    else if (i == 2)
                        line.Clients = WorkSheetExcel.Cells[j + 1, i + 1].Text.ToString();


                }
                if (WorkSheetExcel.Cells[j + 1, 1].Value == null)
                    break;
                line.Id = ++id;
                res.Add(line);
            }

            WorkBookExcel.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ExcelApp.Quit();         // вышел из Excel
            GC.Collect();  // 

           Grid.ItemsSource = res;
           var set = new DataFile();
           set.Data = res;

           BinaryFormatter formatter = new BinaryFormatter();
           File.Delete("showData.dat");

           using (FileStream fs = new FileStream("showData.dat", FileMode.Create))
               formatter.Serialize(fs, set);

            string[] names = new[] { "№", "День недели", "Время", "Посетители" }; ;
            for (int column = 0; column < names.Length; column++)
            {
                Grid.Columns[column].Header = names[column];
            }
        }


        [Serializable]
        public class Line
        {
            public int Id { get; set; }
            public string Week { get; set; }
            public string Time { get; set; }
            public string Clients { get; set; }

            public Line()
            {
            }
        }

        [Serializable]
        public class DataFile
        {
            public List<Line> Data;
        }

         private void Button_OK_Click(object sender, RoutedEventArgs e)
         {
             Close();
         }
    }
}
