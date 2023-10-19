
using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using System.Linq;

namespace Phoenix.Components
{
    public class PairWiseMatrix : GH_Component
    {

        public PairWiseMatrix()
          : base("Construct Pair Wise Matrix", "AHPPWMatrix",
            "Construct Pair-Wise Matrix for AHP",
            "Phoenix", "MCDM")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("7a3a27ad-03e2-476d-9948-b7299478f6e4");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Relative Importance", "RIm", "Importance from 1-9", GH_ParamAccess.tree);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Criteria Weights", "Ws", "Normalised Weights", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<GH_Number> input = new GH_Structure<GH_Number>();
            if (!DA.GetDataTree(0, out input)) return;

            List<List<double>> Matrix = GeneralMethods.ToSystem(input);

            List<double> output = AHPMethods.CalculateWeights(Matrix);
            DA.SetDataList(0, output);
        }
    }
}
