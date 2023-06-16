using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class ProtectBranch
    {
        public long Id { get; set; }
        public long? RepoId { get; set; }
        public string? Name { get; set; }
        public bool? Protected { get; set; }
        public bool? RequirePullRequest { get; set; }
        public bool? EnableWhitelist { get; set; }
        public string? WhitelistUserIDs { get; set; }
        public string? WhitelistTeamIDs { get; set; }
    }
}
