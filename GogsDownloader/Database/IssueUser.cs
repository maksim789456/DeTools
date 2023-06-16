using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class IssueUser
    {
        public long Id { get; set; }
        public long? Uid { get; set; }
        public long? IssueId { get; set; }
        public long? RepoId { get; set; }
        public long? MilestoneId { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsAssigned { get; set; }
        public bool? IsMentioned { get; set; }
        public bool? IsPoster { get; set; }
        public bool? IsClosed { get; set; }
    }
}
