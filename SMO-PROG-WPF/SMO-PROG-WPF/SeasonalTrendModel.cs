using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMO_PROG_WPF
{
    internal class SeasonalTrendModel
    {
        private double averageRelativeError;
        private List<double> SS; //скользящая средняя
        private List<double> Yt1;
        private List<double> Yt2MinusS;
        private List<double> S; //сезонность
        private List<double> Yt2;
        private List<double> forecast; //прогноз
        private List<double> Wrong;

        public SeasonalTrendModel()
        {
            averageRelativeError = 0;
            SS = new List<double>();
            Yt1 = new List<double>();
            Yt2MinusS = new List<double>();
            S = new List<double>();
            Yt2 = new List<double>();
            forecast = new List<double>();
            Wrong = new List<double>();
        }

        public List<double> Forecast(List<double> y, int periodForecast, int howForecast)
        {
            for (int i = 0; i < periodForecast/2; i++)
                SS.Add(0);

            if (periodForecast%2 != 0)
            {
                MessageBox.Show("Период в тренд-сезонной модели должен быть чётным");
                return null;
            }
            int currJ = 0;
            for (int i = 0; i < y.Count - periodForecast; i++)
            {
                double summ = 0, summ2 = 0;
                for (int j = currJ; j <= (currJ + periodForecast); j++)
                    // <= тк скользящая средняя у нас чётная (т.е. считаем periodForecast + 1)
                {
                    if (j == currJ || j == currJ + periodForecast)
                        summ += y[j]/2;
                    else
                        summ2 += y[j];

                }
                currJ++;
                SS.Add((summ + summ2)/periodForecast);
                Yt1.Add(y[SS.Count - 1] - SS.Last());
            }
            int temp = y.Count, count = 0;
            while (temp > 0)
            {
                temp -= periodForecast;
                count++;
            }
            if (temp < 0)
            {
                MessageBox.Show("Данные в тренд-сезонной модели должны быть за полный период");
                return null;
            }
            //далее транспонируем в матрицу Yt
            double[,] matrix = new double[count, periodForecast];
            int yIndx = -1;
            for (int i = 0; i < count; i++)
                for (int j = 0; j < periodForecast; j++)
                {
                    if (i == 0) //на первой строке в начале не все есть значения
                        if (j < periodForecast/2)
                        {
                            matrix[i, j] = 0;
                            continue;
                        }
                    if (i == count - 1) //и на  последней тоже
                        if (j >= periodForecast/2)
                        {
                            matrix[i, j] = 0;
                            continue;
                        }
                    matrix[i, j] = Yt1[++yIndx];
                }
            //вычисляем среднее арифметическое у столбцов матрицы. идём по столбцам
            List<double> x = new List<double>();

            for (int j = 0; j < periodForecast; j++)
            {
                int c = 0;
                double summ = 0;          //считаем сколько чисел в столбце
                for (int i = 0; i < count; i++)
                {
                    if (i == 0) //на первой строке в начале не все есть значения
                        if (j < periodForecast/2)
                            continue;
                    if (i == count - 1) //и на  последней тоже
                        if (j >= periodForecast/2)
                            continue;
                    c++;
                    summ += matrix[i, j];
                }
                x.Add(summ/c);
            }
            double k = x.Sum()/periodForecast;
            List<double> Scp = new List<double>(); //разность x и k
            for (int i = 0; i < x.Count; i++)
                Scp.Add(Math.Round(x[i] - k,2));

             if (Math.Round(Scp.Sum(),0) != 0)
            {
                MessageBox.Show("Сумма Scp не равна нулю");
                return null;
            }

            S = Scp;
            int indxS = 0;
            for (int i = 0; i < y.Count; i++)
            {
                if (indxS + 1 > periodForecast)
                    indxS = 0;
                Yt2MinusS.Add(y[i] - S[indxS]);
                indxS++;
            }
            //наконец считаем прогноз
            double b = MLQ_B(Yt2MinusS);
            double a = MLQ_A(Yt2MinusS);
             indxS = 0;
            for (int i = 0; i < y.Count; i++)
            {
                if (indxS + 1 > periodForecast)
                    indxS = 0;

                Yt2.Add(b + a*(i+1)+Scp[indxS]);
                indxS++;
                Wrong.Add(Math.Abs(y[i] - Yt2.Last())/y[i] * 100);
            }
            averageRelativeError = Wrong.Sum()/Wrong.Count;

            for (int i = y.Count; i < y.Count + periodForecast; i++)
            {
                if (indxS + 1 > periodForecast)
                    indxS = 0;
                forecast.Add(Math.Round(b + a * (i + 1) + Scp[indxS], 0));
                indxS++;
            }
            return forecast;
        }

        public double GetARE()
        { return averageRelativeError;}
        private double MLQ_A(List<double> y)
        {
            double a = 0, sumX = 0, sumXX = 0, sumF = 0, sumFX = 0;

            for (int i = 0; i < y.Count; i++)
            {
                // считаем суммы
                sumF += y[i];
                sumX += i + 1;
                sumXX += (i + 1) * (i + 1);
                sumFX += y[i] * (i + 1);
            }
            return (sumFX - (sumX * sumF) / y.Count) / (sumXX - sumX * sumX / y.Count);
        }
        private double MLQ_B(List<double> y)
        {
            double a = 0, sumX = 0, sumF = 0;

            for (int i = 0; i < y.Count; i++)
            {
                // считаем суммы
                sumF += y[i];
                sumX += i + 1;
            }
            return (sumF / y.Count) - (MLQ_A(y) * sumX / y.Count);
        }
    }
}
