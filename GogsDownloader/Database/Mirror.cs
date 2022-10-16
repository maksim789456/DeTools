using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Mirror
    {
        public long Id { get; set; }
        public long? RepoId { get; set; }
        public int? Interval { get; set; }
        public bool? EnablePrune { get; set; }
        public long? UpdatedUnix { get; set; }
        public long? NextUpdateUnix { get; set; }
    }
}
