using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class AccessToken
    {
        public long Id { get; set; }
        public long? Uid { get; set; }
        public string? Name { get; set; }
        public string? Sha1 { get; set; }
        public long? CreatedUnix { get; set; }
        public long? UpdatedUnix { get; set; }
    }
}
