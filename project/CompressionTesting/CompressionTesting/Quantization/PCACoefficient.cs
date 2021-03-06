﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompressionTesting.PFSS;

namespace CompressionTesting.Quantization
{
    class PCACoefficient
    {
        static int count = 0;
        public static void Backwards(PFSSData data)
        {
            foreach (PFSSLine l in data.lines)
            {
                ErrorCalculator.Point pCheck = new ErrorCalculator.Point(l.pcaTransform[0, 2], l.pcaTransform[1, 2], l.pcaTransform[2, 2]);

                ErrorCalculator.Point p0 = new ErrorCalculator.Point(l.pcaTransform[0, 0], l.pcaTransform[1, 0], l.pcaTransform[2, 0]);
                ErrorCalculator.Point p1 = new ErrorCalculator.Point(l.pcaTransform[0, 1], l.pcaTransform[1, 1], l.pcaTransform[2, 1]);
                ErrorCalculator.Point p3 = ErrorCalculator.Point.cross(p1, p0);
                ErrorCalculator.Point p4 = ErrorCalculator.Point.cross(p0, p1);

                //not finished, but used to try to calculate a meaningful error
                l.pcaTransform[0, 2] = p3.x;
                l.pcaTransform[1, 2] = p3.y;
                l.pcaTransform[2, 2] = p3.z;

                if (Math.Sign(p3.x) != Math.Sign(pCheck.x) || Math.Sign(p3.y) != Math.Sign(pCheck.y) || Math.Sign(p3.z) != Math.Sign(pCheck.z))
                {
                    l.pcaTransform[0, 2] = p4.x;
                    l.pcaTransform[1, 2] = p4.y;
                    l.pcaTransform[2, 2] = p4.z;
                }
                count++;
            }
        }

        public static void ForwardMinus(PFSSData data)
        {
            foreach (PFSSLine l in data.lines)
            {
                ErrorCalculator.Point pCheck = new ErrorCalculator.Point(l.pcaTransform[0, 2], l.pcaTransform[1, 2], l.pcaTransform[2, 2]);

                ErrorCalculator.Point p0 = new ErrorCalculator.Point(l.pcaTransform[0, 0], l.pcaTransform[1, 0], l.pcaTransform[2, 0]);
                ErrorCalculator.Point p1 = new ErrorCalculator.Point(l.pcaTransform[0, 1], l.pcaTransform[1, 1], l.pcaTransform[2, 1]);
                ErrorCalculator.Point p3 = ErrorCalculator.Point.cross(p1, p0);
                if (Math.Sign(p3.x) != Math.Sign(pCheck.x) || Math.Sign(p3.y) != Math.Sign(pCheck.y) || Math.Sign(p3.z) != Math.Sign(pCheck.z))
                {
                    l.minus = 1;
                }
                else
                {
                    l.minus = 0;
                }
            }
        }

        public static void ForwardQuantization(PFSSData data,float factor = 1000)
        {
            ForwardMinus(data);
            foreach (PFSSLine l in data.lines)
            {
                for(int i = 0; i < 2;i++) {
                    for(int j = 0; j < 3;j++) {
                        l.pcaTransform[j,i] = (short)(l.pcaTransform[j,i]* 32767);
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    l.means[i] = (short)(l.means[i] / factor);
                }
            }
        }

        public static void BackwardQuantization(PFSSData data, float factor = 1000)
        {
            
            foreach (PFSSLine l in data.lines)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        l.pcaTransform[j, i] = l.pcaTransform[j, i] / 32767f;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    l.means[i] *=  factor;
                }
            }

            Backwards(data);
        }

        public static void MultiplyLinear(PFSSData data, int channel,double start, double increase, int offset, int length)
        {
            foreach (PFSSLine l in data.lines)
            {
                double div = start;
                for (int i = offset; i < offset + length && i < l.points.Count; i++)
                {
                    PFSSPoint p = l.points[i];
                    if(channel == 0) p.x = (float)(p.x * div);
                    if(channel == 1) p.y = (float)(p.y * div);
                    if (channel == 2) p.z = (float)(p.z * div);
                    div += increase;
                }
            }
        }

        public static void DivideLinear(PFSSData data, int channel, double start, double increase, int offset, int length)
        {
            foreach (PFSSLine l in data.lines)
            {
                double div = start;
                for (int i = offset; i < offset + length && i < l.points.Count; i++)
                {
                    PFSSPoint p = l.points[i];
                    if (channel == 0) p.x = (float)(p.x / div);
                    if (channel == 1) p.y = (float)(p.y / div);
                    if (channel == 2) p.z = (float)(p.z / div);
                    div += increase;
                }
            }
        }
    }
}
