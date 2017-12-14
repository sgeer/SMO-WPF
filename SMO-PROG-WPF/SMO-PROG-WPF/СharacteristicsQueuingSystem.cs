using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMO_PROG_WPF
{
    public class СharacteristicsQueuingSystem
    {
         private double _Q;               //Это вероятность того, что заявка будет обслужена.
         private double _A;               //тоже самое, что и Q, но только в относительных величинах
         private double _p0;
         private double _p;
         private double _mu;              //производительность канала (номинальная). 
         private double _lambda;          //входной поток
         private double _ro;           //относительная интенсивность потока = Mu/Lambda
         private double _psi;           //интенсивность на один канал
         private double _n;
        private double _Nobs;
        private double _Noch;
        public СharacteristicsQueuingSystem(double mu, double lambda, double ro, double psi, double p0, double p, double Nobs, double Noch, double a, double q, double n)
        {
            _mu = mu;
            _p0 = p0;
            _p = p;
            _lambda = lambda;
            _ro = ro;
            _psi = psi;
            _A = a;
            _Q = q;
            _Nobs = Nobs;
            _Noch = Noch;
            _n = n;
        }

        public double n()
        {
            return _n;
        }
        public double Mu()
        {
            return _mu;
        }
        public double Lambda()
        {
            return _lambda;
        }
        public double P0()
        {
            return _p0;
        }
        public double P()
        {
            return _p;
        }
        public double Ro()
        {
            return _ro;
        }
        public double Psi()
        {
            return _psi;
        }
        public double Nobs()
        {
            return _Nobs;
        }
        public double Noch()
        {
            return _Noch;
        }
        public double A()
        {
            return _A;
        }
        public double Q()
        {
            return _Q;
        }

    }
}
