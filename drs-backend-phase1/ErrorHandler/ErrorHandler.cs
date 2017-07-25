using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

/// <summary>
/// Custom Error Message
/// </summary>
public class ErrorResult : IHttpActionResult
{
    /// <summary>
    /// The error
    /// </summary>
    private readonly Error _error;
    /// <summary>
    /// The request
    /// </summary>
    private readonly HttpRequestMessage _request;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResult" /> class.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <param name="request">The request.</param>
    public ErrorResult(Error error, HttpRequestMessage request)
    {
        _error = error;
        _request = request;
    }

    /// <summary>
    /// Creates an <see cref="T:System.Net.Http.HttpResponseMessage" /> asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that, when completed, contains the <see cref="T:System.Net.Http.HttpResponseMessage" />.
    /// </returns>
    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage
        {
            Content = new ObjectContent<Error>(_error, new JsonMediaTypeFormatter()),
            RequestMessage = _request
        };
        return Task.FromResult(response);
    }
}

/// <summary>
/// 
/// </summary>
public class Error
{
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>
    /// The code.
    /// </value>
    public string Code { get; set; }
    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>
    /// The message.
    /// </value>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    /// <value>
    /// The data.
    /// </value>
    public object[] Data { get; set; }
}