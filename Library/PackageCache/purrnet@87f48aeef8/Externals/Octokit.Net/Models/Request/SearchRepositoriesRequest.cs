using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Octokit.Internal;

namespace Octokit
{
    /// <summary>
    /// Searching Repositories
    /// https://docs.github.com/rest/search/search#search-repositories
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class SearchRepositoriesRequest : BaseSearchRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRepositoriesRequest"/> class.
        /// </summary>
        public SearchRepositoriesRequest()
        {
            Order = SortDirection.Descending;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRepositoriesRequest"/> class.
        /// </summary>
        /// <param name="term">The search term.</param>
        public SearchRepositoriesRequest(string term)
            : base(term)
        {
            Order = SortDirection.Descending;
        }

        /// <summary>
        /// For https://docs.github.com/search-github/getting-started-with-searching-on-github/sorting-search-results
        /// Optional Sort field. One of stars, forks, or updated. If not provided, results are sorted by best match.
        /// </summary>
        public RepoSearchSort? SortField { get; set; }

        public override string Sort
        {
            get { return SortField.ToParameter(); }
        }

        private IEnumerable<InQualifier> _inQualifier;

        /// <summary>
        /// The in qualifier limits what fields are searched. With this qualifier you can restrict the search to just the repository name, description, README, or any combination of these.
        /// Without the qualifier, only the name and description are searched.
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-repository-name-description-or-contents-of-the-readme-file
        /// </summary>
        public IEnumerable<InQualifier> In
        {
            get { return _inQualifier; }
            set
            {
                if (value != null && value.Any())
                    _inQualifier = value.Distinct().ToList();
            }
        }

        /// <summary>
        /// Filters repositories based on the number of forks, and/or whether forked repositories should be included in the results at all.
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-number-of-forks
        /// </summary>
        public Range Forks { get; set; }

        /// <summary>
        /// Filters repositories based whether forked repositories should be included in the results at all.
        /// Defaults to ExcludeForks
        /// https://docs.github.com/search-github/searching-on-github/searching-in-forks
        /// </summary>
        public ForkQualifier? Fork { get; set; }

        /// <summary>
        /// The size qualifier finds repository's that match a certain size (in kilobytes).
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-repository-size
        /// </summary>
        public Range Size { get; set; }

        /// <summary>
        /// Searches repositories based on the language they’re written in.
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-language
        /// </summary>
        public Language? Language { get; set; }

        /// <summary>
        /// Searches repositories based on the number of stars.
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-number-of-stars
        /// </summary>
        public Range Stars { get; set; }

        /// <summary>
        /// Limits searches to a specific user or repository.
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-within-a-users-or-organizations-repositories
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Filters repositories based on times of creation.
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-when-a-repository-was-created-or-last-updated
        /// </summary>
        public DateRange Created { get; set; }

        /// <summary>
        /// Filters repositories based on when they were last updated.
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-when-a-repository-was-created-or-last-updated
        /// </summary>
        public DateRange Updated { get; set; }

        /// <summary>
        /// Filters repositories based on license
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-license
        /// </summary>
        public RepoSearchLicense? License { get; set; }

        /// <summary>
        /// Filters whether archived repositories should be included (true) or not (false).
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-based-on-whether-a-repository-is-archived
        /// </summary>
        public bool? Archived { get; set; }

        /// <summary>
        /// Filters on whether repositories are tagged with the given topic.
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-topic
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Filters on the number of topics that a repository is associated with.
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-number-of-topics
        /// </summary>
        public Range Topics { get; set; }

        /// <summary>
        /// Filters on repositories based on custom properties.
        /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-based-on-repository-custom-property
        /// </summary>
        public IDictionary<string, string> CustomProperties { get; set; }

        public override IReadOnlyList<string> MergedQualifiers()
        {
            var parameters = new List<string>();

            if (In != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "in:{0}", string.Join(",", In)));
            }

