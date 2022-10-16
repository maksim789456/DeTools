using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Star
    {
        public long Id { get; set; }
        public long? Uid { get; set; }
        public long? RepoId { get; set; }
    }
}
