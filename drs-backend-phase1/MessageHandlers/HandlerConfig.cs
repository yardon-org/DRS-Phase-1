using System.Collections.ObjectModel;
using System.Net.Http;

namespace drs_backend_phase1.MessageHandlers
{
    /// <summary>
    /// HandlerConfig
    /// </summary>
    public class HandlerConfig
    {
        /// <summary>
        /// Registers the handlers.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        public static void RegisterHandlers(Collection<DelegatingHandler> handlers)
        {
            handlers.Add(new CorsMessageHandler());
        }
    }
}