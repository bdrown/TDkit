using System;

namespace TDkit.MassSpec
{
    public class ChargedIsotopicDistribution : IMzData
    {
        private readonly double[] mz;
        private readonly double[] intensity;

        public ChargedIsotopicDistribution(double[] mz, double[] intensity, int charge)
        {
            this.intensity = intensity;
            this.mz = mz;
            this.Charge = charge;
        }

        public int Charge { get; }

        public double FirstMz => mz[0];

        public double LastMz => mz[mz.Length - 1];

        public int Length => mz.Length;

        public double[] GetIntensity()
        {
            return intensity;
        }

        public double[] GetMz()
        {
            return mz;
        }
    }
}
