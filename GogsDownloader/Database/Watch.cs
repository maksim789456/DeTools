using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Watch
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? RepoId { get; set; }
    }
}
