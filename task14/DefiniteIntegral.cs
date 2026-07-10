using System;
using System.Threading;

namespace task14
{
    public class DefiniteIntegral
    {
        private static readonly object _lock = new object();

        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
        {
            if (threadsNumber <= 0)
                throw new ArgumentException("Количество потоков > 0");
            

            if (a == b) return 0.0;
            double sign = 1.0;

            if (a > b)
            {
                double c = a;
                a = b;
                b = c;
                sign = -1.0;}
            
            double totalResult = 0.0;
            double segmentLength = (b - a) / threadsNumber;
            
            using var barrier = new Barrier(threadsNumber + 1);
            
            Thread[] threads = new Thread[threadsNumber];
            
            for (int i = 0; i < threadsNumber; i++)
            {
                double segmentStart = a + i * segmentLength;
                double segmentEnd = (i == threadsNumber - 1) ? b : a + (i + 1) * segmentLength;
                
                threads[i] = new Thread(() =>
                {
                    double localResult = CalculateIntegral(segmentStart, segmentEnd, function, step); 
                    
                    lock (_lock)
                    {
                        totalResult += localResult;
                    }
                    
                    barrier.SignalAndWait();
                });
                
                threads[i].Start();
            }
            
            barrier.SignalAndWait();
            
            return sign * totalResult;
        }
        
        static double CalculateIntegral(double a, double b, Func<double, double> function, double step)
        {
            double result = 0.0;
            
            for (double x = a; x < b; x += step)
            {
                double nextx = Math.Min(x + step, b);
                result += (function(x) + function(nextx)) / 2.0 * (nextx - x);}
            
            return result;
        }
    }
}
