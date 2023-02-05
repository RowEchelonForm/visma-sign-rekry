namespace HolidayPlanner
{
    /// <summary>
    /// Interface for holiday planner validation implementations.
    /// </summary>
    public interface IHolidayPlannerValidator
    {
        /// <summary>
        /// Validates the holiday period from <paramref name="startDate"/>
        /// to <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate">Start date of the period.</param>
        /// <param name="endDate">End date of the period.</param>
        /// <returns></returns>
        ValidationResult ValidateHolidayPeriod(DateTime startDate, DateTime endDate);
    }
}