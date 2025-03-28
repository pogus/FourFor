﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Octokit.Internal;

namespace Octokit
{
    /// <summary>
    /// The affected files in a <see cref="GitHubCommit"/>.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class GitHubCommitFile
    {
        public GitHubCommitFile()
        {
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        public GitHubCommitFile(string filename, int additions, int deletions, int changes, string status,
            string blobUrl, string contentsUrl, string rawUrl, string sha, string patch, string previousFileName)
        {
            Filename = filename;
            Additions = additions;
            Deletions = deletions;
            Changes = changes;
            Status = status;
            BlobUrl = blobUrl;
            ContentsUrl = contentsUrl;
            RawUrl = rawUrl;
            Sha = sha;
            Patch = patch;
            PreviousFileName = previousFileName;
        }

        /// <summary>
        /// The name of the file
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        public string Filename { get; private set; }

        /// <summary>
        /// Number of additions performed on the file.
        /// </summary>
        public int Additions { get; private set; }

        /// <summary>
        /// Number of deletions performed on the file.
        /// </summary>
        public int Deletions { get; private set; }

        /// <summary>
        /// Number of changes performed on the file.
        /// </summary>
        public int Changes { get; private set; }

        /// <summary>
        /// File status, like modified, added, deleted.
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        /// The url to the file blob.
        /// </summary>
        public string BlobUrl { get; private set; }

        /// <summary>
        /// The url to file contents API.
        /// </summary>
        public string ContentsUrl { get; private set; }

        /// <summary>
        /// The raw url to download the file.
        /// </summary>
        public string RawUrl { get; private set; }

        /// <summary>
        /// The SHA of the file.
        /// </summary>
        public string Sha { get; private set; }

        /// <summary>
        /// The patch associated with the commit
        /// </summary>
        public string Patch { get; private set; }

        /// <summary>
        /// The previous filename for a renamed file.
        /// </summary>
        [Parameter(Key = "previous_filename")]
        public string PreviousFileName { get; private set; }

        internal string DebuggerDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "Filename: {0} ({1})", Filename, Status); }
        }
    }
}