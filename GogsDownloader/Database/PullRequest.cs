using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class PullRequest
    {
        public long Id { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
        public long? IssueId { get; set; }
        public long? Index { get; set; }
        public long? HeadRepoId { get; set; }
        public long? BaseRepoId { get; set; }
        public string? HeadUserName { get; set; }
        public string? HeadBranch { get; set; }
        public string? BaseBranch { get; set; }
        public string? MergeBase { get; set; }
        public bool? HasMerged { get; set; }
        public string? MergedCommitId { get; set; }
        public long? MergerId { get; set; }
        public long? MergedUnix { get; set; }
    }
}
