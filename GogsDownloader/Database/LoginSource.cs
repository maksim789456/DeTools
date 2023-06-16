using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class LoginSource
    {
        public long Id { get; set; }
        public long? Type { get; set; }
        public string? Name { get; set; }
        public bool IsActived { get; set; }
        public bool? IsDefault { get; set; }
        public string? Cfg { get; set; }
        public long? CreatedUnix { get; set; }
        public long? UpdatedUnix { get; set; }
    }
}
