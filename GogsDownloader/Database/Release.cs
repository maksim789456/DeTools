using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Release
    {
        public long Id { get; set; }
        public long? RepoId { get; set; }
        public long? PublisherId { get; set; }
        public string? TagName { get; set; }
        public string? LowerTagName { get; set; }
        public string? Target { get; set; }
        public string? Title { get; set; }
        public string? Sha1 { get; set; }
        public long? NumCommits { get; set; }
        public string? Note { get; set; }
        public bool IsDraft { get; set; }
        public bool? IsPrerelease { get; set; }
        public long? CreatedUnix { get; set; }
    }
}
