using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class Label
    {
        public long Id { get; set; }
        public long? RepoId { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public int? NumIssues { get; set; }
        public int? NumClosedIssues { get; set; }
    }
}
