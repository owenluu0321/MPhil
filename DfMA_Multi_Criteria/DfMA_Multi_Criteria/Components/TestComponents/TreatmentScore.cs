using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Phoenix.Components
{
    public class TreatmentScore : GH_Component
    {

        public TreatmentScore()
          : base("Treatment Score", "Tre",
            "Calculate MC Treatment criteria score upon given input",
            "Phoenix", "Scores")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("831b579f-7e9d-40c6-b83e-29e5c24bd1c0");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Asc", "Asc", "surface area of steel where a single coat paint  system is to be applied", GH_ParamAccess.item);
            pManager.AddNumberParameter("Atc", "Atc", "surface area of steel where a triple coat paint system is to be applied.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Ahdg", "Ahdg", "surface area of steel where a Hot-Dipped Galvanised (HDG) treatment system is to be applied.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Afire", "Afire", "surface area of steel where fire treatment system has been applied (i.e. vermiculite or intumescent paint)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Aut", "Aut", "surface area of steel that is untreated", GH_ParamAccess.item);
            pManager.AddNumberParameter("M", "M", "total mass if the steel structure in tonnes", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wptsc", "Wptsc", "cost rate for a single coat paint system of steel, (typically a single application of a primer or a finish coat)", GH_ParamAccess.item);
            pManager[6].Optional = true;
            pManager.AddNumberParameter("Wpttc", "Wpttc", "cost rate for a triple coat paint system of steel, (typically involving a primer, an intermediate coat, and a finish or topcoat)", GH_ParamAccess.item);
            pManager[7].Optional = true;
            pManager.AddNumberParameter("Wpthdg", "Wpthdg", "cost rate for a Hot-Dipped Galvanised (HDG) protective system, (costs would consider the price of zinc, preparation of the steel, the galvanizing process, and post-galvanizing treatments if any)", GH_ParamAccess.item);
            pManager[8].Optional = true;
            pManager.AddNumberParameter("Wptfire", "Wptfire", "cost rate of vermiculite firespray or intumescent paint (excludes painted or HDG treatment of steel which would also be required)", GH_ParamAccess.item);
            pManager[9].Optional = true;
            pManager.AddNumberParameter("Wptut", "Wptut", "cost rate for untreated steel preparation, may involve dry blasting and cleaning if required.", GH_ParamAccess.item);
            pManager[10].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Treatment Score", "TS", "MC Treatment Criteria Score", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double Asc = default;
            double Atc = default;
            double Ahdg = default;
            double Afire = default;
            double Aut = default;
            double M = default;
            double Wptsc = default;
            double Wpttc = default;
            double Wpthdg = default;
            double Wptfire = default;
            double Wptut = default;


            if (!DA.GetData(0, ref Asc)) return;
            if (!DA.GetData(1, ref Atc)) return;
            if (!DA.GetData(2, ref Ahdg)) return;
            if (!DA.GetData(3, ref Afire)) return;
            if (!DA.GetData(4, ref Aut)) return;
            if (!DA.GetData(5, ref M)) return;
            if (!DA.GetData(6, ref Wptsc)) Wptsc = 40;
            if (!DA.GetData(7, ref Wpttc)) Wpttc = 80;
            if (!DA.GetData(8, ref Wpthdg)) Wpthdg = 50;
            if (!DA.GetData(9, ref Wptfire)) Wptfire = 100;
            if (!DA.GetData(10, ref Wptut)) Wptut = 10;


            double output = CriteriaScoreCalculation.TreatmentScore(Asc, Atc, Ahdg, Afire, Aut, M, Wptsc, Wpttc, Wpthdg, Wptfire, Wptut);

            DA.SetData(0, output);
        }
    }
}