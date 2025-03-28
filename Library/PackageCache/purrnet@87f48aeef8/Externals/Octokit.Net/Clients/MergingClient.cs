﻿using System.Threading.Tasks;

namespace Octokit
{
    /// <summary>
    /// A client for GitHub's Git Merging API.
    /// </summary>
    /// <remarks>
    /// See the <a href="https://developer.github.com/v3/repos/merging/">Git Merging API documentation</a> for more information.
    /// </remarks>
    public class MergingClient : ApiClient, IMergingClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MergingClient"/> class.
        /// </summary>
        /// <param name="apiConnection">The client's connection</param>
        public MergingClient(IApiConnection apiConnection) : base(apiConnection)
        {
        }

        /// <summary>
        /// Create a merge for a given repository
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/repos/merging/#perform-a-merge
        /// </remarks>
        /// <param name="owner">The owner of the repository</param>
        /// <param name="name">The name of the repository</param>
        /// <param name="merge">The merge to create</param>
        /// <returns></returns>
        [ManualRoute("POST", "/repos/{owner}/{repo}/merges")]
        public Task<Merge> Create(string owner, string name, NewMerge merge)
        {
            Ensure.ArgumentNotNullOrEmptyString(owner, nameof(owner));
            Ensure.ArgumentNotNullOrEmptyString(name, nameof(name));
            Ensure.ArgumentNotNull(merge, nameof(merge));

            return ApiConnection.Post<Merge>(ApiUrls.CreateMerge(owner, name), merge);
        }

        /// <summary>
        /// Create a merge for a given repository
        /// </summary>
        /// <remarks>
        /// http://developer.github.com/v3/repos/merging/#perform-a-merge
        /// </remarks>
        /// <param name="repositoryId">The Id of the repository</param>
        /// <param name="merge">The merge to create</param>
        /// <returns></returns>
        [ManualRoute("POST", "/repositories/{id}/merges")]
        public Task<Merge> Create(long repositoryId, NewMerge merge)
        {
            Ensure.ArgumentNotNull(merge, nameof(merge));

            return ApiConnection.Post<Merge>(ApiUrls.CreateMerge(repositoryId), merge);
        }
    }
}