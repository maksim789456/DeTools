using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class PublicKey
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public string Name { get; set; } = null!;
        public string Fingerprint { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int Mode { get; set; }
        public int Type { get; set; }
        public long? CreatedUnix { get; set; }
        public long? UpdatedUnix { get; set; }
    }
}
