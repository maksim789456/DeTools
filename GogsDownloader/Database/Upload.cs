using System;
using System.Collections.Generic;

namespace GogsDownloader.Database
{
    public partial class Upload
    {
        public long Id { get; set; }
        public Guid? Uuid { get; set; }
        public string? Name { get; set; }
    }
}
