using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class OrgUser
    {
        public long Id { get; set; }
        public long? Uid { get; set; }
        public long? OrgId { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsOwner { get; set; }
        public int? NumTeams { get; set; }
    }
}
