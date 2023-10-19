using Eto.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.CalculationFunctions
{
    public class ElementSectionBySpan
    {
        public static List<int> FilterValidDesignation(List<double> depthData, Tuple<double, double>FilterCondition)
        { 
            List<int> index = new List<int>();
            for (int i = 0; i < depthData.Count; i++)
            {
            }
            throw new NotImplementedException();
        }
        public static List<Tuple<double, double>> BeamSpanTomaterialFilter(List<double> spanData, int material, double tolerence)
        {
            List<Tuple<double, double>> depthDimensionFilter = new List<Tuple<double, double>>();
            List<double> uniqueList = new List<double>();
            foreach( double spanRecord in spanData)
            {
                if (uniqueList.Count == 0 || !uniqueList.Contains(spanRecord))
                {
                    uniqueList.Add(spanRecord);
                    double minDepth = spanRecord / materialConstantBeam(material);
                    depthDimensionFilter.Add(new Tuple<double, double>(minDepth, minDepth * (1 + tolerence)));
                } 
            }
            return depthDimensionFilter;
        }
        
        private static double materialConstantBeam(int materialType)
        {
            double materialConstant = 0;
            if (materialType == 0) { materialConstant = 20; } //steel and concrete
            if (materialType == 1) { materialConstant = 15; } //timber

            return materialConstant;
        }
    }
}