            if (Size != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "size:{0}", Size));
            }

            if (Forks != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "forks:{0}", Forks));
            }

            if (Fork != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "fork:{0}", Fork.ToParameter()));
            }

            if (Stars != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "stars:{0}", Stars));
            }

            if (Language != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "language:\"{0}\"", Language.ToParameter()));
            }

            if (User.IsNotBlank())
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "user:{0}", User));
            }

            if (Created != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "created:{0}", Created));
            }

            if (Updated != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "pushed:{0}", Updated));
            }

            if (Archived != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "archived:{0}",
                    Archived.ToString().ToLower()));
            }

            if (Topic != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "topic:{0}", Topic.ToLower()));
            }

            if (Topics != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "topics:{0}", Topics.ToString().ToLower()));
            }

            if (License != null)
            {
                parameters.Add(string.Format(CultureInfo.InvariantCulture, "license:{0}", License.ToParameter()));
            }

            if (CustomProperties != null)
            {
                foreach (var customProperty in CustomProperties)
                {
                    parameters.Add(string.Format(CultureInfo.InvariantCulture, "props.{0}:{1}", customProperty.Key,
                        customProperty.Value));
                }
            }

            return parameters;
        }

        internal string DebuggerDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "Term: {0} Sort: {1}", Term, Sort); }
        }
    }

    /// <summary>
    /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-repository-name-description-or-contents-of-the-readme-file
    /// The in qualifier limits what fields are searched. With this qualifier you can restrict the search to just the
    /// repository name, description, README, or any combination of these.
    /// </summary>
    public enum InQualifier
    {
        [Parameter(Value = "name")] Name,

        [Parameter(Value = "description")] Description,

        [Parameter(Value = "topics")] Topics,

        [Parameter(Value = "readme")] Readme
    }

    /// <summary>
    /// Helper class in generating the range values for a qualifier e.g. In or Size qualifiers
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Range
    {
        private readonly string query = string.Empty;

        /// <summary>
        /// Matches repositories that are <param name="size">size</param> MB exactly
        /// </summary>
        [SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider",
            MessageId = "System.Int32.ToString")]
        public Range(int size)
        {
            query = size.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Matches repositories that are between <param name="minSize"/> and <param name="maxSize"/> KB
        /// </summary>
        public Range(int minSize, int maxSize)
        {
            query = string.Format(CultureInfo.InvariantCulture, "{0}..{1}", minSize, maxSize);
        }

        /// <summary>
        /// Matches repositories with regards to the size <param name="size"/>
        /// We will use the <param name="op"/> to see what operator will be applied to the size qualifier
        /// </summary>
        public Range(int size, SearchQualifierOperator op)
        {
            switch (op)
            {
                case SearchQualifierOperator.GreaterThan:
                    query = string.Format(CultureInfo.InvariantCulture, ">{0}", size);
                    break;
                case SearchQualifierOperator.LessThan:
                    query = string.Format(CultureInfo.InvariantCulture, "<{0}", size);
                    break;
                case SearchQualifierOperator.LessThanOrEqualTo:
                    query = string.Format(CultureInfo.InvariantCulture, "<={0}", size);
                    break;
                case SearchQualifierOperator.GreaterThanOrEqualTo:
                    query = string.Format(CultureInfo.InvariantCulture, ">={0}", size);
                    break;
            }
        }

        internal string DebuggerDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "Query: {0}", query); }
        }

        /// <summary>
        /// Helper class that build a <see cref="Range"/> with a LessThan comparator used for filtering results
        /// </summary>
        public static Range LessThan(int size)
        {
            return new Range(size, SearchQualifierOperator.LessThan);
        }

        /// <summary>
        /// Helper class that build a <see cref="Range"/> with a LessThanOrEqual comparator used for filtering results
        /// </summary>
        public static Range LessThanOrEquals(int size)
        {
            return new Range(size, SearchQualifierOperator.LessThanOrEqualTo);
        }

        /// <summary>
        /// Helper class that build a <see cref="Range"/> with a GreaterThan comparator used for filtering results
        /// </summary>
        public static Range GreaterThan(int size)
        {
            return new Range(size, SearchQualifierOperator.GreaterThan);
        }

        /// <summary>
        /// Helper class that build a <see cref="Range"/> with a GreaterThanOrEqualTo comparator used for filtering results
        /// </summary>
        public static Range GreaterThanOrEquals(int size)
        {
            return new Range(size, SearchQualifierOperator.GreaterThanOrEqualTo);
        }

        public override string ToString()
        {
            return query;
        }
    }

    /// <summary>
    /// helper class in generating the date range values for the date qualifier e.g.
    /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-when-a-repository-was-created-or-last-updated
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class DateRange
    {
        public const string DateTimePattern = "yyyy-MM-dd'T'HH:mm:sszzz";
        public const string DatePattern = "yyyy-MM-dd";

        private readonly string query = string.Empty;

        /// <summary>
        /// Matches repositories with regards to the <param name="date"/>.
        /// We will use the <param name="op"/> to see what operator will be applied to the date qualifier
        /// </summary>
        [Obsolete(
            "This ctor does not support the time component or timezone and will be removed in a future release. Please use the DateTimeOffset overload instead")]
        public DateRange(DateTime date, SearchQualifierOperator op)
        {
            switch (op)
            {
                case SearchQualifierOperator.GreaterThan:
                    query = date.ToString($">{DatePattern}", CultureInfo.InvariantCulture);
                    break;
                case SearchQualifierOperator.LessThan:
                    query = date.ToString($"<{DatePattern}", CultureInfo.InvariantCulture);
                    break;
                case SearchQualifierOperator.LessThanOrEqualTo:
                    query = date.ToString($"<={DatePattern}", CultureInfo.InvariantCulture);
                    break;
                case SearchQualifierOperator.GreaterThanOrEqualTo:
                    query = date.ToString($">={DatePattern}", CultureInfo.InvariantCulture);
                    break;
            }
        }

        /// <summary>
        /// Matches repositories with regards to both the <param name="from"/> and <param name="to"/> dates.
        /// </summary>
        [Obsolete(
            "This ctor does not support the time component or timezone and will be removed in a future release. Please use the DateTimeOffset overload instead")]
        public DateRange(DateTime from, DateTime to)
        {
            query =
                $"{from.ToString(DatePattern, CultureInfo.InvariantCulture)}..{to.ToString(DatePattern, CultureInfo.InvariantCulture)}";
        }

        /// <summary>
        /// Matches repositories with regards to both the <param name="from"/> and <param name="to"/> date and time values.
        /// </summary>
        public DateRange(DateTimeOffset from, DateTimeOffset to)
        {
            query = EncodeOffset(
                $"{from.ToString(DateTimePattern, CultureInfo.InvariantCulture)}..{to.ToString(DateTimePattern, CultureInfo.InvariantCulture)}");
        }

        /// <summary>
        /// Matches repositories with regards to the <param name="dateTime"/>.
        /// We will use the <param name="op"/> to see what operator will be applied to the date qualifier
        /// </summary>
        public DateRange(DateTimeOffset dateTime, SearchQualifierOperator op)
        {
            switch (op)
            {
                case SearchQualifierOperator.GreaterThan:
                    query = EncodeOffset(dateTime.ToString($">{DateTimePattern}", CultureInfo.InvariantCulture));
                    break;
                case SearchQualifierOperator.LessThan:
                    query = EncodeOffset(dateTime.ToString($"<{DateTimePattern}", CultureInfo.InvariantCulture));
                    break;
                case SearchQualifierOperator.LessThanOrEqualTo:
                    query = EncodeOffset(dateTime.ToString($"<={DateTimePattern}", CultureInfo.InvariantCulture));
                    break;
                case SearchQualifierOperator.GreaterThanOrEqualTo:
                    query = EncodeOffset(dateTime.ToString($">={DateTimePattern}", CultureInfo.InvariantCulture));
                    break;
            }
        }

        private string EncodeOffset(string dateTimeOffset)
        {
            return dateTimeOffset.Replace("+", "%2B");
        }

        internal string DebuggerDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "Query: {0}", query); }
        }

        /// <summary>
        /// helper method to create a LessThan Date Comparison
        /// e.g. &lt; 2011
        /// </summary>
        /// <param name="date">date to be used for comparison (times are ignored)</param>
        /// <returns><see cref="DateRange"/></returns>
        [Obsolete(
            "This method does not support the time component or timezone and will be removed in a future release. Please use the DateTimeOffset overload instead")]
        public static DateRange LessThan(DateTime date)
        {
            return new DateRange(date, SearchQualifierOperator.LessThan);
        }

        /// <summary>
        /// helper method to create a LessThanOrEqualTo Date Comparison
        /// e.g. &lt;= 2011
        /// </summary>
        /// <param name="date">date to be used for comparison (times are ignored)</param>
        /// <returns><see cref="DateRange"/></returns>
        [Obsolete(
            "This method does not support the time component or timezone and will be removed in a future release. Please use the DateTimeOffset overload instead")]
        public static DateRange LessThanOrEquals(DateTime date)
        {
            return new DateRange(date, SearchQualifierOperator.LessThanOrEqualTo);
        }

        /// <summary>
        /// helper method to create a GreaterThan Date Comparison
        /// e.g. > 2011
        /// </summary>
        /// <param name="date">date to be used for comparison (times are ignored)</param>
        /// <returns><see cref="DateRange"/></returns>
        [Obsolete(
            "This method does not support the time component or timezone and will be removed in a future release. Please use the DateTimeOffset overload instead")]
        public static DateRange GreaterThan(DateTime date)
        {
            return new DateRange(date, SearchQualifierOperator.GreaterThan);
        }

        /// <summary>
        /// helper method to create a GreaterThanOrEqualTo Date Comparison
        /// e.g. >= 2011
        /// </summary>
        /// <param name="date">date to be used for comparison (times are ignored)</param>
        /// <returns><see cref="DateRange"/></returns>
        [Obsolete(
            "This method does not support the time component or timezone and will be removed in a future release. Please use the DateTimeOffset overload instead")]
        public static DateRange GreaterThanOrEquals(DateTime date)
        {
            return new DateRange(date, SearchQualifierOperator.GreaterThanOrEqualTo);
        }

        /// <summary>
        /// helper method to create a bounded Date Comparison
        /// e.g. 2015-08-01..2015-10-31
        /// </summary>
        /// <param name="from">earlier date of the two</param>
        /// <param name="to">latter date of the two</param>
        /// <returns><see cref="DateRange"/></returns>
        [Obsolete(
            "This method does not support the time component or timezone and will be removed in a future release. Please use the DateTimeOffset overload instead")]
        public static DateRange Between(DateTime from, DateTime to)
        {
            return new DateRange(from, to);
        }

        /// <summary>
        /// helper method to create a LessThan DateTime Comparison
        /// e.g. &lt; 2011
        /// </summary>
        /// <param name="dateTime">datetime to be used for comparison (times are ignored)</param>
        /// <returns><see cref="DateRange"/></returns>
        public static DateRange LessThan(DateTimeOffset dateTime)
        {
            return new DateRange(dateTime, SearchQualifierOperator.LessThan);
        }

        /// <summary>
        /// helper method to create a LessThanOrEqualTo DateTime Comparison
        /// e.g. &lt;= 2011
        /// </summary>
        /// <param name="dateTime">datetime to be used for comparison (times are ignored)</param>
        /// <returns><see cref="DateRange"/></returns>
        public static DateRange LessThanOrEquals(DateTimeOffset dateTime)
        {
            return new DateRange(dateTime, SearchQualifierOperator.LessThanOrEqualTo);
        }

        /// <summary>
        /// helper method to create a GreaterThan DateTime Comparison
        /// e.g. > 2011
        /// </summary>
        /// <param name="dateTime">datetime to be used for comparison (times are ignored)</param>
        /// <returns><see cref="DateRange"/></returns>
        public static DateRange GreaterThan(DateTimeOffset dateTime)
        {
            return new DateRange(dateTime, SearchQualifierOperator.GreaterThan);
        }

        /// <summary>
        /// helper method to create a GreaterThanOrEqualTo DateTime Comparison
        /// e.g. >= 2011
        /// </summary>
        /// <param name="dateTime">datetime to be used for comparison (times are ignored)</param>
        /// <returns><see cref="DateRange"/></returns>
        public static DateRange GreaterThanOrEquals(DateTimeOffset dateTime)
        {
            return new DateRange(dateTime, SearchQualifierOperator.GreaterThanOrEqualTo);
        }

        /// <summary>
        /// helper method to create a bounded DateTime Comparison
        /// e.g. 2015-08-01..2015-10-31
        /// </summary>
        /// <param name="from">earlier datetime of the two</param>
        /// <param name="to">latter datetime of the two</param>
        /// <returns><see cref="DateRange"/></returns>
        public static DateRange Between(DateTimeOffset from, DateTimeOffset to)
        {
            return new DateRange(from, to);
        }

        public override string ToString()
        {
            return query;
        }
    }

    /// <summary>
    /// Languages that can be searched on in GitHub
    /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-language
    /// </summary>
    public enum Language
    {
#pragma warning disable 1591
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Abap")]
        Abap,
        [Parameter(Value = "ActionScript")] ActionScript,
        Ada,
        Apex,
        [Parameter(Value = "AppleScript")] AppleScript,
        Arc,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Arduino")]
        Arduino,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Conf")]
        [Parameter(Value = "ApacheConf")]
        ApacheConf,
        Asp,
        Assembly,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Augeas")]
        Augeas,

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "HotKey")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "HotKey")]
        [Parameter(Value = "AutoHotkey")]
        AutoHotKey,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Awk")]
        Awk,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Batchfile")]
        Batchfile,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Befunge")]
        Befunge,
        [Parameter(Value = "BlitzMax")] BlitzMax,
        Boo,
        Bro,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "C")]
        C,

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "hs")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "hs")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "hs")]
        [Parameter(Value = "C2HS Haskell")]
        C2hsHaskell,
        Ceylon,
        Chuck,
        Clips,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Clojure")]
        Clojure,
        Cobol,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cmake")]
        Cmake,
        [Parameter(Value = "C-ObjDump")] CObjDump,
        [Parameter(Value = "CoffeeScript")] CoffeeScript,
        [Parameter(Value = "ColdFusion")] ColdFusion,
        CommonLisp,
        Coq,
        [Parameter(Value = "cpp")] CPlusPlus,
        [Parameter(Value = "CSharp")] CSharp,
        Css,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cpp")]
        [Parameter(Value = "Cpp-ObjDump")]
        CppObjDump,
        Cucumber,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cython")]
        Cython,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "D")]
        D,
        [Parameter(Value = "D-ObjDump")] DObjDump,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Darcs")]
        [Parameter(Value = "DarcsPatch")]
        DarcsPatch,
        Dart,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dcpu")]
        [Parameter(Value = "DCPU-16 ASM")]
        Dcpu16Asm,
        Dot,
        Dylan,

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ec")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ec")]
        Ec,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ecere")]
        [Parameter(Value = "Ecere Projects")]
        EcereProjects,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ecl")]
        Ecl,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Edn")]
        Edn,
        Eiffel,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Elixir")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Elixir")]
        Elixir,
        Elm,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Emacs")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Emacs")]
        EmacsLisp,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Erlang")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Erlang")]
        Erlang,
        [Parameter(Value = "F#")] FSharp,
        Factor,
        Fancy,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fantom")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Fantom")]
        Fantom,
        Fish,
        Forth,
        Fortran,
        Gas,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Genshi")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Genshi")]
        Genshi,
        [Parameter(Value = "Gentoo Build")] GentooBuild,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Eclass")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Eclass")]
        [Parameter(Value = "Gentoo Eclass")]
        GentooEclass,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gettext")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gettext")]
        [Parameter(Value = "Gettext Catalog")]
        GettextCatalog,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Glsl")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Glsl")]
        Glsl,
        Go,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gosu")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gosu")]
        Gosu,
        Groff,
        Groovy,

        [Parameter(Value = "Groovy Server Pages")]
        GroovyServerPages,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Haml")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Haml")]
        Haml,

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "HandleBars")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "HandleBars")]
        [Parameter(Value = "HandleBars")]
        HandleBars,
        Haskell,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Haxe")]
        Haxe,
        Http,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ini")]
        Ini,

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Io")]
        Io,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ioke")]
        Ioke,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Irc")]
        [Parameter(Value = "IRC log")]
        IrcLog,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "J")]
        J,
        Java,

        [Parameter(Value = "Java Server Pages")]
        JavaServerPages,
        JavaScript,
        Json,
        Julia,

        [Parameter(Value = "Jupyter Notebook")]
        JupyterNotebook,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Kotlin")]
        Kotlin,
        Lasso,
        Less,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lfe")]
        Lfe,
        [Parameter(Value = "LillyPond")] LillyPond,

        [Parameter(Value = "Literate CoffeeScript")]
        LiterateCoffeeScript,

        [Parameter(Value = "Literate Haskell")]
        LiterateHaskell,
        [Parameter(Value = "LiveScript")] LiveScript,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Llvm")]
        Llvm,
        Logos,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Logtalk")]
        Logtalk,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lua")]
        Lua,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "M")]
        M,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Makefile")]
        Makefile,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mako")]
        Mako,
        Markdown,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Matlab")]
        Matlab,
        Max,
        [Parameter(Value = "MiniD")] MiniD,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mirah")]
        Mirah,
        Monkey,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Moocode")]
        Moocode,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Moonscript")]
        Moonscript,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mupad")]
        Mupad,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Myghty")]
        Myghty,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nemerle")]
        Nemerle,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nginx")]
        Nginx,
        Nimrod,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Nsis")]
        Nsis,

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Nu")]
        Nu,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Num")]
        [Parameter(Value = "NumPY")]
        NumPY,
        [Parameter(Value = "ObjDump")] ObjDump,
        ObjectiveC,
        ObjectiveJ,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Caml")]
        [Parameter(Value = "OCaml")]
        OCaml,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Omgrofl")]
        Omgrofl,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ooc")]
        Ooc,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Opa")]
        Opa,
        [Parameter(Value = "OpenCL")] OpenCL,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Abl")]
        [Parameter(Value = "OpenEdge ABL")]
        OpenEdgeAbl,
        Parrot,
        [Parameter(Value = "Parrot Assembly")] ParrotAssembly,

        [Parameter(Value = "Parrot Internal Representation")]
        ParrotInternalRepresentation,
        Pascal,
        Perl,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Php")]
        Php,
        Pike,
        [Parameter(Value = "PogoScript")] PogoScript,
        [Parameter(Value = "PowerShell")] PowerShell,
        Processing,
        Prolog,
        Puppet,
        [Parameter(Value = "Pure Data")] PureData,
        Python,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Traceback")]
        [Parameter(Value = "Python traceback")]
        PythonTraceback,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "R")]
        R,
        Racket,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ragel")]
        [Parameter(Value = "Ragel in Ruby Host")]
        RagelInRubyHost,
        [Parameter(Value = "Raw token data")] RawTokenData,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rebol")]
        Rebol,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Redcode")]
        Redcode,

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ReStructured")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Re")]
        [Parameter(Value = "reStructuredText")]
        ReStructuredText,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rhtml")]
        Rhtml,
        Rouge,
        Ruby,
        Rust,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Scala")]
        Scala,
        Scheme,
        Sage,
        Sass,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Scilab")]
        Scilab,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Scss")]
        Scss,
        Self,
        Shell,
        Slash,
        Smalltalk,
        Smarty,
        Squirrel,
        [Parameter(Value = "Standard ML")] StandardML,

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "SuperCollider")]
        [Parameter(Value = "SuperCollider")]
        SuperCollider,
        [Parameter(Value = "Swift")] Swift,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tcl")]
        Tcl,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tcsh")]
        Tcsh,
        Tea,

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Te")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Te")]
        [Parameter(Value = "TeX")]
        TeX,
        Textile,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Toml")]
        Toml,
        Turing,
        Twig,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Txl")]
        Txl,

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TypeScript")]
        [Parameter(Value = "TypeScript")]
        TypeScript,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Parallel")]
        [Parameter(Value = "Unified Parallel C")]
        UnifiedParallelC,
        Unknown,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vala")]
        Vala,
        Verilog,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vhdl")]
        Vhdl,
        [Parameter(Value = "VimL")] VimL,
        VisualBasic,
        Volt,
        Wisp,

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Xc")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Xc")]
        Xc,
        Xml,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Proc")]
        [Parameter(Value = "XProc")]
        XProc,
        [Parameter(Value = "XQuery")] XQuery,

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Xs")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Xs")]
        Xs,
        Xslt,

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Xtend")]
        Xtend,
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Yaml")]
        Yaml
