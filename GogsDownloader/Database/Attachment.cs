using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class Attachment
    {
        public long Id { get; set; }
        public string? Uuid { get; set; }
        public long? IssueId { get; set; }
        public long? CommentId { get; set; }
        public long? ReleaseId { get; set; }
        public string? Name { get; set; }
        public long? CreatedUnix { get; set; }
    }
}
