using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class TwoFactor
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string? Secret { get; set; }
        public long? CreatedUnix { get; set; }
    }
}
