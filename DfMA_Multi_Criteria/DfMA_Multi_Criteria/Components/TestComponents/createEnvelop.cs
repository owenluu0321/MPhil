using DfMA_Multi_Criteria.GeometryFunction;
using Eto.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Components.TestComponents
{
    public class EnvToFloorSurface : GH_Component
    {

        public EnvToFloorSurface()
          : base("TestComp", "EnvFlo",
            "Create envelop using geometry",
            "Phoenix", "InputComponents")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("d8bf5fd9-b39b-47f2-ae31-6f22f75ed31f");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Envelope", "Env", "Envelop to create floors", GH_ParamAccess.item);
            pManager.AddNumberParameter("F2F_Height", "HF2F", "Floor to Floor Height", GH_ParamAccess.item);
            //option to have different f2f heights
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Floor Surfaces", "F", "Surfaces of each Floor", GH_ParamAccess.list);
            pManager.AddNumberParameter("Area", "A", "Floor Area", GH_ParamAccess.list);

            // Sometimes you want to hide a specific parameter from the Rhino preview.
            // You can use the HideParameter() method as a quick way:
            //pManager.HideParameter(0);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep envelop = new Brep();
            double levelHeight = 0;
            if (!DA.GetData(0, ref envelop)) return;
            if (!DA.GetData(1, ref levelHeight)) return;

            List<Brep> output = GeoFunction.EnvToFloors(envelop, levelHeight);
            DA.SetDataList(0, output);
            List<double> surfaceArea = new List<double>();
            foreach (Brep brep in output) { surfaceArea.Add(brep.GetArea()); }
            DA.SetDataList(1, surfaceArea);
        }
    }

}