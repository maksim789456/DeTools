using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class IssueLabel
    {
        public long Id { get; set; }
        public long? IssueId { get; set; }
        public long? LabelId { get; set; }
    }
}
