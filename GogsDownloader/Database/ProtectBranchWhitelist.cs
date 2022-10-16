using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class ProtectBranchWhitelist
    {
        public long Id { get; set; }
        public long? ProtectBranchId { get; set; }
        public long? RepoId { get; set; }
        public string? Name { get; set; }
        public long? UserId { get; set; }
    }
}
