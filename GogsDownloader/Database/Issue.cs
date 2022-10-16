using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Issue
    {
        public long Id { get; set; }
        public long? RepoId { get; set; }
        public long? Index { get; set; }
        public long? PosterId { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public long? MilestoneId { get; set; }
        public int? Priority { get; set; }
        public long? AssigneeId { get; set; }
        public bool? IsClosed { get; set; }
        public bool? IsPull { get; set; }
        public int? NumComments { get; set; }
        public long? DeadlineUnix { get; set; }
        public long? CreatedUnix { get; set; }
        public long? UpdatedUnix { get; set; }
    }
}
