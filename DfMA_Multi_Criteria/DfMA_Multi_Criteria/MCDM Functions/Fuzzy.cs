using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.MCDM_Functions
{
    public class Fuzzy
    {
        #region General
        public static Tuple<double, double, double> ToFuzzyNumber(double number)
        {
            if (number < 1 || number > 9) { return Tuple.Create(0.0, 0.0, 0.0); }

            if (number == 1 || number == 9) return Tuple.Create(number, number, number);

            else { return Tuple.Create(number - 1, number, number + 1); }
        }

        public static Tuple<double, double, double> FuzzyPowerOfNegativeOne(Tuple<double, double, double> fuzzyNumber)
        {
            return Tuple.Create(1 / fuzzyNumber.Item3, 1 / fuzzyNumber.Item2, 1 / fuzzyNumber.Item3);
        }

        public static double ToNumber(Tuple<double, double, double> fuzzyNumber)
        {
            return (fuzzyNumber.Item1 + fuzzyNumber.Item2 + fuzzyNumber.Item3) / 3;
        }

        public static List<List<Tuple<double, double, double>>> TranslateToFuzzyMatrix(List<List<double>> numberMatrix)
        {
            List<List<Tuple<double, double, double>>> fuzzymatrix = new List<List<Tuple<double, double, double>>>();
            foreach (List<double> row in numberMatrix)
            {
                List<Tuple<double, double, double>> translatedRow = new List<Tuple<double, double, double>>();
                foreach (double value in row)
                {
                    if (value >= 1) { translatedRow.Add(ToFuzzyNumber(value)); }
                    else
                    {
                        Tuple<double, double, double> denominator = ToFuzzyNumber(1 / value);
                        translatedRow.Add(FuzzyPowerOfNegativeOne(denominator));
                    }
                }
                fuzzymatrix.Add(translatedRow);
            }

            return fuzzymatrix;
        }
        #endregion

        #region Fuzzy TOPSIS
        public static void GroupDecisionMakingMatrix()
        { throw new NotImplementedException(); }

        public static void GroupDecisionMakingWeightage()
        { throw new NotImplementedException(); }

        public static List<List<Tuple<double, double, double>>> NormalisingFuzzyMatrix(List<List<Tuple<double, double, double>>> fuzzyMatrix, List<int>BeneficialCriteriaIndex)
        { 
            throw new NotFiniteNumberException();
        }
        #endregion
    }
}
