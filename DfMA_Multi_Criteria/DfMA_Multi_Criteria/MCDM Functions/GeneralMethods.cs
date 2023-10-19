using CsvHelper.Configuration.Attributes;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.MCDM_Functions
{
    public class GeneralMethods
    {
        //This method provides normalisation for any numeric input, map them towards a sum of 1
        public static List<double> SimpleNormalisedWeights(List<double>rawWeights)
        { 
            
            List<double> normalised = new List<double>();
            double sum = rawWeights.Sum();
            foreach (double value in rawWeights)
                normalised.Add(value / sum);

            return normalised;
        }

        public static void Transpose3DMatrix(GH_Structure<GH_Number> input)
        {
            IList<GH_Path> paths = input.Paths;
            GH_Path last = paths.Last();
            DataTree<double> output = new DataTree<double>();
        }

        public static List<double> RootBranchLength(GH_Structure<GH_Number> input)
        {
            IList<GH_Path> paths = input.Paths;
            List<double> length = new List<double>();
            int count = 0;
            int test = 0;
            foreach (GH_Path path in paths)
            {
                test = path.Indices.Last();
                if ((count > test) == true)
                {
                    length.Add(count);
                    count = 0;
                }
                
                count++;
            }
            int last = paths.Last().Indices.Last();
            length.Add(last + 1);
            return length;
        }

        public static double ToSystem(GH_Number num) { return num.Value; }
        public static List<double> ToSystem(List<GH_Number> list)
        {
            List<double> output = new List<double>();
            foreach (GH_Number value in list) { output.Add(value.Value); }
            return output;
        }
        public static List<List<double>> ToSystem(GH_Structure<GH_Number> input)
        {
            List<List<double>> fakeMatrix = new List<List<double>>();
            IList<GH_Path> paths = input.Paths;
            foreach (GH_Path path in paths)
            {
                List<double> branch = new List<double>();
                foreach (GH_Number item in input.get_Branch(path))
                {
                    branch.Add(item.Value);
                }
                fakeMatrix.Add(branch);
            }
            return fakeMatrix;
        }
    }
}
