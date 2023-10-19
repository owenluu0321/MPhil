using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Phoenix.Components
{
    public class ProcurementScore : GH_Component
    {

        public ProcurementScore()
          : base("Procurement Score", "Proc",
            "Calculate MC Procurement criteria score upon given input",
            "Phoenix", "Scores")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("f6927121-506b-4e5e-83e5-faadb858f1d7");

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
            pManager.AddNumberParameter("A", "A", "total plan area of roof, floor or cladding supported by steelwork (where there are multiple levels each level should be summed)", GH_ParamAccess.item);

            pManager.AddNumberParameter("Wm_std_c", "Wm_std_c", "number of weeks for quality assurance and procurement of standard locally procured closed sections, measured as time from start of the ordering process to receival of steelwork in fabricators workshop ", GH_ParamAccess.item);
            pManager[9].Optional = true;
            pManager.AddNumberParameter("Wm_std_o", "Wm_std_o", "number of weeks for quality assurance and procurement of standard locally procured open sections, measured as time from start of the ordering process to receival of steelwork in fabricators workshop ", GH_ParamAccess.item);
            pManager[10].Optional = true;
            pManager.AddNumberParameter("Wm_cus", "Wm_cus", "number of weeks for quality assurance and procurement of custom locally procured fabricated sections, measured as time from start of the ordering process to receival of steelwork in fabricators workshop ", GH_ParamAccess.item);
            pManager[11].Optional = true;
            pManager.AddNumberParameter("Wm_na_aus", "Wm_na_aus", "number of weeks for quality assurance and procurement of sections not locally available, measured as time from start of the ordering process to receival of steelwork in fabricators workshop", GH_ParamAccess.item);
            pManager[12].Optional = true;
            pManager.AddNumberParameter("Wp_std", "Wp_std", "number of weeks for quality assurance and procurement of standard plates, measured as time from start of the ordering process to receival of steelwork in fabricators workshop", GH_ParamAccess.item);
            pManager[13].Optional = true;
            pManager.AddNumberParameter("Wp_na_aus", "Wp_na_aus", "number of weeks for quality assurance and procurement of plate thicknesses not locally available, measured as time from start of the ordering process to receival of steelwork in fabricators workshop", GH_ParamAccess.item);
            pManager[14].Optional = true;
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Procurement Score", "PS", "MC Procurement Criteria Score", GH_ParamAccess.item);
            pManager.AddNumberParameter("Mass Per Square Meter", "MPSM", "Mass Per Square Meter", GH_ParamAccess.item);
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
            double Wmstd_c = default;
            double Wmstd_o = default;
            double Wmcus = default;
            double Wmna_aus = default;
            double Wp_std = default;
            double Wpna_aus = default;
            double M = default;
            double A = default;

            if (!DA.GetData(0, ref Pstd_c)) return;
            if (!DA.GetData(1, ref Pstd_o)) return;
            if (!DA.GetData(2, ref Pcus)) return;
            if (!DA.GetData(3, ref Pna_aus)) return;
            if (!DA.GetData(4, ref Pp_std)) return;
            if (!DA.GetData(5, ref Ppna_aus)) return;
            if (!DA.GetData(6, ref Ppm)) return;
            if (!DA.GetData(7, ref M)) return;
            if (!DA.GetData(8, ref A)) return;
            
            if (!DA.GetData(9, ref Wmstd_c)) Wmstd_c = 4;
            if (!DA.GetData(10, ref Wmstd_o)) Wmstd_o = 3;
            if (!DA.GetData(11, ref Wmcus)) Wmcus = 8;
            if (!DA.GetData(12, ref Wmna_aus)) Wmna_aus = 12;
            if (!DA.GetData(13, ref Wp_std)) Wp_std = 2;
            if (!DA.GetData(14, ref Wpna_aus)) Wpna_aus = 6;
            

            double output = CriteriaScoreCalculation.ProcurementScore(Pstd_c, Pstd_o, Pcus, Pna_aus, Pp_std, Ppna_aus, Ppm, Wmstd_c, Wmstd_o, Wmcus, Wmna_aus, Wp_std, Wpna_aus);
            double outputB = CriteriaScoreCalculation.MassPerSquareMeter(M, A);

            DA.SetData(0, output);
            DA.SetData(1, outputB);
        }
    }
}