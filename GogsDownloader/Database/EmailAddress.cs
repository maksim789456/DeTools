using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class EmailAddress
    {
        public long Id { get; set; }
        public long Uid { get; set; }
        public string Email { get; set; } = null!;
        public bool? IsActivated { get; set; }
    }
}
