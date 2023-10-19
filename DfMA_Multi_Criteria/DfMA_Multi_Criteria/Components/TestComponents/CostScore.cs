using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Phoenix.Components
{
    public class CostScore : GH_Component
    {

        public CostScore()
          : base("Cost Score", "C",
            "Calculate MC Costcriteria score upon given input",
            "Phoenix", "Scores")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("38732eac-e26c-4955-894c-ad6bab892885");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Pstd_c", "Pstd_c", "proportion of total member mass that are standard closed sections that are readily available in Australia (i.e. Circular Hollow Sections (48CHS-457CHS), Square Hollow Sections (50SHS-250SHS), Rectangular Hollow Sections (50x20RHS to 300x200RHS) ", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pstd_o", "Pstd_o", "proportion of total member mass that are open sections readily available in Australia (i.e. Universal Beams (150UB-610UB), Universal Columns (100UC-310UC), Welded Beams (700WB-1200WB), Welded Columns (350WC-500WC), Parallel Flange Channels (75PFC-380PFC), Equal Angles (45EA-200EA), Unequal Angles (65x50UA-150x100UA) ", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pcus", "Pcus", "proportion of total member mass that are custom fabricated sections or sections with non-standard grades (excluding standard welded beam sections i.e. 700WB-1200WB)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pna_aus", "Pna_aus", "proportion of total member mass that are sections not available in Australia or difficult to procure locally. ", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pp_std", "Pp_std", "proportion of total connection mass that are thicknesses that are readily available locally in Australia (typically grades 250, 300 or 350 for plates) ", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ppna_aus", "Ppna_aus", "proportion of total connection mass that are plates with thicknesses that are not readily available locally in Australia ", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ppm", "Ppm", "proportion of connection plates against members, proportion measured as a ratio of mass. ", GH_ParamAccess.item);
            pManager.AddNumberParameter("M", "M", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("FPS", "FPS", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("TS", "TS", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("SCT", "SCT", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("TRANS", "TRANS", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("TOSLT", "TOSLT", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ncranes", "Ncrane", "Average number of cranes on-site that will operate in parallel during the entire construction phase", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Simp Cost Score", "S_Cost", "MC Procurement Criteria Score", GH_ParamAccess.item);
            pManager.AddNumberParameter("Detailed Cost Score", "D_Cost", "Mass Per Square Meter", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double Pstd_c = default;
            double Pstd_o = default;
            double Pcus = default;
            double Pna_aus = default;
            double Pp_std = default;
            double Ppna_aus = default;
            double Ppm = default;
            double M = default;
            double FPS = default;
            double TS = default;
            double SCT = default;
            double Trans = default;
            double TOSLT = default;
            double Ncrane = default;

            if (!DA.GetData(0, ref Pstd_c)) return;
            if (!DA.GetData(1, ref Pstd_o)) return;
            if (!DA.GetData(2, ref Pcus)) return;
            if (!DA.GetData(3, ref Pna_aus)) return;
            if (!DA.GetData(4, ref Pp_std)) return;
            if (!DA.GetData(5, ref Ppna_aus)) return;
            if (!DA.GetData(6, ref Ppm)) return;
            if (!DA.GetData(7, ref M)) return;
            if (!DA.GetData(8, ref FPS)) return;
            if (!DA.GetData(9, ref TS)) return;
            if (!DA.GetData(10, ref SCT)) return;
            if (!DA.GetData(11, ref Trans)) return;
            if (!DA.GetData(12, ref TOSLT)) return;
            if (!DA.GetData(13, ref Ncrane)) return;

            double output = CriteriaScoreCalculation.SimplifiedCost(Ncrane, FPS, Pna_aus, SCT);
            double outputB = CriteriaScoreCalculation.DetailedCost(FPS, TS, Ncrane, SCT, Trans, M, TOSLT, Pcus, Ppna_aus, Pna_aus,
                Pstd_c, Pp_std, Ppm, Pstd_o);

            DA.SetData(0, output);
            DA.SetData(1, outputB);
        }
    }
}