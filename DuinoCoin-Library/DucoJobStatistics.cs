using System;

namespace DuinoCoin
{
    /// <summary>
    /// Class to store job statisctical information.
    /// </summary>
    public class DucoJobStatistics : ICloneable
    {
        /// <summary>
        /// Get the total number of accepted jobs.
        /// </summary>
        public uint Accepted { get { return this.Good; } }

        /// <summary>
        /// Gets the number of bad jobs.
        /// </summary>
        public uint Bad { get; internal set; }

        /// <summary>
        /// Gets the number of blocked jobs.
        /// </summary>
        public uint Block { get; internal set; }

        /// <summary>
        /// Gets the number of good jobs.
        /// </summary>
        public uint Good { get; internal set; }

        /// <summary>
        /// Gets the total number of the rejected jobs.
        /// </summary>
        /// <remarks>
        /// Rejected jobs is the sum of <see cref="Bad"/> and <see cref="Block"/>.
        /// </remarks>
        public uint Rejected { get { return this.Bad + this.Block; } }

        /// <summary>
        /// Creates a new <see cref="DucoJobStatistics"/> instance that is a copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="DucoJobStatistics"/> that is a copy of this instance.</returns>
        internal DucoJobStatistics Clone()
        {
            return new DucoJobStatistics() {
                Bad = this.Bad,
                Block = this.Block,
                Good = this.Good
            };
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Resets the statistical information.
        /// </summary>
        internal void Reset()
        {
            this.Bad = 0;
            this.Block = 0;
            this.Good = 0;
        }

        /// <summary>
        /// Creates a new instance of the class <see cref="DucoJobStatistics"/>.
        /// </summary>
        internal DucoJobStatistics()
        {
            this.Reset();
        }
    }
}
