using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using System.Linq;
using Phoenix.Components.MCDM;
using Grasshopper;

namespace Phoenix.Components
{
    public class AHPTOPSIS : GH_Component
    {

        public AHPTOPSIS()
          : base("AHPTOPSIS Ranking", "AHPTOPSIS",
            "Compute the rank of given alternatives by AHP TOPSIS method",
            "Phoenix", "MCDM")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("21750b01-7477-437c-a14d-8c675911ad1e");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Alternative Data", "Ad", "Data of each options to evaluate and compare", GH_ParamAccess.tree);
            pManager.AddNumberParameter("Criteria Weights", "Ws", "Normalised Weights", GH_ParamAccess.list);
            pManager.AddNumberParameter("Benificial Criteria Index", "BInd", "Beneficial Criteria Index", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Score", "Sc", "Performance Socre for ranking", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<GH_Number> input = new GH_Structure<GH_Number>();
            List<double> weights = new List<double>();
            List<double> index = new List<double>();
            List<double> ideal = new List<double>();
            List<double> antiIdeal = new List<double>();
            if (!DA.GetDataTree(0, out input)) return;
            if (!DA.GetDataList(1, weights)) return;
            if (!DA.GetDataList(2, index)) return;

            int indexLongest = input.LongestPathIndex();
            IList<GH_Path> paths = input.Paths;
            int treeDepth = paths[indexLongest].Length;

            List<List<double>> output = new List<List<double>>();
            List<double> eachType = new List<double>();
            if (treeDepth == 1)
            {
                List<List<double>> Matrix = GeneralMethods.ToSystem(input);
                eachType = TOPSISMethods.TOPSISRanking(Matrix, weights, index);
                output.Add(eachType);
            }

            else 
            {
                List<double> subBranchNumber = GeneralMethods.RootBranchLength(input);
                int count = subBranchNumber.Count;

                
                for (int i = 0; i < count; i++)
                {
                    List<List<double>> Matrix = new List<List<double>>();
                    for (int j = 0; j < subBranchNumber[i]; j++)
                    {
                        List<double> perBranch = GeneralMethods.ToSystem(input.Branches[6 * i + j]);
                        Matrix.Add(perBranch);
                    }
                    eachType = TOPSISMethods.TOPSISRanking(Matrix, weights, index);
                    
                    output.Insert(i, eachType);
                }
            }
            DataTree<double> outTree = new DataTree<double>();
            for (int i = 0; i < output.Count; i++)
            {
                outTree.AddRange(output[i], new GH_Path(i));
            }

            DA.SetDataTree(0, outTree);
        }
    }
}