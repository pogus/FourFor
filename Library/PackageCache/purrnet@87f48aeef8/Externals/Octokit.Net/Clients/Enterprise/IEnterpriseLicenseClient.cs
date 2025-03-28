﻿using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Octokit
{
    /// <summary>
    /// A client for GitHub's Enterprise License API
    /// </summary>
    /// <remarks>
    /// See the <a href="https://developer.github.com/v3/enterprise/license/">Enterprise License API documentation</a> for more information.
    ///</remarks>
    public interface IEnterpriseLicenseClient
    {
        /// <summary>
        /// Gets GitHub Enterprise License Information (must be Site Admin user).
        /// </summary>
        /// <remarks>
        /// https://developer.github.com/v3/enterprise/license/#get-license-information
        /// </remarks>
        /// <returns>The <see cref="LicenseInfo"/> statistics.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
        Task<LicenseInfo> Get();
    }
}