using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Data;
using Phoenix.MCDM_Functions;
using Rhino.DocObjects;
using System.Data.Common;
using Grasshopper.Kernel.Types;

namespace Phoenix.Components.MCDM
{
    public class TOPSISMethods
    {
        public static List<double> TOPSISRanking(List<List<double>> Matrix, List<double> CriteriaWeights, List<double> beneficialIndex)
        {
            List<List<double>> normalised = MatrixVectorNormalisation(Matrix);
            List<List<double>> weighted = WeightedNormalisedMatrix(normalised, CriteriaWeights);
            EuclideanDistance(weighted, beneficialIndex, out List<double>ideal, out List<double>anti);
            List<double> performance = PerformanceScore(ideal, anti);
            
            return performance;
        }

        public static List<List<double>> MatrixVectorNormalisation(List<List<double>> rawMatrix)
        {
            List<double> columnSquared = new List<double>();
            for (int i = 0; i < rawMatrix[0].Count; i++)
            {
                double perColumnSquare = 0;
                foreach (List<double> row in rawMatrix)
                { 
                    perColumnSquare += row[i] * row[i];
                }
                columnSquared.Add(Math.Sqrt(perColumnSquare));
            }
            
            List<List<double>> normalisedmatrix = new List<List<double>>();
            
            foreach (List<double> row in rawMatrix)
            { 
                List<double> normalisedRow = new List<double>();
                double count = row.Count;
                for (int i = 0; i < count; i++)
                    normalisedRow.Add(row[i] / columnSquared[i]);
                normalisedmatrix.Add(normalisedRow);
            }
            return normalisedmatrix;
        }

        public static List<List<double>> WeightedNormalisedMatrix(List<List<double>> normalisedMatrix, List<double> criteriaWeights)
        {
            List<List<double>> weightedNormalisedMatrix = new List<List<double>>();
            int itemCount = normalisedMatrix[0].Count;
            int criteriaNumber = criteriaWeights.Count;


            for (int i = 0; i < normalisedMatrix.Count; i++)
            {
                List<double> weightedRow = new List<double>();
                List<double> row = normalisedMatrix[i];
                for (int j = 0; j < itemCount; j++)
                {
                    double weightedValue = row[j] * criteriaWeights[j];
                    weightedRow.Add(weightedValue);
                }
                weightedNormalisedMatrix.Add(weightedRow);
            }
            
            return weightedNormalisedMatrix;
        }

        public static void EuclideanDistance(List<List<double>> weightedNormalisedMatrix, List<double> index, out List<double>idealDistance, out List<double>antiIdealDistance)
        {
            double count = weightedNormalisedMatrix[0].Count;
            List<double> idealInColumn = new List<double>();
            List<double> antiidealInColumn = new List<double>();
            for (int i = 0; i < count; i++)
            {
                List<double> valuePicker = new List<double>();
                foreach (List<double> row in weightedNormalisedMatrix)
                {
                    valuePicker.Add(row[i]);
                }
                if (index.Contains(i)) { idealInColumn.Add(valuePicker.Min()); antiidealInColumn.Add(valuePicker.Max()); }
                else
                {
                    idealInColumn.Add(valuePicker.Max()); antiidealInColumn.Add(valuePicker.Min());
                }
            }


            List<double> ideal_Distance = new List<double>();
            List<double> antiideal_Distance = new List<double>();
            for (int i = 0; i < weightedNormalisedMatrix.Count; i++)
            {
                double ideal = default;
                double anti = default;
                for (int j = 0; j < weightedNormalisedMatrix[0].Count; j++)
                {
                    double plus = weightedNormalisedMatrix[i][j] - idealInColumn[j];
                    double minus = weightedNormalisedMatrix[i][j] - antiidealInColumn[j];
                    ideal = ideal + (plus * plus);
                    anti = anti + (minus * minus);
                }
                ideal_Distance.Add(Math.Sqrt(ideal));
                antiideal_Distance.Add(Math.Sqrt(anti));
            }

            idealDistance = ideal_Distance;
            antiIdealDistance = antiideal_Distance;
        }

        public static List<double> PerformanceScore(List<double> idealDistance, List<double> antiIdealDistance)
        { 
            if (idealDistance.Count != antiIdealDistance.Count) return new List<double> { };
            List<double> result = new List<double>();
            for (int i = 0; i < idealDistance.Count; i++)
            { 
                double score = antiIdealDistance[i] / (idealDistance[i] + antiIdealDistance[i]);
                result.Add(score);
            }
            return result;
        }
    }
}
