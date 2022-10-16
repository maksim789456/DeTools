using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Notice
    {
        public long Id { get; set; }
        public int? Type { get; set; }
        public string? Description { get; set; }
        public long? CreatedUnix { get; set; }
    }
}
