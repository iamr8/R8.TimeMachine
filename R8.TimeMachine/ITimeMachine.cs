using System;

namespace R8.TimeMachine
{
    /// <summary>
    ///     An interface to manage time-traveling.
    /// </summary>
    public interface ITimeMachine
    {
        /// <summary>
        ///     Returns <see cref="UtcNow" /> in the specified timezone.
        /// </summary>
        /// <param name="timezone">A <see cref="LocalTimezone" /> object.</param>
        /// <returns>A <see cref="TimezoneDateTime" /> object.</returns>
        /// <remarks>It's recommended to be used inside services, to have more control over the time.</remarks>
        TimezoneDateTime Now(LocalTimezone? timezone = null);

        /// <summary>
        ///     Returns the current UTC time.
        /// </summary>
        /// <returns>A <see cref="DateTime" /> object.</returns>
        /// <remarks>It's recommended to be used inside services, to have more control over the time.</remarks>
        DateTime UtcNow();
    }
}