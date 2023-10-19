using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Phoenix.Components
{
    public class FabProcessingScore : GH_Component
    {

        public FabProcessingScore()
          : base("Fabrication & Processing Score", "fab&Proc",
            "Calculate MC Fabrication and Processing criteria score upon given input",
            "Phoenix", "Scores")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("3e1efa0b-eb44-4550-aeb4-ef40e41b2fb2");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Cnt", "Cnt", "Number of connections total (cranks and splices count as connections)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cn1", "Cn1", "Number of type1 connections (single pass fillet welds and simple connections)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cn2", "Cn2", "Number of type2 connections (FPBW connections or bolted moment splice connection)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cn3", "Cn3", "Number of type3 connections (Cruciform moment connections, other complex connections)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cn4", "Cn4", "Number of type4 connections (connections with welds that are 10+ pass welds)", GH_ParamAccess.item);
            pManager.AddNumberParameter("FABeff", "FABeff", "Percentage saving in fabrication hours due to cuts and holes that could be processed via beam-line or similar" +
                "automated fabrication processes)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cnr", "Cnr", "Repetition-number of unique connections total (Connections that would require different plates, hole arrangements" +
                "weld preparation, welds etc.) measure as an average time saving in hours/tonne", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ppc", "Ppc", "Proportion of members requiring precamber or a curved profile", GH_ParamAccess.item);
            pManager.AddNumberParameter("Mp", "Mp", "member, plate prep specifically for treatment measured in hours/tonne", GH_ParamAccess.item);
            pManager.AddNumberParameter("M", "M", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wfp1", "Wfp1", "Approximate time to fabricate connection type 1 (hours per conenction))", GH_ParamAccess.item);
            pManager[10].Optional = true;
            pManager.AddNumberParameter("Wfp2", "Wfp2", "Approximate time to fabricate connection type 2 (hours per conenction))", GH_ParamAccess.item);
            pManager[11].Optional = true;
            pManager.AddNumberParameter("Wfp3", "Wfp3", "Approximate time to fabricate connection type 3 (hours per conenction))", GH_ParamAccess.item);
            pManager[12].Optional = true;
            pManager.AddNumberParameter("Wfp4", "Wfp4", "Approximate time to fabricate connection type 4 (hours per conenction))", GH_ParamAccess.item);
            pManager[13].Optional = true;
            pManager.AddNumberParameter("Wfppc", "Wfppc", "Approximate time in hours to precamber or curve each member including preparation, heating and bending", GH_ParamAccess.item);
            pManager[14].Optional = true;
            pManager.AddNumberParameter("Wfpr", "Wfpr", "Approximate hours saving per connection due to repetition", GH_ParamAccess.item);
            pManager[15].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Fabrication and Processing Score", "FPS", "MC Fabrication and Processing Criteria Score", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double CNt = default;
            double CN1 = default;
            double CN2 = default;
            double CN3 = default;
            double CN4 = default;
            double FABeff = default;
            double CNr= default;
            double Ppc = default;
            double Mp = default;
            double M = default;
            double Wfp1 = default;
            double Wfp2 = default;
            double Wfp3 = default;
            double Wfp4 = default;
            double Wfp_pc = default;
            double Wfpr = default;
            
            if (!DA.GetData(0, ref CNt)) return;
            if (!DA.GetData(1, ref CN1)) return;
            if (!DA.GetData(2, ref CN2)) return;
            if (!DA.GetData(3, ref CN3)) return;
            if (!DA.GetData(4, ref CN4)) return;
            if (!DA.GetData(5, ref FABeff)) return;
            if (!DA.GetData(6, ref CNr)) return;
            if (!DA.GetData(7, ref Ppc)) return;
            if (!DA.GetData(8, ref Mp)) return;
            if (!DA.GetData(9, ref M)) return;
            
            if (!DA.GetData(10, ref Wfp1)) Wfp1 =2;
            if (!DA.GetData(11, ref Wfp2)) Wfp2 = 4;
            if (!DA.GetData(12, ref Wfp3)) Wfp3 = 5;
            if (!DA.GetData(13, ref Wfp4)) Wfp4 = 6;
            if (!DA.GetData(14, ref Wfp_pc)) Wfp_pc = 4;
            if (!DA.GetData(15, ref Wfpr)) Wfpr = 0.5;

            double output = CriteriaScoreCalculation.FabricationAndProcessingScore(CNt, CN1, CN2, CN3, CN4, FABeff, CNr, Ppc, Mp, M, Wfp1, Wfp2, Wfp3, Wfp4, Wfp_pc, Wfpr);

            DA.SetData(0, output);
        }
    }
}