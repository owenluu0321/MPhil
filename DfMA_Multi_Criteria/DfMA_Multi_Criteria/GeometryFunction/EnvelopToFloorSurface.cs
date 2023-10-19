using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DfMA_Multi_Criteria.GeometryFunction
{
    public class GeoFunction
    {
        public static List<Brep> EnvToFloors(Brep inputBrep, double F2F)
        {
            BoundingBox envelopBox = inputBrep.GetBoundingBox(true);
            Point3d maxControlPoints = envelopBox.Corner(false, false, false);
            Point3d minControlPoints = envelopBox.Corner(true, true, true);
            double levelNumber = Math.Floor((maxControlPoints.Z - minControlPoints.Z) / F2F) + 1;

            List<double> floorHeights = new List<double>();
            List<Plane> floorPlanes = new List<Plane>();
            List<Brep> floorplanSurfaces = new List<Brep>();
            for (int i = 0; i < levelNumber; i++)
            {
                double height = minControlPoints.Z + i * F2F;
                floorHeights.Add(height);
                Plane plane = new Plane(new Point3d(0, 0, height), new Vector3d(0, 0, 1));
                floorPlanes.Add(plane);
                Curve[] curveOut = null;
                Point3d[] pointOut = null;
                Rhino.Geometry.Intersect.Intersection.BrepPlane(inputBrep, plane, 0.01, out curveOut, out pointOut);
                curveOut.ToList().AsEnumerable();
                Brep[] BoundarySurface = Brep.CreatePlanarBreps(curveOut, 0.01);
                BoundarySurface.ToList();
                foreach (Brep surface in BoundarySurface)
                    floorplanSurfaces.Add(surface);
            }
            return floorplanSurfaces;
        }
    }
}
