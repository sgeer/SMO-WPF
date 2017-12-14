using System.Collections.Generic;
using System.Linq;


namespace SMO_PROG_WPF
{
    class BusinessLogic
    {
        private List<СharacteristicsQueuingSystem> chQueuingSystems;
        private SeasonalTrendModel STM;
        private bool flagHW;

        public BusinessLogic()
        {
            chQueuingSystems = new List<СharacteristicsQueuingSystem>();
            STM = new SeasonalTrendModel();
        }

        public List<Line> GetTabulatedLines(List<double> quantity, double lastTime, double t, int queueLength, int periodData, int predictSteps,
                                            int maxCashbox, double serviceTime, bool infiniteQueue, bool limitedQueue, bool limitedTime)
        {
            List<Line> lines = new List<Line>();
            List<double> result = new List<double>();
            QueuingSystem queuingSystem = new QueuingSystem();
            chQueuingSystems = new List<СharacteristicsQueuingSystem>();
            STM = new SeasonalTrendModel();
            result = STM.Forecast(quantity, periodData, predictSteps);


            for (int i = 0; i < result.Count; i++)
            {
                lastTime++;
                if (lastTime >= 24)
                    lastTime = 0;
                if (limitedQueue)
                    chQueuingSystems.Add(queuingSystem.MultiChannelLimitedQueue(1/serviceTime, result[i]/60, maxCashbox, queueLength));
                else if (infiniteQueue)
                    chQueuingSystems.Add(queuingSystem.MultiChannelInfiniteQueue(1/serviceTime, result[i]/60, maxCashbox));
                else
                    chQueuingSystems.Add(queuingSystem.MultiChannelLimitedQueueAndTime(1/serviceTime, result[i]/60, maxCashbox, queueLength, t));

                lines.Add(new Line(i + 1, lastTime.ToString() + ":00", result[i].ToString(), chQueuingSystems.Last().n().ToString()));
            }

            return lines;
        }

        public List<double> СharacteristicsQS(int index)
        {
            if (index < 0 || index >= chQueuingSystems.Count)
                return null;
            List <double> result = new List<double>();
            result.Add(chQueuingSystems[index].Mu());
            result.Add(chQueuingSystems[index].Lambda());
            result.Add(chQueuingSystems[index].P());
            result.Add(chQueuingSystems[index].P0());
            result.Add(chQueuingSystems[index].Psi());
            result.Add(chQueuingSystems[index].Ro());
            result.Add(chQueuingSystems[index].Nobs());
            result.Add(chQueuingSystems[index].Noch());
            result.Add(chQueuingSystems[index].A());
            result.Add(chQueuingSystems[index].Q());
            result.Add(STM.GetARE());
            return result;
        }

    }
}
