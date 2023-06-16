using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class Action
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? OpType { get; set; }
        public long? ActUserId { get; set; }
        public string? ActUserName { get; set; }
        public long? RepoId { get; set; }
        public string? RepoUserName { get; set; }
        public string? RepoName { get; set; }
        public string? RefName { get; set; }
        public bool IsPrivate { get; set; }
        public string? Content { get; set; }
        public long? CreatedUnix { get; set; }
    }
}
