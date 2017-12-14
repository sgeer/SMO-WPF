using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using Color = System.Windows.Media.Color;
using Cursors = System.Windows.Forms.Cursors;
using MessageBox = System.Windows.MessageBox;
using Excel = Microsoft.Office.Interop.Excel;

namespace SMO_PROG_WPF
{
    public partial class MainWindow : Window
    {
        private ShowDate showDate;
        private BusinessLogic businessLogic;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_ShowDate_Click(object sender, RoutedEventArgs e)
        {
            showDate = new ShowDate();
            showDate.Show();
        }

        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            businessLogic = new BusinessLogic();
            List<double> quantity = new List<double>();
            ShowDate.DataFile fileData = GetShowDataFile();
            Settings.SettingsFile fileSettings = GetSettingsFile();

            for (int i = 0; i < fileData.Data.Count; i++)
                quantity.Add(Convert.ToDouble(fileData.Data[i].Clients));

            dataGrid.ItemsSource = businessLogic.GetTabulatedLines(quantity, Convert.ToDouble(fileData.Data.Last().Time), fileSettings.t, fileSettings.QueueLength, fileSettings.PeriodData, fileSettings.PredictSteps,
                                                                    fileSettings.MaxCashbox, fileSettings.ServiceTime, fileSettings.RadioButton_InfiniteQueue,
                                                                    fileSettings.RadioButton_LimitedQueue, fileSettings.RadioButton_LimitedTime);
            string[] names = new[] { "№", "Время", "Посетители", "Кассы" };
            for (int column = 0; column < names.Length; column++)
            {
                dataGrid.Columns[column].Header = names[column];
            }
            //MessageBox.Show(queuingSystem.MultiChannelInfiniteQueue(1.5, 2.5, 10).ToString());

            //MessageBox.Show(queuingSystem.MultiChannelLimitedQueue((double)1/5,(double)4/9, 4, 6).ToString());
        }

        private ShowDate.DataFile GetShowDataFile ()
        {
            ShowDate.DataFile file = new ShowDate.DataFile();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("showData.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length == 0) return null;
                file = (ShowDate.DataFile)formatter.Deserialize(fs);
            }
            return file;
        }

        private Settings.SettingsFile GetSettingsFile()
        {
            Settings.SettingsFile file = new Settings.SettingsFile();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("settings.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length == 0) return null;
                file = (Settings.SettingsFile)formatter.Deserialize(fs);
            }
            return file;
        }

        private void dataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataGrid.Columns.Count != 0)
            {
                Line line = dataGrid.SelectedItem as Line;
                //СharacteristicsQueuingSystemWindow chWindow = new СharacteristicsQueuingSystemWindow(chQueuingSystems[line.Id-1]);
                 businessLogic.СharacteristicsQS(line.Id - 1);
                СharacteristicsQueuingSystemWindow chWindow = new СharacteristicsQueuingSystemWindow(businessLogic.СharacteristicsQS(line.Id - 1));
                chWindow.Show();
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Items.Count == 0)
            {
                MessageBox.Show("Нет данных для сохранения");
                return;
            }
            //CloseExcel();
            //Microsoft.Win32.OpenFileDialog saveFileDialog = new Microsoft.Win32.OpenFileDialog();
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            bool isSave = false;
            //saveFileDialog.InitialDirectory = "C:";
            saveFileDialog.Title = "Сохранить в файл";
            saveFileDialog.FileName = "";
            saveFileDialog.Filter = "Файлы xlsx|*.xlsx";
            string path = null;

            if (saveFileDialog.ShowDialog() == true)
            {
                path = saveFileDialog.FileName;
                isSave = true;
            }
            else
                MessageBox.Show("Файл не сохранен");

            if (isSave)
            {
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
                  //Если файл не существует, то мы создадим его
                if (!File.Exists(path))
                {
                    Excel.Application WorkExcel = new Excel.Application();
                    Excel.Workbook WorkBook = WorkExcel.Workbooks.Add(); // создаем книгу

                    WorkBook.SaveAs(path); //сохранить Excel файл

                    WorkBook.Close();
                    WorkExcel.Quit();
                }
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;

                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();


                var workbooks = xlApp.Workbooks;

                xlWorkBook = workbooks.Open(path, 0, false, 5, "", "", true,
                Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                var worksheets = xlWorkBook.Worksheets;

                xlWorkSheet = (Excel.Worksheet) worksheets.get_Item(1);
                xlWorkSheet.Cells.Clear();

                for (int j = 0; j < dataGrid.Items.Count; j++)
                {
                    for (int i = 0; i < dataGrid.Columns.Count; i++)
                    {
                        Line line = (Line)dataGrid.Items[j];
                        if (i == 0)
                            xlWorkSheet.Cells[j + 1, i + 1].Value = line.Time;
                        else if (i == 1)
                            xlWorkSheet.Cells[j + 1, i + 1].Value = line.Clients;
                        else if (i == 2)
                            xlWorkSheet.Cells[j + 1, i + 1].Value = line.Сashbox;
                    }
                }

                xlWorkBook.Save();

                //Закрываем
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                MessageBox.Show("Сохранено!");
            }
        }
    }
}
