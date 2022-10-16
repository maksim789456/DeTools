using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class TwoFactorRecoveryCode
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string? Code { get; set; }
        public bool? IsUsed { get; set; }
    }
}
