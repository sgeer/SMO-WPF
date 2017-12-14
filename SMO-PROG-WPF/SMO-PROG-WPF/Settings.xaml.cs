using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace SMO_PROG_WPF
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        [Serializable]
        public class SettingsFile
        {
            public double t;            //время ожидания в очереди
            public int QueueLength;      //ограничение на длину очереди
            public int PeriodData;        // период данных
            public int PredictSteps;        // прогнозировать PredictSteps шагов
            public int MaxCashbox;      //максимальное количество касс
            public double ServiceTime;     // время обслуживания у одного канала
            public bool RadioButton_InfiniteQueue;
            public bool RadioButton_LimitedQueue;
            public bool RadioButton_LimitedTime;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsFile settings = new SettingsFile();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("settings.dat", FileMode.OpenOrCreate))
            {
                if (fs.Length == 0) return;
                settings = (SettingsFile)formatter.Deserialize(fs);
            }
            TextBox_WaitingTime.Text = settings.t.ToString();
            TextBox_QueueLength.Text = settings.QueueLength.ToString();
            TextBox_PeriodDate.Text = settings.PeriodData.ToString();
            TextBox_PredictSteps.Text = settings.PredictSteps.ToString();
            TextBox_MaxCashbox.Text = settings.MaxCashbox.ToString();
            TextBox_ServiceTime.Text = settings.ServiceTime.ToString();
            RadioButton_InfiniteQueue.IsChecked = settings.RadioButton_InfiniteQueue;
            RadioButton_LimitedQueue.IsChecked = settings.RadioButton_LimitedQueue;
            RadioButton_LimitedTime.IsChecked = settings.RadioButton_LimitedTime;
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            var set = new SettingsFile();
            set.t = Convert.ToDouble(TextBox_WaitingTime.Text);
            set.QueueLength = Convert.ToInt32(TextBox_QueueLength.Text);
            set.PeriodData = Convert.ToInt32(TextBox_PeriodDate.Text);
            set.PredictSteps = Convert.ToInt32(TextBox_PredictSteps.Text);
            set.ServiceTime = Convert.ToDouble(TextBox_ServiceTime.Text);
            set.MaxCashbox = Convert.ToInt32(TextBox_MaxCashbox.Text);
            set.RadioButton_LimitedQueue = RadioButton_LimitedQueue.IsChecked.Value;
            set.RadioButton_InfiniteQueue = RadioButton_InfiniteQueue.IsChecked.Value;
            set.RadioButton_LimitedTime = RadioButton_LimitedTime.IsChecked.Value;

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("settings.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, set);
            }
            Close();
        }

        private void TextBox_WaitingTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_WaitingTime.Text.Length == 0)
                return;
            try
            {
                double t = Convert.ToDouble(TextBox_WaitingTime.Text);
                if (t <= -1)
                    throw new Exception("Период меньше или равен нулю");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                TextBox_WaitingTime.Text = "0";
            }

            TextBox_WaitingTime.Text = Regex.Replace(TextBox_WaitingTime.Text, @"(\d+(,|\.)\d{2})\d+", @"$1");
            TextBox_WaitingTime.Select(TextBox_WaitingTime.Text.Length, 0);
        }

        private void TextBox_QueueLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_QueueLength.Text.Length == 0)
                return;
            try
            {
                double t = Convert.ToDouble(TextBox_QueueLength.Text);
                if (t <= -1)
                    throw new Exception("Число меньше или равен нулю");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                TextBox_QueueLength.Text = "0";
            }

            TextBox_QueueLength.Text = Regex.Replace(TextBox_QueueLength.Text, @"(\d+(,|\.)\d{2})\d+", @"$1");
            TextBox_QueueLength.Select(TextBox_QueueLength.Text.Length, 0);
        }

        private void TextBox_PeriodDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_PeriodDate.Text.Length == 0)
                return;
            try
            {
                double t = Convert.ToDouble(TextBox_PeriodDate.Text);
                if (t <= -1)
                    throw new Exception("Число меньше или равен нулю");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                TextBox_PeriodDate.Text = "0";
            }

            TextBox_PeriodDate.Text = Regex.Replace(TextBox_PeriodDate.Text, @"(\d+(,|\.)\d{2})\d+", @"$1");
            TextBox_PeriodDate.Select(TextBox_PeriodDate.Text.Length, 0);
        }

        private void TextBox_PredictSteps_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_PredictSteps.Text.Length == 0)
                return;
            try
            {
                double t = Convert.ToDouble(TextBox_PredictSteps.Text);
                if (t <= -1)
                    throw new Exception("Число меньше или равен нулю");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                TextBox_PredictSteps.Text = "0";
            }

            TextBox_PredictSteps.Text = Regex.Replace(TextBox_PredictSteps.Text, @"(\d+(,|\.)\d{2})\d+", @"$1");
            TextBox_PredictSteps.Select(TextBox_PredictSteps.Text.Length, 0);
        }

        private void TextBox_ServiceTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_ServiceTime.Text.Length == 0)
                return;
            try
            {
                double t = Convert.ToDouble(TextBox_ServiceTime.Text);
                if (t <= -1)
                    throw new Exception("Число меньше или равен нулю");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                TextBox_ServiceTime.Text = "0";
            }

            TextBox_ServiceTime.Text = Regex.Replace(TextBox_ServiceTime.Text, @"(\d+(,|\.)\d{2})\d+", @"$1");
            TextBox_ServiceTime.Select(TextBox_ServiceTime.Text.Length, 0);

        }

        private void TextBox_MaxCashbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox_MaxCashbox.Text.Length == 0)
                return;
            try
            {
                double t = Convert.ToDouble(TextBox_MaxCashbox.Text);
                if (t <= -1)
                    throw new Exception("Число меньше или равен нулю");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                TextBox_MaxCashbox.Text = "0";
            }

            TextBox_MaxCashbox.Text = Regex.Replace(TextBox_MaxCashbox.Text, @"(\d+(,|\.)\d{2})\d+", @"$1");
            TextBox_MaxCashbox.Select(TextBox_MaxCashbox.Text.Length, 0);
        }
    }
}
