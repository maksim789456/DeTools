using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Webhook
    {
        public long Id { get; set; }
        public long? RepoId { get; set; }
        public long? OrgId { get; set; }
        public string? Url { get; set; }
        public int? ContentType { get; set; }
        public string? Secret { get; set; }
        public string? Events { get; set; }
        public bool? IsSsl { get; set; }
        public bool? IsActive { get; set; }
        public int? HookTaskType { get; set; }
        public string? Meta { get; set; }
        public int? LastStatus { get; set; }
        public long? CreatedUnix { get; set; }
        public long? UpdatedUnix { get; set; }
    }
}
