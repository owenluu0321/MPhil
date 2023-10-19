using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;

namespace Phoenix.Components
{
    public class SustainabilityScore : GH_Component
    {

        public SustainabilityScore()
          : base("Sustainability Score", "SC",
            "Calculate MC sustainability criteria score upon given input",
            "Phoenix", "Scores")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("324810a8-3529-4d4d-a329-83c561d50df7");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("TRANS", "TRANS", "transportation score", GH_ParamAccess.item);
            pManager.AddNumberParameter("w_dc", "w_dc", "surface area of steel where a triple coat paint system is to be applied.", GH_ParamAccess.item);
            pManager.AddNumberParameter("w_rec", "w_rec", "surface area of steel where a Hot-Dipped Galvanised (HDG) treatment system is to be applied.", GH_ParamAccess.item);
            pManager.AddNumberParameter("w_reu", "w_reu", "surface area of steel where fire treatment system has been applied (i.e. vermiculite or intumescent paint)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pwaste", "Pwaste", "surface area of steel that is untreated", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pgs", "Pgs", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("M", "M", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pdfd", "Pdfd", "cost rate for a single coat paint system of steel, (typically a single application of a primer or a finish coat)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pstd_c", "Pstd_c", "proportion of total member mass that are standard closed sections that are readily available in Australia (i.e. Circular Hollow Sections (48CHS-457CHS), Square Hollow Sections (50SHS-250SHS), Rectangular Hollow Sections (50x20RHS to 300x200RHS) ", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pstd_o", "Pstd_o", "proportion of total member mass that are open sections readily available in Australia (i.e. Universal Beams (150UB-610UB), Universal Columns (100UC-310UC), Welded Beams (700WB-1200WB), Welded Columns (350WC-500WC), Parallel Flange Channels (75PFC-380PFC), Equal Angles (45EA-200EA), Unequal Angles (65x50UA-150x100UA) ", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pcus", "Pcus", "proportion of total member mass that are custom fabricated sections or sections with non-standard grades (excluding standard welded beam sections i.e. 700WB-1200WB)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pna_aus", "Ppna_aus", "proportion of total connection mass that are plates with thicknesses that are not readily available locally in Australia ", GH_ParamAccess.item);
            pManager.AddNumberParameter("Pp_std", "Pp_std", "proportion of total connection mass that are thicknesses that are readily available locally in Australia (typically grades 250, 300 or 350 for plates) ", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ppm", "Ppm", "proportion of connection plates against members, proportion measured as a ratio of mass. ", GH_ParamAccess.item);
            /*
            pManager.AddNumberParameter("Wco2_stdc", "Wco2_stdc", "cost rate for a triple coat paint system of steel, (typically involving a primer, an intermediate coat, and a finish or topcoat)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wco2_stdo", "Wco2_stdo", "cost rate for a Hot-Dipped Galvanised (HDG) protective system, (costs would consider the price of zinc, preparation of the steel, the galvanizing process, and post-galvanizing treatments if any)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wco2_cus", "Wco2-cus", "cost rate of vermiculite firespray or intumescent paint (excludes painted or HDG treatment of steel which would also be required)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wco2_na_aus", "Wco2_na_aus", "cost rate for untreated steel preparation, may involve dry blasting and cleaning if required.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wco2_pstd", "Wco2_pstd", "cost rate for untreated steel preparation, may involve dry blasting and cleaning if required.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wco2_pna_aus", "Wco2_pna_aus", "cost rate for untreated steel preparation, may involve dry blasting and cleaning if required.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wco2_gs", "Wco2_gs", "cost rate for untreated steel preparation, may involve dry blasting and cleaning if required.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wco2_tra", "Wco2_tra", "cost rate for untreated steel preparation, may involve dry blasting and cleaning if required.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wdfd", "Wdfd", "cost rate for untreated steel preparation, may involve dry blasting and cleaning if required.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wl_dfd", "Wl_dfd", "cost rate for untreated steel preparation, may involve dry blasting and cleaning if required.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wrec", "Wrec", "cost rate for untreated steel preparation, may involve dry blasting and cleaning if required.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wdreu", "Wreu", "cost rate for untreated steel preparation, may involve dry blasting and cleaning if required.", GH_ParamAccess.item);
            */
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Sustainability Score", "Sc", "MC Sustainability Criteria Score", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double Trans = default;
            double w_dc = default;
            double w_rec = default;
            double w_reu = default;
            double Pwaste = default;
            double Pgs = default;
            double Pdfd = default;
            double M = default;
            double Pstd_c = default;
            double Pstd_o = default;
            double Pcus = default;
            double Pna_aus = default;
            double Pp_std = default;
            double Ppm = default;

            /*
            double Wco2_stdc = default;
            double Wco2_stdo = default;
            double Wco2_cus = default;
            double Wco2_na_aus = default;
            double Wco2_pstd = default;
            double Wco2_pna_aus = default;
            double Wco2_gs = default;
            double Wco2_tra = default;
            double Wdfd = default;
            double Wl_dfd = default;
            double Wrec = default;
            double Wreu = default;
            */


            if (!DA.GetData(0, ref Trans)) return;
            if (!DA.GetData(1, ref w_dc)) return;
            if (!DA.GetData(2, ref w_rec)) return;
            if (!DA.GetData(3, ref w_reu)) return;
            if (!DA.GetData(4, ref Pwaste)) return;
            if (!DA.GetData(5, ref Pgs)) return;
            if (!DA.GetData(6, ref M)) return;
            if (!DA.GetData(7, ref Pdfd)) return;
            if (!DA.GetData(8, ref Pstd_c)) return;
            if (!DA.GetData(9, ref Pstd_o)) return;
            if (!DA.GetData(10, ref Pcus)) return;
            if (!DA.GetData(11, ref Pna_aus)) return;
            if (!DA.GetData(12, ref Pp_std)) return;
            if (!DA.GetData(13, ref Ppm)) return;
            /*
            if (!DA.GetData(12, ref Wco2_stdc)) return;
            if (!DA.GetData(13, ref Wco2_stdo)) return;
            if (!DA.GetData(14, ref Wco2_cus)) return;
            if (!DA.GetData(15, ref Wco2_na_aus)) return;
            if (!DA.GetData(16, ref Wco2_pstd)) return;
            if (!DA.GetData(17, ref Wco2_pna_aus)) return;
            if (!DA.GetData(18, ref Wco2_gs)) return;
            if (!DA.GetData(19, ref Wco2_tra)) return;
            if (!DA.GetData(20, ref Wdfd)) return;
            if (!DA.GetData(21, ref Wl_dfd)) return;
            if (!DA.GetData(22, ref Wrec)) return;
            if (!DA.GetData(23, ref Wreu)) return;
            */


            double output = CriteriaScoreCalculation.SustainabilityScore(Trans, w_dc, w_rec, w_reu, Pwaste, Pgs, Pdfd, M, Pstd_c, Pstd_o, Pcus, Pna_aus, Pp_std, Ppm);
            double complexPart = (1700 * Pstd_c) + (1600 * Pstd_o) + (1800 * Pcus) + (1800 * Pna_aus) + (1100 * Pgs);
            complexPart = complexPart * (1 - Ppm);
            complexPart = complexPart + (1700 * Ppm * Pp_std) - (1100 * 0.2 * Pdfd);
            complexPart = complexPart * (1 + Pwaste);

            double Front = (Trans * 0.8) + (1 / M) * ((2500 * w_dc - w_reu) + (1500 * w_rec));
            DA.SetData(0, output);
        }
    }
}