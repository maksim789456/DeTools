using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Follow
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? FollowId { get; set; }
    }
}
