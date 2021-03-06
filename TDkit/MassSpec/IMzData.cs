﻿using System.Collections.Generic;

namespace TDkit.MassSpec
{
    /// <summary>
    /// Represents minimal spectral data that contains only m/z and intensity
    /// </summary>
    interface IMzData
    {
        /// <summary>
        /// The smallest m/z value
        /// </summary>
        double FirstMz { get; }

        /// <summary>
        /// The largest m/z value
        /// </summary>
        double LastMz { get; }

        /// <summary>
        /// Array of m/z values
        /// </summary>
        /// <returns></returns>
        IList<double> GetMz();

        /// <summary>
        /// Array of intensity values
        /// </summary>
        /// <returns></returns>
        IList<double> GetIntensity();

        /// <summary>
        /// Length of data or number of data points
        /// </summary>
        int Length { get; }
    }
}
