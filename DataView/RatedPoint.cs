﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    class RatedPoint : PointWithFeatures
    {
        public double rating;
        public RatedPoint(PointWithFeatures point, double rating) : base(point.x, point.y, point.z, point.featureVector)
        {
            this.rating = rating;
        }
    }
}