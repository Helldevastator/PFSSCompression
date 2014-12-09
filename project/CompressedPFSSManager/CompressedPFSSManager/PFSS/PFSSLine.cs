﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressedPFSSManager.PFSS
{
    enum TYPE
    {
        SUN_TO_OUTSIDE,
        SUN_TO_SUN,
        OUTSIDE_TO_SUN
    }

    class PFSSLine : IComparable<PFSSLine>
    {
        internal TYPE Type { get; private set; }
        internal List<PFSSPoint> points { get; private set; }

        internal float[] extraX;
        internal float[] extraY;
        internal float[] extraZ;
        internal ExtraPoints[] extra;

        public PFSSLine(TYPE t, List<PFSSPoint> points)
        {
            this.Type = t;
            this.points = points;
        }



        public int CompareTo(PFSSLine other)
        {
            if (other.Type == this.Type)
                return 0;
            if (this.Type == TYPE.OUTSIDE_TO_SUN)
                return -1;
            if (other.Type == TYPE.OUTSIDE_TO_SUN)
                return 1;
            return 0;
        }
    }
}
