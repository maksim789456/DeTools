using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class HookTask
    {
        public long Id { get; set; }
        public long? RepoId { get; set; }
        public long? HookId { get; set; }
        public string? Uuid { get; set; }
        public int? Type { get; set; }
        public string? Url { get; set; }
        public string? Signature { get; set; }
        public string? PayloadContent { get; set; }
        public int? ContentType { get; set; }
        public string? EventType { get; set; }
        public bool? IsSsl { get; set; }
        public bool? IsDelivered { get; set; }
        public long? Delivered { get; set; }
        public bool? IsSucceed { get; set; }
        public string? RequestContent { get; set; }
        public string? ResponseContent { get; set; }
    }
}
