using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class DeployKey
    {
        public long Id { get; set; }
        public long? KeyId { get; set; }
        public long? RepoId { get; set; }
        public string? Name { get; set; }
        public string? Fingerprint { get; set; }
        public long? CreatedUnix { get; set; }
        public long? UpdatedUnix { get; set; }
    }
}
