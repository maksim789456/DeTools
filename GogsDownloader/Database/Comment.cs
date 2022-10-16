using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Comment
    {
        public long Id { get; set; }
        public int? Type { get; set; }
        public long? PosterId { get; set; }
        public long? IssueId { get; set; }
        public long? CommitId { get; set; }
        public long? Line { get; set; }
        public string? Content { get; set; }
        public long? CreatedUnix { get; set; }
        public long? UpdatedUnix { get; set; }
        public string? CommitSha { get; set; }
    }
}
