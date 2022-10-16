using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class LfsObject
    {
        public long RepoId { get; set; }
        public string Oid { get; set; } = null!;
        public long Size { get; set; }
        public string Storage { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
