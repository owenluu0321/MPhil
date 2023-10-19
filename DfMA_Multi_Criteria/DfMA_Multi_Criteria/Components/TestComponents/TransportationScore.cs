using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Phoenix.Components
{
    public class TransportScore : GH_Component
    {

        public TransportScore()
          : base("Transportation Score", "TRAS",
            "Calculate MC Transportation criteria score upon given input",
            "Phoenix", "Scores")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("14bbcb38-9bed-4c32-a6c9-61ddadc06cf7");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Mtruck", "Mtruck", "average tonnage per truck movement (load efficiency)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ttruck", "Ttruck", "average time required to load and unload (load time efficiency), excludes transportation time and lifts off the truck trailers which are directly used for installation", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pstd_tr", "Pstd_tr", "proportion of total steel tonnage that fits on a standard trailer (i.e. 2.5 wide x 3.5m high x 12m long) i.e. no escort required", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pesc_tr", "Pesc_tr", "proportion of total steel tonnage that overhangs a standard trailer (less than 3.5 wide x 3.5m high x 16m long) i.e. escort required but no police escort required", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ppol_tr", "Ppol_tr", "proportion of total steel tonnage that overhangs a standard trailer (greater than 3.5 wide x 3.5m high x 16m long) i.e. police escort required", GH_ParamAccess.item);
            pManager.AddNumberParameter("Dtruck", "Dtruck", "average distance transported via truck (averaged for 1 truck load) including transportation from supplier to fabricator, fabricator to painter, painter to site", GH_ParamAccess.item);
            pManager.AddNumberParameter("M", "M", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wpstd_tr", "Wpstd_tr", "constant for transport of a standard truck (no penalty)", GH_ParamAccess.item);
            pManager[7].Optional = true;
            pManager.AddNumberParameter("Wpesc_tr", "Wpesc_tr", "penalty associated with having a single or dual standard vehicle escort", GH_ParamAccess.item);
            pManager[8].Optional = true;
            pManager.AddNumberParameter("Wppol_tr", "Wppol_tr", "penalty associated with having a police escort", GH_ParamAccess.item);
            pManager[9].Optional = true;
            pManager.AddNumberParameter("Wttruck", "Wttruck", "equivalent distance per minute of standing time", GH_ParamAccess.item);
            pManager[10].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Transportation Score", "TRAS", "MC Transportation Criteria Score", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double Mtruck = default;
            double Ttruck = default;
            double Pstd_tr = default;
            double Pesc_tr = default;
            double Ppol_tr = default;
            double Dtruck = default;
            double M = default;
            double Wpstd_tr = default;
            double Wpesc_tr = default;
            double Wpescpol_tr = default;
            double Wttruck = default;


            if (!DA.GetData(0, ref Mtruck)) return;
            if (!DA.GetData(1, ref Ttruck)) return;
            if (!DA.GetData(2, ref Pstd_tr)) return;
            if (!DA.GetData(3, ref Pesc_tr)) return;
            if (!DA.GetData(4, ref Ppol_tr)) return;
            if (!DA.GetData(5, ref Dtruck)) return;
            if (!DA.GetData(6, ref M)) return;
            if (!DA.GetData(7, ref Wpstd_tr)) Wpstd_tr = 1;
            if (!DA.GetData(8, ref Wpesc_tr)) Wpesc_tr = 1.5;
            if (!DA.GetData(9, ref Wpescpol_tr)) Wpescpol_tr = 2.5;
            if (!DA.GetData(10, ref Wttruck)) Wttruck = 2;


            double output = CriteriaScoreCalculation.TransportationScore(Mtruck, Ttruck, Pstd_tr, Pesc_tr, Ppol_tr, Dtruck, M, Wpstd_tr, Wpesc_tr, Wpescpol_tr, Wttruck);
            DA.SetData(0, output);
        }
    }
}