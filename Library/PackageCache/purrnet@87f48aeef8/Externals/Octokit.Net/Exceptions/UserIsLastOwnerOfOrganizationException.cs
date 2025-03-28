﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.Serialization;

namespace Octokit
{
    /// <summary>
    /// Represents an error that occurs when trying to convert the 
    /// last owner of the organization to an outside collaborator
    /// </summary>
    [Serializable]
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors",
        Justification = "These exceptions are specific to the GitHub API and not general purpose exceptions")]
    public class UserIsLastOwnerOfOrganizationException : ApiException
    {
        /// <summary>
        /// Constructs an instance of the <see cref="UserIsLastOwnerOfOrganizationException"/> class.
        /// </summary>
        /// <param name="response">The HTTP payload from the server</param>
        public UserIsLastOwnerOfOrganizationException(IResponse response) : this(response, null)
        {
        }

        /// <summary>
        /// Constructs an instance of the <see cref="UserIsLastOwnerOfOrganizationException"/> class.
        /// </summary>
        /// <param name="response">The HTTP payload from the server</param>
        /// <param name="innerException">The inner exception</param>
        public UserIsLastOwnerOfOrganizationException(IResponse response, ApiException innerException)
            : base(response, innerException)
        {
            Debug.Assert(response != null && response.StatusCode == HttpStatusCode.Forbidden,
                "UserIsLastOwnerOfOrganizationException created with the wrong HTTP status code");
        }

        // https://developer.github.com/v3/orgs/outside_collaborators/#response-if-user-is-the-last-owner-of-the-organization
        public override string Message => ApiErrorMessageSafe ?? "User is the last owner of the organization.";

        /// <summary>
        /// Constructs an instance of <see cref="UserIsLastOwnerOfOrganizationException"/>.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"/> that contains
        /// contextual information about the source or destination.
        /// </param>
        protected UserIsLastOwnerOfOrganizationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}