using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.MCDM_Functions
{
    public class AHPMethods
    {
        public static List<double> CalculateWeights(List<List<double>> RawMatrix)
        {
            NormalisingMatrix(RawMatrix, out List<List<double>> normalisedMatrix);//check original matrix, is it as simple as index / row

            CalculateCriteriaWeight(normalisedMatrix, out List<double> criteriaWeights);

            return criteriaWeights;
        }

        public static void ConstructMatrix(List<double>RelativeImportance, out List<List<double>>rawMatrix)
        { 
            //looking into an easier constructor rather than typing out the entire matrix in GH Panel

            throw new NotImplementedException();   
        }

        public static void NormalisingMatrix(List<List<double>> rawmatrix, out List<List<double>> normalisedMatrix)
        {
            double count = rawmatrix.Count;
            List<double> sumEach = new List<double>();
            List<List<double>> Normalisedratios = new List<List<double>>();
            for (int i = 0; i < count; i++)
            {
                List<double> valuePicker = new List<double>();
                foreach (List<double> row in rawmatrix)
                {
                    valuePicker.Add(row[i]);
                }
                sumEach.Add(valuePicker.Sum());
            }

            foreach (List<double> row in rawmatrix)
            {
                List<double> normalisedRow = new List<double>();
                for (int i = 0; i < count; i++)
                { 
                    double normalisedRatio = row[i] / sumEach[i];
                    normalisedRow.Add(normalisedRatio);
                }
                Normalisedratios.Add(normalisedRow);
            }

            normalisedMatrix = Normalisedratios;
        }

        public static void CalculateCriteriaWeight(List<List<double>> NormalisedMatrix, out List<double> criteriaWeights)
        {
            List<double> weights = new List<double>();
            foreach (List<double> row in NormalisedMatrix)
            { 
                double criteriaWeight = row.Sum() / row.Count;
                weights.Add(criteriaWeight);
            }
            criteriaWeights = weights;
        }

        public void ConsistancyRatio()
        { 
            throw new NotImplementedException();
        }
    }
}
