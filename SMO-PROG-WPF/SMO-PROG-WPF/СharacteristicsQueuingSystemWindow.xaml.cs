using System;
using System.Collections.Generic;
using System.Windows;


namespace SMO_PROG_WPF
{

    public partial class СharacteristicsQueuingSystemWindow : Window
    {
        public СharacteristicsQueuingSystemWindow(List <double> characteristics)
        {
            InitializeComponent();
            TextBox_Mu.Text = Math.Round(characteristics[0], 3).ToString();
            TextBox_Lambda.Text = Math.Round(characteristics[1], 3).ToString();
            TextBox_P.Text = Math.Round(characteristics[2], 3).ToString();
            TextBox_P0.Text = Math.Round(characteristics[3], 3).ToString();
            TextBox_Psi.Text = Math.Round(characteristics[4], 3).ToString();
            TextBox_Ro.Text = Math.Round(characteristics[5], 3).ToString();
            TextBox_Nobs.Text = Math.Round(characteristics[6], 3).ToString();
            TextBox_Noch.Text = Math.Round(characteristics[7], 3).ToString();
            TextBox_A.Text = Math.Round(characteristics[8], 3).ToString();
            TextBox_Q.Text = Math.Round(characteristics[9], 3).ToString();
            TextBox_ARE.Text = Math.Round(characteristics[10], 2).ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
