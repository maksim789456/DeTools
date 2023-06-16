using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class Milestone
    {
        public long Id { get; set; }
        public long? RepoId { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public bool? IsClosed { get; set; }
        public int? NumIssues { get; set; }
        public int? NumClosedIssues { get; set; }
        public int? Completeness { get; set; }
        public long? DeadlineUnix { get; set; }
        public long? ClosedDateUnix { get; set; }
    }
}
