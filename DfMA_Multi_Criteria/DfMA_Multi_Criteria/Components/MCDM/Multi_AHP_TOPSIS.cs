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

namespace Phoenix.Components
{
    public class MultiAHPTOPSIS : GH_Component
    {

        public MultiAHPTOPSIS()
          : base("Multi AHPTOPSIS", "MAHPTOPSIS",
            "This component attempts to rank multiple given scenarios",
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
            if (!DA.GetDataTree(0, out input)) return;
            if (!DA.GetDataList(1, weights)) return;
            if (!DA.GetDataList(2, index)) return;

            
            
            List<List<double>> Matrix = GeneralMethods.ToSystem(input);


            List<double> output = TOPSISMethods.TOPSISRanking(Matrix, weights, index);
            DA.SetDataList(0, output);
        }
    }
}