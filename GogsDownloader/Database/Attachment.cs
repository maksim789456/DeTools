using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Attachment
    {
        public long Id { get; set; }
        public Guid? Uuid { get; set; }
        public long? IssueId { get; set; }
        public long? CommentId { get; set; }
        public long? ReleaseId { get; set; }
        public string? Name { get; set; }
        public long? CreatedUnix { get; set; }
    }
}
