using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SMO_PROG_WPF
{

    class QueuingSystem
    {
        // private double Q;               //Это вероятность того, что заявка будет обслужена.
        //private double A;               //тоже самое, что и Q, но только в относительных величинах
        //private double Mu;              //производительность канала (номинальная). 
        // private double Lambda;          //входной поток
        // private double Ro;           //относительная интенсивность потока = Mu/Lambda
        // private double psi;           //интенсивность на один канал
        private double _v;
        private double _ro;
        private double _n;
        private double _psi;
        private double _p;
        private double _Q;
        private double _N;
        private double _A;
        private double _po;
        private double _Noch;
        private double _Nob;

        public QueuingSystem()
        {
            _v = 0;
            _ro = 0;
            _n = 1;
            _psi = 0;
            _p = 0;
            _Q = 0;
            _N = 0;
            _A = 0;
            _po = 0;
            _Noch = 0;
            _Nob = 0;
        }

        public СharacteristicsQueuingSystem MultiChannelInfiniteQueue(double mu, double lambda, double maxCashbox)
        {

            //  double ro = lambda / mu;        // относительная интенсивность входящего потока
                                                                                //среднее число занятых каналов

            ClearCharacteristics();
            _ro = lambda / mu;
            _n = 1;


            for (int i = 1; i <= maxCashbox; i++)
            {
                //_psi = _ro / _n;              //интенсивность на один канал < 1, иначе приходящих людей будет больше, чем обслужанных
                _psi = _ro / i;
                if (_psi >= 1)
                {
                   // _n++;
                    continue;
                }
                double r = _ro;
                //_po = p0(_n, _ro, _psi);
                _po = p0(i, _ro, _psi);
                // double lq = Lq(_n, _ro, _psi, _po);
                double lq = Lq(i, _ro, _psi, _po);
                _Q = 1 - _p;
                _Nob = _ro;                                                                                //среднее число занятых каналов
                //_Noch  = (Math.Pow(_n, _n) / Factorial(_n)) * Math.Pow(_psi, _n + 1)/(Math.Pow(1-_psi,2)) * _po; //среднее число заявок в очереди
                _Noch  = (Math.Pow(i, i) / Factorial(i)) * Math.Pow(_psi, i + 1)/(Math.Pow(1-_psi,2)) * _po; //среднее число заявок в очереди
                _N = _ro * _Q;
                _A = lambda * _Q;
                // MessageBox.Show("L: " + L(r, lq).ToString());
               // if (W(lambda, L(r, lq)) > mu) //средняя продолжительность пребывания клиента (заявки на обслуживание) в очереди
                //    n++;
                //else
                //    break;
                //n++;
                _n = i;
                break;
            }

            return new СharacteristicsQueuingSystem(mu, lambda, _ro, _psi, _po, _p, _Nob, _Noch, _A, _Q, _n); ;
        }
        public СharacteristicsQueuingSystem MultiChannelLimitedQueue(double mu, double lambda, double maxCashbox, double m)
        {

            ClearCharacteristics();
            _ro = lambda / mu;
            _n = 1;

            for (int i = 1; i <= maxCashbox; i++)
            {
                // if (n > ro)         // (n<ro) - условие на число каналов, которые необходимо иметь, чтобы система справлялась с потоком заявок
                //   return n--;

                _psi = _ro/_n; //интенсивность на один канал

                //вероятность простоя
                _po = p0(_n, _ro, _psi, m);
                //вероятность отказа
                _p = _po*Math.Pow(_psi, _n + m)*(Math.Pow(_n, _n)/Factorial(_n));

                _Q = 1 - _p;
                _N = _ro*_Q;
                _A = lambda*_Q;

                if (_p*100 <= 5)
                    break;
                _n++;
            }
            _Noch = (Math.Pow(_n, _n)/Factorial(_n))*Math.Pow(_psi, _n + 1)*
                ((1 - (m + 1)*Math.Pow(_psi, m) + m*Math.Pow(_psi, m + 1))/(Math.Pow(1 - _psi, 2)))*_po;
            _Nob = _ro * _Q;
            double Nsys = _Nob + _Noch;
            double Toch = _Noch/lambda;
            double Tsys = Nsys/lambda;
            return new СharacteristicsQueuingSystem(mu, lambda, _ro, _psi, _po, _p, _Nob, _Noch, _A, _Q, _n);
        }
        public СharacteristicsQueuingSystem MultiChannelLimitedQueueAndTime(double mu, double lambda, double maxCashbox, double m, double t)
        {
            ClearCharacteristics();
            _v = 1/t;
            _ro = lambda/mu;
            _n = 1;

            for (int c = 1; c <= maxCashbox; c++)
            {
                _psi = _ro / _n;              //интенсивность на один канал < 1, иначе приходящих людей будет больше, чем обслужанных
                if (_psi >= 1)
                {
                    _n++;
                    continue;
                }
                //считаем p0 (вероятность простоя)
                double part1 = 0;

                for (int k = 0; k <= _n; k++)
                    part1 += Math.Pow(_ro, k)/Factorial(k);

                double part2 = 0;
                for (int i = 1; i <= m; i++)
                {
                    double mult = 1;
                    for (int l = 1; l <= i; l++)
                    {
                        mult *= _n + l*_v/mu;
                    }
                    mult = Math.Pow(_ro, i)/mult;
                    part2 += mult;
                }
                part2 *= Math.Pow(_ro, _n)/Factorial(_n);
                _po = part1 + part2;
                _po = Math.Pow(_po, -1);

                //считаем Pn
                double Pn = Math.Pow(_ro, _n)/Factorial(_n)*_po;

                //считаем p
                double mul = 1;
                for (int i = 1; i <= m; i++)
                {
                    mul *= _n + i*_v/mu;
                }
                //вероятность отказа
                _p = Pn*Math.Pow(_ro, m)/mul;

                //считаем Noch
                double summ = 0;
                for (int i = 1; i <= m; i++)
                {
                    double mult = 1;
                    for (int l = 1; l <= i; l++)
                    {
                        mult *= _n + l * _v / mu;
                    }
                    summ += (i * Math.Pow(_ro, i) )/ mult;
                    //mult = Math.Pow(_ro, i) / mult;
                    //summ += i * Math.Pow(_ro,i)/mult;
                    // summ += i / mult;
                }
                 _Noch = Pn*summ;
              //  _Noch = summ;
                //находим Nobs
                summ = 0;
                for (int k = 1; k <= _n; k++)
                    summ += k*Math.Pow(_ro, k)/Factorial(k) * _po;
                _Nob = summ;
                summ = 0;
                for (int i = 1; i <= m; i++)
                {
                    double mult = 1;                    //считаем Pn+i
                    for (int l = 1; l <= i; l++)
                    {
                        mult *= _n + l * _v / mu;
                    }
                    mult = Math.Pow(_ro, i) / mult *Pn;   //считаем Pn+i
                    summ += _n*mult;
                }
                _Nob += summ;


                _Q = 1 - _p;
                _A = lambda*_Q;
                if (_p * 100 <= 5)
                    break;
                _n++;
            }
            return new СharacteristicsQueuingSystem(mu, lambda, _ro, _psi, _po, _p, _Nob, _Noch, _A, _Q, _n);
        }
        private double Factorial(double n)
        {
            if (n == 0)
                return 1;
            return n * Factorial(n - 1);
        }
        private double p0(double n, double ro, double psi)
        {
            double result = 0;
            for (int i = 0; i <= n; i++)
            {
                result += ( Math.Pow(ro,i)/Factorial(i));
            }
            result += ( (Math.Pow(ro, n)*psi)/(Factorial(n)*(1 - psi)) );
            return Math.Pow(result,-1);
        }
        private double p0(double n, double ro, double psi, double m)
        {
            double result = 0;
            for (int i = 0; i <= n; i++)
            {
                result += (Math.Pow(ro, i) / Factorial(i));
            }
            result += ((Math.Pow(ro, n) * psi) * (1 - Math.Pow(psi,m)) / (Factorial(n) * (1 - psi)));
            return Math.Pow(result, -1);
        }

        private double Lq(double n, double ro, double psi, double p0)
        { return (Math.Pow(ro, n + 1)*p0)/(n*Factorial(n)*Math.Pow(1 - psi, 2));}

        private void ClearCharacteristics()
        {
            _v = 0;
            _ro = 0;
            _n = 0;
            _psi = 0;
            _p = 0;
            _Q = 0;
            _N = 0;
            _A = 0;
            _po = 0;
            _Noch = 0;
            _Nob = 0;
        }
    }
}
