using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class Repository
    {
        public long Id { get; set; }
        public long? OwnerId { get; set; }
        public string LowerName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Website { get; set; }
        public string? DefaultBranch { get; set; }
        public long Size { get; set; }
        public bool? UseCustomAvatar { get; set; }
        public int? NumWatches { get; set; }
        public int? NumStars { get; set; }
        public int? NumForks { get; set; }
        public int? NumIssues { get; set; }
        public int? NumClosedIssues { get; set; }
        public int? NumPulls { get; set; }
        public int? NumClosedPulls { get; set; }
        public int NumMilestones { get; set; }
        public int NumClosedMilestones { get; set; }
        public bool? IsPrivate { get; set; }
        public bool IsUnlisted { get; set; }
        public bool? IsBare { get; set; }
        public bool? IsMirror { get; set; }
        public bool? EnableWiki { get; set; }
        public bool? AllowPublicWiki { get; set; }
        public bool? EnableExternalWiki { get; set; }
        public string? ExternalWikiUrl { get; set; }
        public bool? EnableIssues { get; set; }
        public bool? AllowPublicIssues { get; set; }
        public bool? EnableExternalTracker { get; set; }
        public string? ExternalTrackerUrl { get; set; }
        public string? ExternalTrackerFormat { get; set; }
        public string? ExternalTrackerStyle { get; set; }
        public bool? EnablePulls { get; set; }
        public bool PullsIgnoreWhitespace { get; set; }
        public bool PullsAllowRebase { get; set; }
        public bool IsFork { get; set; }
        public long? ForkId { get; set; }
        public long? CreatedUnix { get; set; }
        public long? UpdatedUnix { get; set; }
    }
}
