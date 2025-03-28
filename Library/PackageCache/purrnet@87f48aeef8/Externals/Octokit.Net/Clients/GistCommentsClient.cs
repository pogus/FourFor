﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Octokit
{
    /// <summary>
    /// A client for GitHub's Gist Comments API.
    /// </summary>
    /// <remarks>
    /// See the <a href="http://developer.github.com/v3/gists/comments/">Gist Comments API documentation</a> for more information.
    /// </remarks>
    public class GistCommentsClient : ApiClient, IGistCommentsClient
    {
        /// <summary>
        /// Instantiates a new GitHub Gist Comments API client.
        /// </summary>
        /// <param name="apiConnection">An API connection</param>
        public GistCommentsClient(IApiConnection apiConnection) : base(apiConnection)
        {
        }

        /// <summary>
        /// Gets a single comment by gist- and comment id.
        /// </summary>
        /// <remarks>http://developer.github.com/v3/gists/comments/#get-a-single-comment</remarks>
        /// <param name="gistId">The id of the gist</param>
        /// <param name="commentId">The id of the comment</param>
        /// <returns>Task{GistComment}.</returns>
        [ManualRoute("GET", "/gists/{gist_id}/comments/{comment_id}")]
        public Task<GistComment> Get(string gistId, long commentId)
        {
            return ApiConnection.Get<GistComment>(ApiUrls.GistComment(gistId, commentId));
        }

        /// <summary>
        /// Gets all comments for the gist with the specified id.
        /// </summary>
        /// <remarks>http://developer.github.com/v3/gists/comments/#list-comments-on-a-gist</remarks>
        /// <param name="gistId">The id of the gist</param>
        /// <returns>Task{IReadOnlyList{GistComment}}.</returns>
        [ManualRoute("GET", "/gists/{gist_id}/comments")]
        public Task<IReadOnlyList<GistComment>> GetAllForGist(string gistId)
        {
            Ensure.ArgumentNotNullOrEmptyString(gistId, nameof(gistId));

            return GetAllForGist(gistId, ApiOptions.None);
        }

        /// <summary>
        /// Gets all comments for the gist with the specified id.
        /// </summary>
        /// <remarks>http://developer.github.com/v3/gists/comments/#list-comments-on-a-gist</remarks>
        /// <param name="gistId">The id of the gist</param>
        /// <param name="options">Options for changing the API response</param>
        /// <returns>Task{IReadOnlyList{GistComment}}.</returns>
        [ManualRoute("GET", "/gists/{gist_id}/comments")]
        public Task<IReadOnlyList<GistComment>> GetAllForGist(string gistId, ApiOptions options)
        {
            Ensure.ArgumentNotNullOrEmptyString(gistId, nameof(gistId));
            Ensure.ArgumentNotNull(options, nameof(options));

            return ApiConnection.GetAll<GistComment>(ApiUrls.GistComments(gistId), options);
        }

        /// <summary>
        /// Creates a comment for the gist with the specified id.
        /// </summary>
        /// <remarks>http://developer.github.com/v3/gists/comments/#create-a-comment</remarks>
        /// <param name="gistId">The id of the gist</param>
        /// <param name="comment">The body of the comment</param>
        /// <returns>Task{GistComment}.</returns>
        [ManualRoute("POST", "/gists/{gist_id}/comments")]
        public Task<GistComment> Create(string gistId, string comment)
        {
            Ensure.ArgumentNotNullOrEmptyString(comment, nameof(comment));

            return ApiConnection.Post<GistComment>(ApiUrls.GistComments(gistId), new BodyWrapper(comment));
        }

        /// <summary>
        /// Updates the comment with the specified gist- and comment id.
        /// </summary>
        /// <remarks>http://developer.github.com/v3/gists/comments/#edit-a-comment</remarks>
        /// <param name="gistId">The id of the gist</param>
        /// <param name="commentId">The id of the comment</param>
        /// <param name="comment">The updated body of the comment</param>
        /// <returns>Task{GistComment}.</returns>
        [ManualRoute("PATCH", "/gists/{gist_id}/comments/{comment_id}")]
        public Task<GistComment> Update(string gistId, long commentId, string comment)
        {
            Ensure.ArgumentNotNullOrEmptyString(comment, nameof(comment));

            return ApiConnection.Patch<GistComment>(ApiUrls.GistComment(gistId, commentId), new BodyWrapper(comment));
        }

        /// <summary>
        /// Deletes the comment with the specified gist- and comment id.
        /// </summary>
        /// <remarks>http://developer.github.com/v3/gists/comments/#delete-a-comment</remarks>
        /// <param name="gistId">The id of the gist</param>
        /// <param name="commentId">The id of the comment</param>
        /// <returns>Task.</returns>
        [ManualRoute("DELETE", "/gists/{gist_id}/comments/{comment_id}")]
        public Task Delete(string gistId, long commentId)
        {
            return ApiConnection.Delete(ApiUrls.GistComment(gistId, commentId));
        }
    }
}