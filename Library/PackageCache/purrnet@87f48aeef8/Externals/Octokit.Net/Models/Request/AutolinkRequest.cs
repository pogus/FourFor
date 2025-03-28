﻿using Octokit.Internal;
using System.Diagnostics;


namespace Octokit
{
    /// <summary>
    /// Represents a repository Autolink object.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class AutolinkRequest
    {
        public AutolinkRequest()
        {
        }

        public AutolinkRequest(string keyPrefix, string urlTemplate, bool isAlphanumeric)
        {
            this.KeyPrefix = keyPrefix;
            this.UrlTemplate = urlTemplate;
            this.IsAlphanumeric = isAlphanumeric;
        }


        /// <summary>
        /// This prefix appended by certain characters will generate a link any time it is found in an issue, pull request, or commit.
        /// </summary>
        public string KeyPrefix { get; set; }

        /// <summary>
        /// The URL must contain &lt;num&gt; for the reference number. &lt;num&gt; matches different characters depending on the value of is_alphanumeric.
        /// </summary>
        public string UrlTemplate { get; set; }

        /// <summary>
        /// Whether this autolink reference matches alphanumeric characters. If true, the &lt;num&gt; parameter of the url_template matches alphanumeric characters A-Z (case insensitive), 0-9, and -. If false, this autolink reference only matches numeric characters.
        /// </summary>
        public bool IsAlphanumeric { get; set; }


        internal string DebuggerDisplay
        {
            get { return new SimpleJsonSerializer().Serialize(this); }
        }
    }
}