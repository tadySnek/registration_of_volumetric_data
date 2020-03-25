﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class FeatureComputer : IFeatureComputer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="d"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static List<Point3D> GetSphere(Point3D x, double r)
        {
            return GetSphere(x, r, 0.5);
        }

        /// <summary>
        /// Overload with step as a parameter
        /// </summary>
        /// <param name="x"></param>
        /// <param name="d"></param>
        /// <param name="r"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static List<Point3D> GetSphere(Point3D x, double r, double step)
        {
            List<Point3D> points = new List<Point3D>();

            double rSquared = r * r;

            Point3D A = new Point3D(x.X - r, x.Y + r, x.Z - r);
            Point3D C = new Point3D(x.X + r, x.Y - r, x.Z - r);
            Point3D D = new Point3D(x.X - r, x.Y - r, x.Z - r);
            Point3D H = new Point3D(x.X - r, x.Y - r, x.Z + r);

            for (double i = D.X; i <= C.X; i += step)
            {
                for (double j = D.Y; j <= A.Y; j += step)
                {
                    for (double k = D.Z; k <= H.Z; k += step)
                    {
                        double d2 = (i - x.X) * (i - x.X) + (j - x.Y) * (j - x.Y) + (k - x.Z) * (k - x.Z);
                        if (d2 <= rSquared)
                        {
                            points.Add(new Point3D(i, j, k));
                        }
                    }
                }
            }
            return points;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public FeatureVector ComputeFeatureVector(IData d, Point3D p)
        {
            double[] fv = new double[5];
            double norm = 0;
            double nConst = (d.XSpacing + d.YSpacing + d.ZSpacing)/3;
            double s = nConst; //nconst /2

            int i = 0;
            for (double r = 0.5; r <= 2.5; r += 0.5)
            {
                double sum = 0;
                List<Point3D> points = GetSphere(p, r, s); // r change nconst!
                
                foreach (Point3D point in points)
                {
                    sum += d.GetValue(point); //real coordinates
                }
                double avg = sum / points.Count;
                norm += avg*avg;
                fv[i] = avg;
                i++;
            }

            norm = Math.Sqrt(norm);
            if(norm == 0)
            {
                norm = 1;
            }
            return new FeatureVector(p, fv[0] / norm, fv[1] / norm, fv[2] / norm, fv[3] / norm, fv[4] / norm);
            //return new FeatureVector(p, fv[0], fv[1], fv[2], fv[3], fv[4]);
        }

        public FeatureVector ComputeFeatureVector2(IData d, Point3D p)
        {
            double[] fv = new double[5];
            double norm = 0;
            double nConst = (d.XSpacing + d.YSpacing + d.ZSpacing) / 3;
            double s = nConst; //nconst /2

            int i = 0;
            double sum = 0;
            double countOld = 0;

            for (double r = 0.5; r <= 2.5; r += 0.5)
            {
                double sumOld = sum;
                List<Point3D> points = GetSphere(p, r, s); // r change nconst!

                foreach (Point3D point in points)
                {
                    sum += d.GetValue(point); //real coordinates
                }
                sum -= sumOld;
                
                double count = points.Count - countOld;
                double avg = sum / count;
                countOld = points.Count;
                norm += avg * avg;
                fv[i] = avg;
                i++;
            }

            norm = Math.Sqrt(norm);
            if (norm == 0)
            {
                norm = 1;
            }
            //return new FeatureVector(p, fv[0] / norm, fv[1] / norm, fv[2] / norm, fv[3] / norm, fv[4] / norm);
            return new FeatureVector(p, fv[0], fv[1], fv[2], fv[3], fv[4]);
        }

        public FeatureVector ComputeFeatureVector3(IData d, Point3D p)
        {
            double[] fv = new double[5];
            double norm = 0;
            double nConst = (d.XSpacing + d.YSpacing + d.ZSpacing) / 3;
            double s = nConst*2; //nconst /2

            int i = 0;
            double sum = 0;
            double countOld = 0;

            for (double r = 1; r <= 5; r += 1)
            {
                double sumOld = sum;
                List<Point3D> points = GetSphere(p, r, s); // r change nconst!

                foreach (Point3D point in points)
                {
                    sum += d.GetValue(point); //real coordinates
                }
                sum -= sumOld;

                double count = points.Count - countOld;
                double avg = sum / count;
                countOld = points.Count;
                norm += avg * avg;
                fv[i] = avg;
                i++;
            }

            norm = Math.Sqrt(norm);
            if (norm == 0)
            {
                norm = 1;
            }
            return new FeatureVector(p, fv[0] / norm, fv[1] / norm, fv[2] / norm, fv[3] / norm, fv[4] / norm);
           // return new FeatureVector(p, fv[0], fv[1], fv[2], fv[3], fv[4]);
        }

        public FeatureVector ComputeFeatureVectorA(int[] t, Point3D p)
        {
            double[] fv = new double[5];
            double norm = 0;
            for (double r = 0; r <= 2; r += 0.5)
            {
                double sum = 0;
                List<Point3D> points = GetSphere(p, r);
                foreach (Point3D point in points)
                {
                    sum += 1 * (point.X + t[0]) + 3 * (point.Y + t[1]) + 8 * (point.Z + t[2]); //Avalue
                }
                int i = (int)(r / 0.5);
                norm += sum;
                fv[i] = sum;
            }
            norm = Math.Sqrt(norm);
            return new FeatureVector(p, fv[0] / norm, fv[1] / norm, fv[2] / norm, fv[3] / norm, fv[4] / norm);
            //return new FeatureVector(p, fv[0], fv[1], fv[2], fv[3], fv[4]);
        }
    }
}
