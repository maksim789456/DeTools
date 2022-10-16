using Microsoft.EntityFrameworkCore;

namespace GogsDownloader.Database
{
    public partial class GogsDbContext : DbContext
    {
        private string _connectionString;
        public DatabaseType _databaseType;

        public GogsDbContext(string connectionString, DatabaseType dbType)
        {
            _connectionString = connectionString;
            _databaseType = dbType;
        }

        public GogsDbContext(DbContextOptions<GogsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Access> Accesses { get; set; } = null!;
        public virtual DbSet<AccessToken> AccessTokens { get; set; } = null!;
        public virtual DbSet<Action> Actions { get; set; } = null!;
        public virtual DbSet<Attachment> Attachments { get; set; } = null!;
        public virtual DbSet<Collaboration> Collaborations { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<DeployKey> DeployKeys { get; set; } = null!;
        public virtual DbSet<EmailAddress> EmailAddresses { get; set; } = null!;
        public virtual DbSet<Follow> Follows { get; set; } = null!;
        public virtual DbSet<HookTask> HookTasks { get; set; } = null!;
        public virtual DbSet<Issue> Issues { get; set; } = null!;
        public virtual DbSet<IssueLabel> IssueLabels { get; set; } = null!;
        public virtual DbSet<IssueUser> IssueUsers { get; set; } = null!;
        public virtual DbSet<Label> Labels { get; set; } = null!;
        public virtual DbSet<LfsObject> LfsObjects { get; set; } = null!;
        public virtual DbSet<LoginSource> LoginSources { get; set; } = null!;
        public virtual DbSet<Milestone> Milestones { get; set; } = null!;
        public virtual DbSet<Mirror> Mirrors { get; set; } = null!;
        public virtual DbSet<Notice> Notices { get; set; } = null!;
        public virtual DbSet<OrgUser> OrgUsers { get; set; } = null!;
        public virtual DbSet<ProtectBranch> ProtectBranches { get; set; } = null!;
        public virtual DbSet<ProtectBranchWhitelist> ProtectBranchWhitelists { get; set; } = null!;
        public virtual DbSet<PublicKey> PublicKeys { get; set; } = null!;
        public virtual DbSet<PullRequest> PullRequests { get; set; } = null!;
        public virtual DbSet<Release> Releases { get; set; } = null!;
        public virtual DbSet<Repository> Repositories { get; set; } = null!;
        public virtual DbSet<Star> Stars { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<TeamRepo> TeamRepos { get; set; } = null!;
        public virtual DbSet<TeamUser> TeamUsers { get; set; } = null!;
        public virtual DbSet<TwoFactor> TwoFactors { get; set; } = null!;
        public virtual DbSet<TwoFactorRecoveryCode> TwoFactorRecoveryCodes { get; set; } = null!;
        public virtual DbSet<Upload> Uploads { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Version> Versions { get; set; } = null!;
        public virtual DbSet<Watch> Watches { get; set; } = null!;
        public virtual DbSet<Webhook> Webhooks { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                switch (_databaseType)
                {
                    case DatabaseType.Postgre:
                        optionsBuilder.UseNpgsql(_connectionString);
                        break;
                    case DatabaseType.MySql:
                        optionsBuilder.UseMySql(ServerVersion.AutoDetect(_connectionString));
                        break;
                    case DatabaseType.Sqlite:
                        optionsBuilder.UseSqlite(_connectionString);
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Access>(entity =>
            {
                entity.ToTable("access");

                entity.HasIndex(e => new { e.UserId, e.RepoId }, "UQE_access_s")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Mode).HasColumnName("mode");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<AccessToken>(entity =>
            {
                entity.ToTable("access_token");

                entity.HasIndex(e => e.Sha1, "access_token_sha1_key")
                    .IsUnique();

                entity.HasIndex(e => e.Uid, "idx_access_token_uid");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Sha1)
                    .HasMaxLength(40)
                    .HasColumnName("sha1");

                entity.Property(e => e.Uid).HasColumnName("uid");

                entity.Property(e => e.UpdatedUnix).HasColumnName("updated_unix");
            });

            modelBuilder.Entity<Action>(entity =>
            {
                entity.ToTable("action");

                entity.HasIndex(e => e.RepoId, "IDX_action_repo_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActUserId).HasColumnName("act_user_id");

                entity.Property(e => e.ActUserName)
                    .HasMaxLength(255)
                    .HasColumnName("act_user_name");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.IsPrivate).HasColumnName("is_private");

                entity.Property(e => e.OpType).HasColumnName("op_type");

                entity.Property(e => e.RefName)
                    .HasMaxLength(255)
                    .HasColumnName("ref_name");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.RepoName)
                    .HasMaxLength(255)
                    .HasColumnName("repo_name");

                entity.Property(e => e.RepoUserName)
                    .HasMaxLength(255)
                    .HasColumnName("repo_user_name");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.ToTable("attachment");

                entity.HasIndex(e => e.IssueId, "IDX_attachment_issue_id");

                entity.HasIndex(e => e.ReleaseId, "IDX_attachment_release_id");

                entity.HasIndex(e => e.Uuid, "UQE_attachment_uuid")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.ReleaseId).HasColumnName("release_id");

                entity.Property(e => e.Uuid).HasColumnName("uuid");
            });

            modelBuilder.Entity<Collaboration>(entity =>
            {
                entity.ToTable("collaboration");

                entity.HasIndex(e => e.RepoId, "IDX_collaboration_repo_id");

                entity.HasIndex(e => e.UserId, "IDX_collaboration_user_id");

                entity.HasIndex(e => new { e.RepoId, e.UserId }, "UQE_collaboration_s")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Mode)
                    .HasColumnName("mode")
                    .HasDefaultValueSql("2");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.HasIndex(e => e.IssueId, "IDX_comment_issue_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommitId).HasColumnName("commit_id");

                entity.Property(e => e.CommitSha)
                    .HasMaxLength(40)
                    .HasColumnName("commit_sha");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.Line).HasColumnName("line");

                entity.Property(e => e.PosterId).HasColumnName("poster_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdatedUnix).HasColumnName("updated_unix");
            });

            modelBuilder.Entity<DeployKey>(entity =>
            {
                entity.ToTable("deploy_key");

                entity.HasIndex(e => e.KeyId, "IDX_deploy_key_key_id");

                entity.HasIndex(e => e.RepoId, "IDX_deploy_key_repo_id");

                entity.HasIndex(e => new { e.KeyId, e.RepoId }, "UQE_deploy_key_s")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.Fingerprint)
                    .HasMaxLength(255)
                    .HasColumnName("fingerprint");

                entity.Property(e => e.KeyId).HasColumnName("key_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.UpdatedUnix).HasColumnName("updated_unix");
            });

            modelBuilder.Entity<EmailAddress>(entity =>
            {
                entity.ToTable("email_address");

                entity.HasIndex(e => e.Uid, "IDX_email_address_uid");

                entity.HasIndex(e => e.Email, "UQE_email_address_email")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.IsActivated).HasColumnName("is_activated");

                entity.Property(e => e.Uid).HasColumnName("uid");
            });

            modelBuilder.Entity<Follow>(entity =>
            {
                entity.ToTable("follow");

                entity.HasIndex(e => new { e.UserId, e.FollowId }, "UQE_follow_follow")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FollowId).HasColumnName("follow_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<HookTask>(entity =>
            {
                entity.ToTable("hook_task");

                entity.HasIndex(e => e.RepoId, "IDX_hook_task_repo_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ContentType).HasColumnName("content_type");

                entity.Property(e => e.Delivered).HasColumnName("delivered");

                entity.Property(e => e.EventType)
                    .HasMaxLength(255)
                    .HasColumnName("event_type");

                entity.Property(e => e.HookId).HasColumnName("hook_id");

                entity.Property(e => e.IsDelivered).HasColumnName("is_delivered");

                entity.Property(e => e.IsSsl).HasColumnName("is_ssl");

                entity.Property(e => e.IsSucceed).HasColumnName("is_succeed");

                entity.Property(e => e.PayloadContent).HasColumnName("payload_content");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.RequestContent).HasColumnName("request_content");

                entity.Property(e => e.ResponseContent).HasColumnName("response_content");

                entity.Property(e => e.Signature).HasColumnName("signature");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Url).HasColumnName("url");

                entity.Property(e => e.Uuid)
                    .HasMaxLength(255)
                    .HasColumnName("uuid");
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.ToTable("issue");

                entity.HasIndex(e => e.RepoId, "IDX_issue_repo_id");

                entity.HasIndex(e => new { e.RepoId, e.Index }, "UQE_issue_repo_index")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AssigneeId).HasColumnName("assignee_id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.DeadlineUnix).HasColumnName("deadline_unix");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.IsClosed).HasColumnName("is_closed");

                entity.Property(e => e.IsPull).HasColumnName("is_pull");

                entity.Property(e => e.MilestoneId).HasColumnName("milestone_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.NumComments).HasColumnName("num_comments");

                entity.Property(e => e.PosterId).HasColumnName("poster_id");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.UpdatedUnix).HasColumnName("updated_unix");
            });

            modelBuilder.Entity<IssueLabel>(entity =>
            {
                entity.ToTable("issue_label");

                entity.HasIndex(e => new { e.IssueId, e.LabelId }, "UQE_issue_label_s")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.LabelId).HasColumnName("label_id");
            });

            modelBuilder.Entity<IssueUser>(entity =>
            {
                entity.ToTable("issue_user");

                entity.HasIndex(e => e.RepoId, "IDX_issue_user_repo_id");

                entity.HasIndex(e => e.Uid, "IDX_issue_user_uid");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsAssigned).HasColumnName("is_assigned");

                entity.Property(e => e.IsClosed).HasColumnName("is_closed");

                entity.Property(e => e.IsMentioned).HasColumnName("is_mentioned");

                entity.Property(e => e.IsPoster).HasColumnName("is_poster");

                entity.Property(e => e.IsRead).HasColumnName("is_read");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.MilestoneId).HasColumnName("milestone_id");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.Uid).HasColumnName("uid");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.ToTable("label");

                entity.HasIndex(e => e.RepoId, "IDX_label_repo_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Color)
                    .HasMaxLength(7)
                    .HasColumnName("color");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.NumClosedIssues).HasColumnName("num_closed_issues");

                entity.Property(e => e.NumIssues).HasColumnName("num_issues");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");
            });

