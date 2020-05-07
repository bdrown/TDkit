using System.Collections.Generic;

namespace TDkit.MassSpec
{
    class Spectrum : IMzData
    {
        private readonly double[] mz;
        private readonly double[] intensity;

        /// <summary>
        /// Simple constructor for Spectrum instance
        /// </summary>
        /// <param name="mz">Array of mz data</param>
        /// <param name="intensity">Array of intensity data</param>
        public Spectrum(double[] mz, double[] intensity, double rt = 0.0, int MSLevel = 1, double precursor = 0.0)
        {
            this.intensity = intensity;
            this.mz = mz;
            this.RetentionTime = rt;
            this.MSLevel = MSLevel;
            this.Precursor = precursor;
        }

        /// <summary>
        /// Simple constructor for Spectrum instance
        /// </summary>
        /// <param name="mz">List of mz data</param>
        /// <param name="intensity">List of intensity data</param>
        public Spectrum(List<double> mz, List<double> intensity, double rt = 0.0, int MSLevel = 1, double precursor = 0.0)
        {
            this.intensity = intensity.ToArray();
            this.mz = mz.ToArray();
            this.RetentionTime = rt;
            this.MSLevel = MSLevel;
            this.Precursor = precursor;
        }

        /// <summary>
        /// Time at which Spectrum was acquired in minutes
        /// </summary>
        public double RetentionTime { get; }

        /// <summary>
        /// Level of MS acquisition. MSLevel > 1 indicates fragment spectrum.
        /// </summary>
        public int MSLevel { get; }

        /// <summary>
        /// mz selection used to acquire spectrum
        /// </summary>
        public double Precursor { get; }

        /// <summary>
        /// First m/z data value
        /// </summary>
        public double FirstMz => mz[0];

        /// <summary>
        /// Last m/z data value
        /// </summary>
        public double LastMz => mz[mz.Length - 1];

        /// <summary>
        /// Number of data points in spectrum
        /// </summary>
        public int Length => mz.Length;

        /// <summary>
        /// Provides the intensity data in the spectrum
        /// </summary>
        /// <returns></returns>
        public IList<double> GetIntensity()
        {
            return intensity;
        }

        /// <summary>
        /// Provides the m/z data in the spectrum
        /// </summary>
        /// <returns></returns>
        public IList<double> GetMz()
        {
            return mz;
        }
    }
}
