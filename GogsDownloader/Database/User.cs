using System;
using System.Collections.Generic;

namespace GogsDownloader
{
    public partial class User
    {
        public long Id { get; set; }
        public string LowerName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? FullName { get; set; }
        public string Email { get; set; } = null!;
        public string Passwd { get; set; } = null!;
        public long LoginSource { get; set; }
        public string? LoginName { get; set; }
        public int? Type { get; set; }
        public string? Location { get; set; }
        public string? Website { get; set; }
        public string? Rands { get; set; }
        public string? Salt { get; set; }
        public long? CreatedUnix { get; set; }
        public long? UpdatedUnix { get; set; }
        public bool? LastRepoVisibility { get; set; }
        public int MaxRepoCreation { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? AllowGitHook { get; set; }
        public bool? AllowImportLocal { get; set; }
        public bool? ProhibitLogin { get; set; }
        public string Avatar { get; set; } = null!;
        public string AvatarEmail { get; set; } = null!;
        public bool? UseCustomAvatar { get; set; }
        public int? NumFollowers { get; set; }
        public int NumFollowing { get; set; }
        public int? NumStars { get; set; }
        public int? NumRepos { get; set; }
        public string? Description { get; set; }
        public int? NumTeams { get; set; }
        public int? NumMembers { get; set; }
    }
}