#pragma warning restore 1591
    }

    /// <summary>
    /// Licenses than can be searched on GitHub
    /// https://docs.github.com/search-github/searching-on-github/searching-for-repositories#search-by-license
    /// </summary>
    public enum RepoSearchLicense
    {
        [Parameter(Value = "afl-3.0")] AcademicFree_3_0,
        [Parameter(Value = "apache-2.0")] Apache_2_0,
        [Parameter(Value = "artistic-2.0")] Artistic_2_0,
        [Parameter(Value = "bsl-1.0")] BoostSoftware_1_0,
        [Parameter(Value = "0bsd")] BSD0Clause,
        [Parameter(Value = "bsd-2-clause")] BSD2Clause,
        [Parameter(Value = "bsd-3-clause")] BSD3Clause,

        [Parameter(Value = "bsd-3-clause-clear")]
        BSD3ClauseClear,
        [Parameter(Value = "bsd-4-clause")] BSD4Clause,
        [Parameter(Value = "cc")] CreativeCommons,
        [Parameter(Value = "cc0-1.0")] CreativeCommonsZero_1_0,
        [Parameter(Value = "cc-by-4.0")] CreativeCommonsAtrribution_4_0,
        [Parameter(Value = "cc-by-sa-4.0")] CreativeCommonsAttributionShareAlike_4_0,
        [Parameter(Value = "cecill-2.1")] CeCILL_2_1,
        [Parameter(Value = "wtfpl")] DoWhatTheFYouWant,
        [Parameter(Value = "ecl-2.0")] EducationalCommunity_2_0,
        [Parameter(Value = "epl-1.0")] EclipsePublic_1_0,
        [Parameter(Value = "epl-2.0")] EclipsePublic_2_0,
        [Parameter(Value = "eupl-1.1")] EuropeanUnionPublic_1_1,
        [Parameter(Value = "eupl-1.2")] EuropeanUnionPublic_1_2,
        [Parameter(Value = "agpl-3.0")] GNUAfferoGeneralPublic_3_0,
        [Parameter(Value = "gpl")] GNUGeneralPublic,
        [Parameter(Value = "gpl-2.0")] GNUGeneralPublic_2_0,
        [Parameter(Value = "gpl-3.0")] GNUGeneralPublic_3_0,
        [Parameter(Value = "lgpl")] GNULesserGeneralPublic,
        [Parameter(Value = "lgpl-2.1")] GNULesserGeneralPublic_2_1,
        [Parameter(Value = "lgpl-3.0")] GNULesserGeneralPublic_3_0,
        [Parameter(Value = "isc")] ISC,
        [Parameter(Value = "lppl-1.3c")] LatexProjectPublic,
        [Parameter(Value = "ms-pl")] MicrosoftPublic,
        [Parameter(Value = "ms-rl")] MicrosoftReciprocal,
        [Parameter(Value = "mit")] MIT,
        [Parameter(Value = "mpl-2.0")] MozillaPublic_2_0,
        [Parameter(Value = "odbl-1.0")] ODCOpenDatabase,
        [Parameter(Value = "osl-3.0")] OpenSoftware_3_0,
        [Parameter(Value = "postgresql")] PostgreseSQL,
        [Parameter(Value = "ofl-1.1")] SILOpenFont,
        [Parameter(Value = "upl-1.0")] UniversalPermissive,
        [Parameter(Value = "ncsa")] NSCAOpenSource,
        [Parameter(Value = "unlicense")] TheUnlicense,
        [Parameter(Value = "vim")] Vim,
        [Parameter(Value = "zlib")] ZLib
    }

    /// <summary>
    /// sorting repositories by any of below
    /// https://docs.github.com/rest/search/search?#search-repositories
    /// https://docs.github.com/search-github/getting-started-with-searching-on-github/sorting-search-results
    /// </summary>
    public enum RepoSearchSort
    {
        /// <summary>
        /// search by number of stars
        /// </summary>
        [Parameter(Value = "stars")] Stars,

        /// <summary>
        /// search by number of forks
        /// </summary>
        [Parameter(Value = "forks")] Forks,

        /// <summary>
        /// search by number of help-wanted-issues
        /// </summary>
        [Parameter(Value = "help-wanted-issues")]
        HelpWantedIssues,

        /// <summary>
        /// search by last updated
        /// </summary>
        [Parameter(Value = "updated")] Updated
    }

    /// <summary>
    /// https://docs.github.com/search-github/searching-on-github/searching-in-forks
    /// Specifying whether forked repositories should be included in results or not.
    /// </summary>
    public enum ForkQualifier
    {
        /// <summary>
        /// only search for forked repos
        /// </summary>
        [Parameter(Value = "only")] OnlyForks,

        /// <summary>
        /// include forked repos into the search
        /// </summary>
        [Parameter(Value = "true")] IncludeForks
    }
}