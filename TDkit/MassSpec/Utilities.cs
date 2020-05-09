
namespace TDkit.MassSpec
{
    /// <summary>
    /// Common utility functions related to mass spec data
    /// </summary>
    public class Utilities
    {
        private const double PROTON = 1.007276466;

        /// <summary>
        /// Converts mass to m/z
        /// </summary>
        /// <param name="neutralMass">Neutral mass to convert</param>
        /// <param name="charge">Charge of species</param>
        /// <param name="chargeCarrier"></param>
        /// <returns></returns>
        public static double MassToMz(double neutralMass, int charge, bool positive = true, double chargeCarrier=PROTON)
        {
            if (positive)
                return neutralMass / charge + chargeCarrier;
            return neutralMass / charge - chargeCarrier;
        }

        /// <summary>
        /// Converts m/z to mass
        /// </summary>
        /// <param name="neutralMass">Neutral mass to convert</param>
        /// <param name="charge">Charge of species</param>
        /// <param name="chargeCarrier"></param>
        /// <returns></returns>
        public static double MztoMass(double mz, int charge, bool positive = true, double chargeCarrier = PROTON)
        {
            if (positive)
                return (mz - chargeCarrier) * charge;
            return (mz + chargeCarrier) * charge;
        }
    }
}
