using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class TeamRepo
    {
        public long Id { get; set; }
        public long? OrgId { get; set; }
        public long? TeamId { get; set; }
        public long? RepoId { get; set; }
    }
}
