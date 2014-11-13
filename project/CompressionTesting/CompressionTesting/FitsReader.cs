﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompressionTesting.PFSS;
using System.IO;
using nom.tam.fits;

namespace CompressionTesting
{
    class FitsReader
    {

        public static PFSSData ReadFits(FileInfo input, bool subsampling) 
        {
            bool compressed = input.Extension == ".gz";

            Fits fits = new Fits(input, compressed);
            BasicHDU[] hdus = fits.Read();
            BinaryTableHDU bhdu = (BinaryTableHDU)hdus[1];
            //bhdu1.

            string type = string.Join(" ", ((string[])bhdu.GetColumn("TYPE")).Select(v => v.ToString()));
            string date = string.Join(" ", ((string[])bhdu.GetColumn("DATE_TIME")).Select(v => v.ToString()));

            double b0 = ((double[])bhdu.GetColumn("B0"))[0];
            double l0 = ((double[])bhdu.GetColumn("L0"))[0];
            short[] ptr = ((short[])((Array[])bhdu.GetColumn("PTR"))[0]);
            short[] ptr_nz_len = ((short[])((Array[])bhdu.GetColumn("PTR_NZ_LEN"))[0]);
            short[] ptph = ((short[])((Array[])bhdu.GetColumn("PTPH"))[0]);
            short[] ptth = ((short[])((Array[])bhdu.GetColumn("PTTH"))[0]);

            SimpleLineConstructor constructor = new SimpleLineConstructor(l0, b0, ptr, ptr_nz_len, ptph, ptth, subsampling);

            PFSSData data = constructor.ConstructLines();
            return data;
        }

        public static PFSSData ReadFloatFits(FileInfo input, bool quantization)
        {
            bool compressed = input.Extension == ".gz";

            Fits fits = new Fits(input, compressed);
            BasicHDU[] hdus = fits.Read();
            BinaryTableHDU bhdu = (BinaryTableHDU)hdus[1];
            //bhdu1.
            string type = string.Join(" ", ((string[])bhdu.GetColumn("TYPE")).Select(v => v.ToString()));
            string date = string.Join(" ", ((string[])bhdu.GetColumn("DATE_TIME")).Select(v => v.ToString()));

            double b0 = ((double[])bhdu.GetColumn("B0"))[0];
            double l0 = ((double[])bhdu.GetColumn("L0"))[0];
            float[] ptr = ((float[])((Array[])bhdu.GetColumn("PTR"))[0]);
            short[] ptr_nz_len = ((short[])((Array[])bhdu.GetColumn("PTR_NZ_LEN"))[0]);
            float[] ptph = ((float[])((Array[])bhdu.GetColumn("PTPH"))[0]);
            float[] ptth = ((float[])((Array[])bhdu.GetColumn("PTTH"))[0]);

            FloatLineConstructor constructor = new FloatLineConstructor(l0, b0, ptr, ptr_nz_len, ptph, ptth, quantization);

            PFSSData data = constructor.ConstructLines();
            return data;
        }
    }
}
