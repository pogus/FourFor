﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Octokit.Internal
{
    /// <summary>
    /// Generic Http client. Useful for those who want to swap out System.Net.HttpClient with something else.
    /// </summary>
    /// <remarks>
    /// Most folks won't ever need to swap this out. But if you're trying to run this on Windows Phone, you might.
    /// </remarks>
    public interface IHttpClient : IDisposable
    {
        /// <summary>
        /// Sends the specified request and returns a response.
        /// </summary>
        /// <param name="request">A <see cref="IRequest"/> that represents the HTTP request</param>
        /// <param name="cancellationToken">Used to cancel the request</param>
        /// <param name="preprocessResponseBody">Function to preprocess HTTP response prior to deserialization (can be null)</param>
        /// <returns>A <see cref="Task" /> of <see cref="IResponse"/></returns>
        Task<IResponse> Send(IRequest request, CancellationToken cancellationToken,
            Func<object, object> preprocessResponseBody = null);


        /// <summary>
        /// Sets the timeout for the connection between the client and the server.
        /// </summary>
        /// <param name="timeout">The Timeout value</param>
        void SetRequestTimeout(TimeSpan timeout);
    }
}