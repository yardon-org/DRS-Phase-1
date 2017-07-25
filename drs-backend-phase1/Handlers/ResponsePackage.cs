using System.Collections.Generic;

namespace drs_backend_phase1.Handlers
{
    /// <summary>
    /// Response Package fpr Validation Purpoawa
    /// </summary>
    public class ResponsePackage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsePackage"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="errors">The errors.</param>
        public ResponsePackage(object result, List<string> errors)
        {
            Errors = errors;
            Result = result;
        }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public object Result { get; set; }
    }
}