using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace drs_backend_phase1.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbEntityValidationExtensions
    {
        /// <summary>
        /// Databases the entity validation exception to string.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public static string DbEntityValidationExceptionToString(this DbEntityValidationException e)
        {
            var validationErrors = e.DbEntityValidationResultToString();
            var exceptionMessage = string.Format("{0}{1}Validation errors:{1}{2}", e, Environment.NewLine, validationErrors);
            return exceptionMessage;
        }

        /// <summary>
        /// A DbEntityValidationException extension method that aggregate database entity validation results to string.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <returns></returns>
        public static string DbEntityValidationResultToString(this DbEntityValidationException e)
        {
            return e.EntityValidationErrors
                .Select(dbEntityValidationResult => dbEntityValidationResult.DbValidationErrorsToString(dbEntityValidationResult.ValidationErrors))
                .Aggregate(string.Empty, (current, next) => string.Format("{0}{1}{2}", current, Environment.NewLine, next));
        }

        /// <summary>
        /// A DbEntityValidationResult extension method that to strings database validation errors.
        /// </summary>
        /// <param name="dbEntityValidationResult">The database entity validation result.</param>
        /// <param name="dbValidationErrors">The database validation errors.</param>
        /// <returns></returns>
        public static string DbValidationErrorsToString(this DbEntityValidationResult dbEntityValidationResult, IEnumerable<DbValidationError> dbValidationErrors)
        {
            var entityName = string.Format("[{0}]", dbEntityValidationResult.Entry.Entity.GetType().Name);
            const string indentation = "\t - ";
            var aggregatedValidationErrorMessages = dbValidationErrors.Select(error => string.Format("[{0} - {1}]", error.PropertyName, error.ErrorMessage))
                .Aggregate(string.Empty, (current, validationErrorMessage) => current + (Environment.NewLine + indentation + validationErrorMessage));
            return string.Format("{0}{1}", entityName, aggregatedValidationErrorMessages);
        }
    }
}