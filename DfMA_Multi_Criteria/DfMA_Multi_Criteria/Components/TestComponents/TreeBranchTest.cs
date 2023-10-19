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
using Phoenix.MCDM_Functions.ExtensionMethods;
using Grasshopper;

namespace Phoenix.Components.TestComponents
{
    public class TestComponent : GH_Component
    {

        public TestComponent()
          : base("Test", "AHPTOPSIS",
            "Compute the rank of given alternatives by AHP TOPSIS method",
            "Phoenix", "MCDM")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("99460723-7430-4fa5-923c-8ea805d69696");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Benificial Criteria Index", "BInd", "Beneficial Criteria Index", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Tree", "Sc", "Performance Socre for ranking", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double index = default;
            if (!DA.GetData(0, ref index)) return;


            DataTree<double> output = new DataTree<double>();
            for (int i = 0; i < 5; i++)
            {
                GH_Path level1 = new GH_Path(i);
                for (int j = 0; j < 10; j++)
                {
                    GH_Path level2 = level1.AppendElement(j);
                    output.Add(j, level2);
                }
            }
            DA.SetDataTree(0, output);
        }
    }
}