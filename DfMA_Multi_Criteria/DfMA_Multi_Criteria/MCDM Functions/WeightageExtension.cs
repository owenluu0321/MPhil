using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.MCDM_Functions.ExtensionMethods
{
    public static class WeightageExtension
    {
        public static List<double> SubCriteriaWeightages(this double criteriaWeightage, List<double> localWeighting)
        { 
            List<double> result = new List<double>();
            foreach (double weightage in localWeighting) { result.Add(weightage * criteriaWeightage); }
            return result;
        }
    }
}