            modelBuilder.Entity<LfsObject>(entity =>
            {
                entity.HasKey(e => new { e.RepoId, e.Oid })
                    .HasName("lfs_object_pkey");

                entity.ToTable("lfs_object");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.Oid).HasColumnName("oid");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Size).HasColumnName("size");

                entity.Property(e => e.Storage).HasColumnName("storage");
            });

            modelBuilder.Entity<LoginSource>(entity =>
            {
                entity.ToTable("login_source");

                entity.HasIndex(e => e.Name, "login_source_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cfg).HasColumnName("cfg");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.IsActived).HasColumnName("is_actived");

                entity.Property(e => e.IsDefault).HasColumnName("is_default");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdatedUnix).HasColumnName("updated_unix");
            });

            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.ToTable("milestone");

                entity.HasIndex(e => e.RepoId, "IDX_milestone_repo_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClosedDateUnix).HasColumnName("closed_date_unix");

                entity.Property(e => e.Completeness).HasColumnName("completeness");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.DeadlineUnix).HasColumnName("deadline_unix");

                entity.Property(e => e.IsClosed).HasColumnName("is_closed");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.NumClosedIssues).HasColumnName("num_closed_issues");

                entity.Property(e => e.NumIssues).HasColumnName("num_issues");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");
            });

            modelBuilder.Entity<Mirror>(entity =>
            {
                entity.ToTable("mirror");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EnablePrune)
                    .IsRequired()
                    .HasColumnName("enable_prune")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Interval).HasColumnName("interval");

                entity.Property(e => e.NextUpdateUnix).HasColumnName("next_update_unix");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.UpdatedUnix).HasColumnName("updated_unix");
            });

            modelBuilder.Entity<Notice>(entity =>
            {
                entity.ToTable("notice");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<OrgUser>(entity =>
            {
                entity.ToTable("org_user");

                entity.HasIndex(e => e.OrgId, "IDX_org_user_org_id");

                entity.HasIndex(e => e.Uid, "IDX_org_user_uid");

                entity.HasIndex(e => new { e.Uid, e.OrgId }, "UQE_org_user_s")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsOwner).HasColumnName("is_owner");

                entity.Property(e => e.IsPublic).HasColumnName("is_public");

                entity.Property(e => e.NumTeams).HasColumnName("num_teams");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.Uid).HasColumnName("uid");
            });

            modelBuilder.Entity<ProtectBranch>(entity =>
            {
                entity.ToTable("protect_branch");

                entity.HasIndex(e => new { e.RepoId, e.Name }, "UQE_protect_branch_protect_branch")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EnableWhitelist).HasColumnName("enable_whitelist");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Protected).HasColumnName("protected");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.RequirePullRequest).HasColumnName("require_pull_request");

                entity.Property(e => e.WhitelistTeamIDs).HasColumnName("whitelist_team_i_ds");

                entity.Property(e => e.WhitelistUserIDs).HasColumnName("whitelist_user_i_ds");
            });

            modelBuilder.Entity<ProtectBranchWhitelist>(entity =>
            {
                entity.ToTable("protect_branch_whitelist");

                entity.HasIndex(e => new { e.RepoId, e.Name, e.UserId }, "UQE_protect_branch_whitelist_protect_branch_whitelist")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.ProtectBranchId).HasColumnName("protect_branch_id");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<PublicKey>(entity =>
            {
                entity.ToTable("public_key");

                entity.HasIndex(e => e.OwnerId, "IDX_public_key_owner_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content).HasColumnName("content");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.Fingerprint)
                    .HasMaxLength(255)
                    .HasColumnName("fingerprint");

                entity.Property(e => e.Mode)
                    .HasColumnName("mode")
                    .HasDefaultValueSql("2");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.UpdatedUnix).HasColumnName("updated_unix");
            });

            modelBuilder.Entity<PullRequest>(entity =>
            {
                entity.ToTable("pull_request");

                entity.HasIndex(e => e.IssueId, "IDX_pull_request_issue_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BaseBranch)
                    .HasMaxLength(255)
                    .HasColumnName("base_branch");

                entity.Property(e => e.BaseRepoId).HasColumnName("base_repo_id");

                entity.Property(e => e.HasMerged).HasColumnName("has_merged");

                entity.Property(e => e.HeadBranch)
                    .HasMaxLength(255)
                    .HasColumnName("head_branch");

                entity.Property(e => e.HeadRepoId).HasColumnName("head_repo_id");

                entity.Property(e => e.HeadUserName)
                    .HasMaxLength(255)
                    .HasColumnName("head_user_name");

                entity.Property(e => e.Index).HasColumnName("index");

                entity.Property(e => e.IssueId).HasColumnName("issue_id");

                entity.Property(e => e.MergeBase)
                    .HasMaxLength(40)
                    .HasColumnName("merge_base");

                entity.Property(e => e.MergedCommitId)
                    .HasMaxLength(40)
                    .HasColumnName("merged_commit_id");

                entity.Property(e => e.MergedUnix).HasColumnName("merged_unix");

                entity.Property(e => e.MergerId).HasColumnName("merger_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Release>(entity =>
            {
                entity.ToTable("release");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.IsDraft).HasColumnName("is_draft");

                entity.Property(e => e.IsPrerelease).HasColumnName("is_prerelease");

                entity.Property(e => e.LowerTagName)
                    .HasMaxLength(255)
                    .HasColumnName("lower_tag_name");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.NumCommits).HasColumnName("num_commits");

                entity.Property(e => e.PublisherId).HasColumnName("publisher_id");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.Sha1)
                    .HasMaxLength(40)
                    .HasColumnName("sha1");

                entity.Property(e => e.TagName)
                    .HasMaxLength(255)
                    .HasColumnName("tag_name");

                entity.Property(e => e.Target)
                    .HasMaxLength(255)
                    .HasColumnName("target");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Repository>(entity =>
            {
                entity.ToTable("repository");

                entity.HasIndex(e => e.LowerName, "IDX_repository_lower_name");

                entity.HasIndex(e => e.Name, "IDX_repository_name");

                entity.HasIndex(e => new { e.OwnerId, e.LowerName }, "UQE_repository_s")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AllowPublicIssues).HasColumnName("allow_public_issues");

                entity.Property(e => e.AllowPublicWiki).HasColumnName("allow_public_wiki");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.DefaultBranch)
                    .HasMaxLength(255)
                    .HasColumnName("default_branch");

                entity.Property(e => e.Description)
                    .HasMaxLength(512)
                    .HasColumnName("description");

                entity.Property(e => e.EnableExternalTracker).HasColumnName("enable_external_tracker");

                entity.Property(e => e.EnableExternalWiki).HasColumnName("enable_external_wiki");

                entity.Property(e => e.EnableIssues)
                    .IsRequired()
                    .HasColumnName("enable_issues")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.EnablePulls)
                    .IsRequired()
                    .HasColumnName("enable_pulls")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.EnableWiki)
                    .IsRequired()
                    .HasColumnName("enable_wiki")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.ExternalTrackerFormat)
                    .HasMaxLength(255)
                    .HasColumnName("external_tracker_format");

                entity.Property(e => e.ExternalTrackerStyle)
                    .HasMaxLength(255)
                    .HasColumnName("external_tracker_style");

                entity.Property(e => e.ExternalTrackerUrl)
                    .HasMaxLength(255)
                    .HasColumnName("external_tracker_url");

                entity.Property(e => e.ExternalWikiUrl)
                    .HasMaxLength(255)
                    .HasColumnName("external_wiki_url");

                entity.Property(e => e.ForkId).HasColumnName("fork_id");

                entity.Property(e => e.IsBare).HasColumnName("is_bare");

                entity.Property(e => e.IsFork).HasColumnName("is_fork");

                entity.Property(e => e.IsMirror).HasColumnName("is_mirror");

                entity.Property(e => e.IsPrivate).HasColumnName("is_private");

                entity.Property(e => e.LowerName)
                    .HasMaxLength(255)
                    .HasColumnName("lower_name");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.NumClosedIssues).HasColumnName("num_closed_issues");

                entity.Property(e => e.NumClosedMilestones).HasColumnName("num_closed_milestones");

                entity.Property(e => e.NumClosedPulls).HasColumnName("num_closed_pulls");

                entity.Property(e => e.NumForks).HasColumnName("num_forks");

                entity.Property(e => e.NumIssues).HasColumnName("num_issues");

                entity.Property(e => e.NumMilestones).HasColumnName("num_milestones");

                entity.Property(e => e.NumPulls).HasColumnName("num_pulls");

                entity.Property(e => e.NumStars).HasColumnName("num_stars");

                entity.Property(e => e.NumWatches).HasColumnName("num_watches");

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.Property(e => e.PullsAllowRebase).HasColumnName("pulls_allow_rebase");

                entity.Property(e => e.PullsIgnoreWhitespace).HasColumnName("pulls_ignore_whitespace");

                entity.Property(e => e.Size).HasColumnName("size");

                entity.Property(e => e.UpdatedUnix).HasColumnName("updated_unix");

                entity.Property(e => e.UseCustomAvatar).HasColumnName("use_custom_avatar");

                entity.Property(e => e.Website)
                    .HasMaxLength(255)
                    .HasColumnName("website");
            });

            modelBuilder.Entity<Star>(entity =>
            {
                entity.ToTable("star");

                entity.HasIndex(e => new { e.Uid, e.RepoId }, "UQE_star_s")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.Uid).HasColumnName("uid");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("team");

                entity.HasIndex(e => e.OrgId, "IDX_team_org_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Authorize).HasColumnName("authorize");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.LowerName)
                    .HasMaxLength(255)
                    .HasColumnName("lower_name");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.NumMembers).HasColumnName("num_members");

                entity.Property(e => e.NumRepos).HasColumnName("num_repos");

                entity.Property(e => e.OrgId).HasColumnName("org_id");
            });

            modelBuilder.Entity<TeamRepo>(entity =>
            {
                entity.ToTable("team_repo");

                entity.HasIndex(e => e.OrgId, "IDX_team_repo_org_id");

                entity.HasIndex(e => new { e.TeamId, e.RepoId }, "UQE_team_repo_s")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.TeamId).HasColumnName("team_id");
            });

            modelBuilder.Entity<TeamUser>(entity =>
            {
                entity.ToTable("team_user");

                entity.HasIndex(e => e.OrgId, "IDX_team_user_org_id");

                entity.HasIndex(e => new { e.TeamId, e.Uid }, "UQE_team_user_s")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.TeamId).HasColumnName("team_id");

                entity.Property(e => e.Uid).HasColumnName("uid");
            });

            modelBuilder.Entity<TwoFactor>(entity =>
            {
                entity.ToTable("two_factor");

                entity.HasIndex(e => e.UserId, "UQE_two_factor_user_id")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.Secret)
                    .HasMaxLength(255)
                    .HasColumnName("secret");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<TwoFactorRecoveryCode>(entity =>
            {
                entity.ToTable("two_factor_recovery_code");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(11)
                    .HasColumnName("code");

                entity.Property(e => e.IsUsed).HasColumnName("is_used");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Upload>(entity =>
            {
                entity.ToTable("upload");

                entity.HasIndex(e => e.Uuid, "UQE_upload_uuid")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Uuid).HasColumnName("uuid");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.LowerName, "UQE_user_lower_name")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "UQE_user_name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AllowGitHook).HasColumnName("allow_git_hook");

                entity.Property(e => e.AllowImportLocal).HasColumnName("allow_import_local");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(2048)
                    .HasColumnName("avatar");

                entity.Property(e => e.AvatarEmail)
                    .HasMaxLength(255)
                    .HasColumnName("avatar_email");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("full_name");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsAdmin).HasColumnName("is_admin");

                entity.Property(e => e.LastRepoVisibility).HasColumnName("last_repo_visibility");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .HasColumnName("location");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(255)
                    .HasColumnName("login_name");

                entity.Property(e => e.LoginSource).HasColumnName("login_source");

                entity.Property(e => e.LowerName)
                    .HasMaxLength(255)
                    .HasColumnName("lower_name");

                entity.Property(e => e.MaxRepoCreation)
                    .HasColumnName("max_repo_creation")
                    .HasDefaultValueSql("'-1'::integer");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.NumFollowers).HasColumnName("num_followers");

                entity.Property(e => e.NumFollowing).HasColumnName("num_following");

                entity.Property(e => e.NumMembers).HasColumnName("num_members");

                entity.Property(e => e.NumRepos).HasColumnName("num_repos");

                entity.Property(e => e.NumStars).HasColumnName("num_stars");

                entity.Property(e => e.NumTeams).HasColumnName("num_teams");

                entity.Property(e => e.Passwd)
                    .HasMaxLength(255)
                    .HasColumnName("passwd");

                entity.Property(e => e.ProhibitLogin).HasColumnName("prohibit_login");

                entity.Property(e => e.Rands)
                    .HasMaxLength(10)
                    .HasColumnName("rands");

                entity.Property(e => e.Salt)
                    .HasMaxLength(10)
                    .HasColumnName("salt");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdatedUnix).HasColumnName("updated_unix");

                entity.Property(e => e.UseCustomAvatar).HasColumnName("use_custom_avatar");

                entity.Property(e => e.Website)
                    .HasMaxLength(255)
                    .HasColumnName("website");
            });

            modelBuilder.Entity<Version>(entity =>
            {
                entity.ToTable("version");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Version1).HasColumnName("version");
            });

            modelBuilder.Entity<Watch>(entity =>
            {
                entity.ToTable("watch");

                entity.HasIndex(e => new { e.UserId, e.RepoId }, "UQE_watch_watch")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<Webhook>(entity =>
            {
                entity.ToTable("webhook");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ContentType).HasColumnName("content_type");

                entity.Property(e => e.CreatedUnix).HasColumnName("created_unix");

                entity.Property(e => e.Events).HasColumnName("events");

                entity.Property(e => e.HookTaskType).HasColumnName("hook_task_type");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsSsl).HasColumnName("is_ssl");

                entity.Property(e => e.LastStatus).HasColumnName("last_status");

                entity.Property(e => e.Meta).HasColumnName("meta");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.RepoId).HasColumnName("repo_id");

                entity.Property(e => e.Secret).HasColumnName("secret");

                entity.Property(e => e.UpdatedUnix).HasColumnName("updated_unix");

                entity.Property(e => e.Url).HasColumnName("url");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
