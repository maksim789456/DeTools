using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Team
    {
        public long Id { get; set; }
        public long? OrgId { get; set; }
        public string? LowerName { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Authorize { get; set; }
        public int? NumRepos { get; set; }
        public int? NumMembers { get; set; }
    }
}
