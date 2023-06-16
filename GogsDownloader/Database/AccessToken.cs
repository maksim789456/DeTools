using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class AccessToken
    {
        public long Id { get; set; }
        public long? Uid { get; set; }
        public string? Name { get; set; }
        public string? Sha1 { get; set; }
        public string Sha256 { get; set; } = null!;
        public long? CreatedUnix { get; set; }
        public long? UpdatedUnix { get; set; }
    }
}
