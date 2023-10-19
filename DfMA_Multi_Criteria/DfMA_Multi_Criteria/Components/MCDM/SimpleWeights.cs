using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Phoenix.Components
{
    public class CreateSimpleWeights : GH_Component
    {

        public CreateSimpleWeights()
          : base("Create Simple Weights", "simpW",
            "Create nromalised weights upon given input",
            "Phoenix", "MCDM")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("456e8005-70d2-489a-ba09-78e49f7afd95");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Raw Weights", "RW", "Any number representing importance of criterias", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Normalised Criteria Weights", "NorWs", "Normalised Criteria Weights", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> rawWeights = new List<double>();
            if (!DA.GetDataList(0, rawWeights)) return;

            List<double> normalised = GeneralMethods.SimpleNormalisedWeights(rawWeights);

            DA.SetDataList(0, normalised);
        }
    }
}