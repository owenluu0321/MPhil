using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using Phoenix.MCDM_Functions.ExtensionMethods;
using System.Linq;

namespace Phoenix.Components
{
    public class SubCriteraWeights : GH_Component
    {

        public SubCriteraWeights()
          : base("Create Sub-Criteria Weightage", "subW",
            "Create subcriteria Weightage",
            "Phoenix", "MCDM")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("d04c2723-2e39-4d99-9b76-c67598ca0312");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Normalised Weights", "NprW", "Normalised weights of criterias", GH_ParamAccess.list);
            pManager.AddNumberParameter("Index of Criteria", "InC", "Index of criteria to create subcriteria", GH_ParamAccess.list);
            pManager.AddNumberParameter("Local Weightages", "LocW", "Local Weightages", GH_ParamAccess.tree);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Normalised Criteria Weights", "NorWs", "Normalised Criteria Weights", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> rawWeights = new List<double>();
            List<double>index = new List<double>();
            GH_Structure<GH_Number> localSubWeights = new GH_Structure<GH_Number>();
            if (!DA.GetDataList(0, rawWeights)) return;
            if (!DA.GetDataList(1, index)) return;
            if (!DA.GetDataTree(2, out localSubWeights)) return;

            List<List<double>> subMatrix = GeneralMethods.ToSystem(localSubWeights);
            Grasshopper.DataTree<double> output = new Grasshopper.DataTree<double>();
            if (index.Count != subMatrix.Count) { output = default; }
            else 
            { 
                int count = rawWeights.Count;
                int iteration = 0;
                for (int i = 0; i < count; i++)
                {
                    if (index.Contains(i) == true)
                    {
                        IEnumerable<double> globalW = WeightageExtension.SubCriteriaWeightages(rawWeights[i], subMatrix[iteration]).AsEnumerable();
                        output.AddRange(globalW, new GH_Path(i));
                        iteration++;
                    }

                    else
                    {
                        output.Add(rawWeights[i], new GH_Path(i));
                    }
                }
            }

            
            DA.SetDataTree(0, output);
        }
    }
}