using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Collaboration
    {
        public long Id { get; set; }
        public long RepoId { get; set; }
        public long UserId { get; set; }
        public int Mode { get; set; }
    }
}
