using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Phoenix.Components
{
    public class SiteTimeScore : GH_Component
    {

        public SiteTimeScore()
          : base("Site Construction Time Score", "SiConTi",
            "Calculate MC Site Construction Time criteria score upon given input",
            "Phoenix", "Scores")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("0c3f11ee-395b-4cec-95cc-958020f6983c");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Nlifts", "Nlifts", "number of on-site crane lifts total", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ncranes", "Ncrane", "Average number of cranes on-site that will operate in parallel during the entire construction phase", GH_ParamAccess.item);
            pManager.AddNumberParameter("Tlift", "Tlift", "average time for rigging and lifting of each member/panel", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cna", "Cna", "number of type A on-site connections - bolts on 1 plane i.e. all bolts on web or all bolts on lower flange", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cnb", "Cnb", "number of type B on-site connections - bolts on multiple planes", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cnc", "Cnc", "number of type C on-site connections - welding", GH_ParamAccess.item);
            pManager.AddNumberParameter("Npwf", "Npwf", "average number of parallel work fronts across the entire on-site construction phase", GH_ParamAccess.item);
            pManager.AddNumberParameter("Tdelay", "Tdelay", "delays due to external factors, weather delays, site accessibility, safety incidences/equipment issues, regulatory (union) delays, public holidays, permits and approval delays represented as a % estimate of overall construction time. ", GH_ParamAccess.item);
            pManager.AddNumberParameter("Tcont", "Tcont", "contingency (buffer and rework) represented as a % estimate of overall construction time.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Tsite_allow", "Tsite_allow", "average construction hours permitted and anticipated to be used each week (excluded breaks) ", GH_ParamAccess.item);
            pManager.AddNumberParameter("M", "M", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("Nppl", "Nppl", "Avg. number of people on-site contributing to the installation of the steelwork throughoutall of construction", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wcna", "Wcna", "average time to complete connection type A on-site including mobilisation, demobilisation, and quality assurance.", GH_ParamAccess.item);
            pManager[12].Optional = true;
            pManager.AddNumberParameter("Wcnb", "Wcnb", "average time to complete connection type B on-site including mobilisation, demobilisation, and quality assurance.", GH_ParamAccess.item);
            pManager[13].Optional = true;
            pManager.AddNumberParameter("Wcnc", "Wcnc", "average time to complete connection type c on-site including mobilisation, demobilisation, and quality assurance.", GH_ParamAccess.item);
            pManager[14].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Site Construction Time Score", "SCT", "MC Site Construction Time Criteria Score", GH_ParamAccess.item);
            pManager.AddNumberParameter("Total Onsite Labour Time", "TOSLT", "MC Site Construction Time Criteria Score", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double Nlifts = default;
            double Ncranes = default;
            double Tlift = default;
            double Cna = default;
            double Cnb = default;
            double Cnc = default;
            double Npwf = default;
            double Tdelay = default;
            double Tcont = default;
            double Tsite_allow = default;
            double M = default;
            double Nppl = default;
            double Wcna = default;
            double Wcnb = default;
            double Wcnc = default;


            if (!DA.GetData(0, ref Nlifts)) return;
            if (!DA.GetData(1, ref Ncranes)) return;
            if (!DA.GetData(2, ref Tlift)) return;
            if (!DA.GetData(3, ref Cna)) return;
            if (!DA.GetData(4, ref Cnb)) return;
            if (!DA.GetData(5, ref Cnc)) return;
            if (!DA.GetData(6, ref Npwf)) return;
            if (!DA.GetData(7, ref Tdelay)) return;
            if (!DA.GetData(8, ref Tcont)) return;
            if (!DA.GetData(9, ref Tsite_allow)) return;
            if (!DA.GetData(10, ref M)) return;
            if (!DA.GetData(11, ref Nppl)) return;
            if (!DA.GetData(12, ref Wcna)) Wcna = 0.3;
            if (!DA.GetData(13, ref Wcnb)) Wcnb = 0.4;
            if (!DA.GetData(14, ref Wcnc)) Wcnc = 1;


            double output = CriteriaScoreCalculation.SiteConstructionTimeScore(M, Tsite_allow, Tdelay, Tcont, Tlift, Nlifts, Ncranes, Npwf, Cna, Cnb, Cnc, Wcna, Wcnb, Wcnc);
            double outputB = CriteriaScoreCalculation.TotalOnsiteLabourTime(M, Nppl, Tdelay, Tcont, Tlift, Nlifts, Cna, Cnb, Cnc, Wcna, Wcnb, Wcnc);

            DA.SetData(0, output);
            DA.SetData(1, outputB);
        }
    }
}