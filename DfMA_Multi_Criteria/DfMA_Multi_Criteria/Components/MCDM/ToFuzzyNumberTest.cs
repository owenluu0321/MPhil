using Phoenix.MCDM_Functions;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace Phoenix.Components
{
    public class ToFuzzyNumber : GH_Component
    {

        public ToFuzzyNumber()
          : base("FuzzyFuzzy", "fuz",
            "Translate a crisp value to fuzzy number set",
            "Phoenix", "MCDM")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("a47a18b5-55eb-4e04-b67a-1c78b1398e07");

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Number", "CN", "A crisp number to be translated", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Fuzzy Number One", "FuzNumber1", "Fuzzy1", GH_ParamAccess.list);
            pManager.AddNumberParameter("Fuzzy Number Two", "FuzNumber2", "Fuzzy2", GH_ParamAccess.list);
            pManager.AddNumberParameter("Fuzzy Number Three", "FuzNumber3", "Fuzzy3", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<GH_Number> number = new List<GH_Number>();
            List<double> dou = new List<double>();
            List<double> re = new List<double>();
            List<double> mi = new List<double>();
            if (!DA.GetDataList(0, number)) return;
            foreach (GH_Number Item in number)
            {
                Tuple<double, double, double> fuzzy = Fuzzy.ToFuzzyNumber(Item.Value);
                dou.Add(fuzzy.Item1);
                re.Add(fuzzy.Item2);
                mi.Add(fuzzy.Item3);    
            }

            DA.SetDataList(0, dou);
            DA.SetDataList(1, re);
            DA.SetDataList(2, mi);
        }
    }
}