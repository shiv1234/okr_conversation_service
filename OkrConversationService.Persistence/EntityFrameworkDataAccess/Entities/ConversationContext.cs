using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OkrConversationService.Domain.Common;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public partial class ConversationContext : DbContext
    {
        private readonly HttpContext _httpContext;
        private readonly IConfiguration Configuration;
        private readonly ILogger<ConversationContext> _logger;
        public ConversationContext()
        {
        }

        public ConversationContext(DbContextOptions<ConversationContext> options, IConfiguration configuration, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor = null)
            : base(options)
        {
            _httpContext = httpContextAccessor?.HttpContext;
            Configuration = configuration;
            _logger = loggerFactory.CreateLogger<ConversationContext>();
        }

        public virtual DbSet<ApplicationMaster> ApplicationMasters { get; set; }
        public virtual DbSet<AssignmentTypeMaster> AssignmentTypeMasters { get; set; }
        public virtual DbSet<BlockedDomain> BlockedDomains { get; set; }
        public virtual DbSet<CheckInDetail> CheckInDetails { get; set; }
        public virtual DbSet<CheckInPoint> CheckInPoints { get; set; }
        public virtual DbSet<ColorCode> ColorCodes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ConfidenceAudit> ConfidenceAudits { get; set; }
        public virtual DbSet<ConfidenceMaster> ConfidenceMasters { get; set; }
        public virtual DbSet<Constant> Constants { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
        public virtual DbSet<EmployeeTag> EmployeeTag { get; set; }
        public virtual DbSet<ConversationFile> ConversationFiles { get; set; }
        public virtual DbSet<ConversationLog> ConversationLogs { get; set; }
        public virtual DbSet<LikeReaction> ConversationReactions { get; set; }
        public virtual DbSet<CriteriaFeedbackMapping> CriteriaFeedbackMappings { get; set; }
        public virtual DbSet<CriteriaMaster> CriteriaMasters { get; set; }
        public virtual DbSet<CriteriaTypeMaster> CriteriaTypeMasters { get; set; }
        public virtual DbSet<Cycle> Cycles { get; set; }
        public virtual DbSet<CycleSymbol> CycleSymbols { get; set; }
        public virtual DbSet<DirectReporteesFilter> DirectReporteesFilters { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeContactDetail> EmployeeContactDetails { get; set; }
        public virtual DbSet<EmployeeEngagement> EmployeeEngagements { get; set; }
        public virtual DbSet<EmployeeTeamMapping> EmployeeTeamMappings { get; set; }
        public virtual DbSet<EmployeeOkrProgress> EmployeeOkrProgresses { get; set; }
        public virtual DbSet<EmployeePermissionMapping> EmployeePermissionMappings { get; set; }
        public virtual DbSet<EmployeeProgressNature> EmployeeProgressNatures { get; set; }
        public virtual DbSet<EmployeeRoleMapping> EmployeeRoleMappings { get; set; }
        
        public virtual DbSet<EngagementType> EngagementTypes { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
          public virtual DbSet<FeedbackDetail> FeedbackDetails { get; set; }
        public virtual DbSet<FeedbackOnTypeMaster> FeedbackOnTypeMasters { get; set; }
        public virtual DbSet<FeedbackRequest> FeedbackRequests { get; set; }
        public virtual DbSet<GoalKey> GoalKeys { get; set; }
        public virtual DbSet<GoalKeyAudit> GoalKeyAudits { get; set; }
        public virtual DbSet<GoalKeyHistory> GoalKeyHistories { get; set; }
        public virtual DbSet<GoalObjective> GoalObjectives { get; set; }
        public virtual DbSet<GoalSequence> GoalSequences { get; set; }
        public virtual DbSet<GoalStatusMaster> GoalStatusMasters { get; set; }
        public virtual DbSet<GoalTypeMaster> GoalTypeMasters { get; set; }
        public virtual DbSet<GuidedTour> GuidedTours { get; set; }
        public virtual DbSet<GuidedTourControl> GuidedTourControls { get; set; }
        public virtual DbSet<ImpersonationActivities> ImpersonationActivities { get; set; }
        public virtual DbSet<ImpersonationLog> ImpersonationLog { get; set; }
        public virtual DbSet<JobsAudit> JobsAudits { get; set; }
        public virtual DbSet<KrStatusMaster> KrStatusMasters { get; set; }
        public virtual DbSet<KrStatusMessage> KrStatusMessages { get; set; }
        public virtual DbSet<Mail> Mail { get; set; }
        public virtual DbSet<MailAddressType> MailAddressTypes { get; set; }
        public virtual DbSet<MailSentLog> MailSentLogs { get; set; }
        public virtual DbSet<MailSetupConfig> MailSetupConfigs { get; set; }
        public virtual DbSet<MailerTemplate> MailerTemplates { get; set; }
        public virtual DbSet<MessageMaster> MessageMasters { get; set; }
        public virtual DbSet<MessageType> MessageTypes { get; set; }
        public virtual DbSet<MetricDataMaster> MetricDataMasters { get; set; }
        public virtual DbSet<MetricMaster> MetricMasters { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<NoteEmployeeTag> NoteEmployeeTags { get; set; }
        public virtual DbSet<NoteFile> NoteFiles { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<NotificationsDetail> NotificationsDetails { get; set; }
        public virtual DbSet<OkrAutoSubmitLog> OkrAutoSubmitLogs { get; set; }
        public virtual DbSet<OkrNatureMaster> OkrNatureMasters { get; set; }
        public virtual DbSet<OkrTypeFilter> OkrTypeFilters { get; set; }
        public virtual DbSet<OkrstatusMaster> OkrstatusMasters { get; set; }
        public virtual DbSet<OnBoardingControl> OnBoardingControls { get; set; }
        public virtual DbSet<OnBoardingScreen> OnBoardingScreens { get; set; }
        public virtual DbSet<OneToOneDetail> OneToOneDetails { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<ProgressAudit> ProgressAudits { get; set; }
        public virtual DbSet<RaisedTypeMaster> RaisedTypeMasters { get; set; }
        public virtual DbSet<RequestMaster> RequestMasters { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermissionMapping> RolePermissionMappings { get; set; }
        public virtual DbSet<StatusMaster> StatusMasters { get; set; }
        public virtual DbSet<TaskDetail> TaskDetails { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamCycle> TeamCycles { get; set; }
        public virtual DbSet<TeamCycleDetail> TeamCycleDetails { get; set; }
        public virtual DbSet<TeamProgress> TeamProgresses { get; set; }
        public virtual DbSet<TeamProgressNature> TeamProgressNatures { get; set; }
        public virtual DbSet<TeamSequence> TeamSequences { get; set; }
        public virtual DbSet<TeamSetting> TeamSettings { get; set; }
        public virtual DbSet<TypeOfGoalCreation> TypeOfGoalCreations { get; set; }
        public virtual DbSet<UnLockLog> UnLockLogs { get; set; }
        public virtual DbSet<UnlockSupportTeam> UnlockSupportTeams { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
        public virtual DbSet<WeightMaster> WeightMasters { get; set; }
        public virtual DbSet<Recognition> Recognition { get; set; }
        public virtual DbSet<RecognitionCategory> RecognitionCategory { get; set; }
        public virtual DbSet<RecognitionImageMapping> RecognitionImageMapping { get; set; }
        public virtual DbSet<CommentDetails> CommentDetails { get; set; }
        public virtual DbSet<RecognitionEmployeeTeamMapping> RecognitionEmployeeTeamMapping { get; set; }
        public virtual DbSet<CheckInEmployeeMapping> CheckInEmployeeMappings { get; set; }
        public virtual DbSet<EmployeeCheckInVisiblePermissions> EmployeeCheckInVisiblePermissionss { get; set; }

        public virtual DbSet<ConversationEmployeeTag> ConversationEmployeeTags { get; set; }
        
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                _logger.LogError("IsConfigured False");
                var hasTenant = _httpContext.Request.Headers.TryGetValue("TenantId", out var tenantId);
                if (!hasTenant && _httpContext.Request.Host.Value.Contains("localhost"))
                    tenantId = Configuration.GetValue<string>("TenantId");

                if (hasTenant)
                    tenantId = Encryption.DecryptRijndael(tenantId, AppConstants.EncryptionPrivateKey);

                var defaultDbName = Configuration.GetValue<string>("ConnectionStrings:DBName");
                var connectionString = Configuration.GetValue<string>("ConnectionStrings:ConnectionString");
                foreach (var splitItem in connectionString.Split(";"))
                {
                    if (splitItem.Contains(defaultDbName))
                    {
                        int startIndex = splitItem.IndexOf(defaultDbName);
                        var replaceddefaultDbName = splitItem.Substring(startIndex);
                        var dbName = defaultDbName + "_" + tenantId;
                        connectionString = connectionString.Replace(replaceddefaultDbName, dbName);

                        break;
                    }
                }
                _logger.LogError("Connection String - " + connectionString);
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AssignmentTypeMaster>(entity =>
            {
                entity.HasKey(e => e.AssignmentTypeId)
                    .HasName("PK__Assignme__F6D4789997CBD66B");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<BlockedDomain>(entity =>
            {
                entity.Property(e => e.DomainName).IsUnicode(false);
            });

            modelBuilder.Entity<CheckInDetail>(entity =>
            {
                entity.HasKey(e => e.CheckInDetailsId)
                    .HasName("PK__CheckInD__78E535A85BAEF70F");

                entity.Property(e => e.CheckInDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<CheckInPoint>(entity =>
            {
                entity.HasKey(e => e.CheckInPointsId)
                    .HasName("PK__CheckInP__2F7C52AF0CD7FFDD");
            });

            modelBuilder.Entity<ColorCode>(entity =>
            {
                entity.Property(e => e.BackGroundColorCode).IsUnicode(false);

                entity.Property(e => e.ColorCode1).IsUnicode(false);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.FeedbackDetail)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.FeedbackDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comment__Feedbac__13F1F5EB");
            });

            modelBuilder.Entity<ConfidenceAudit>(entity =>
            {
                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ConfidenceMaster>(entity =>
            {
                entity.HasKey(e => e.ConfidenceId)
                    .HasName("PK__Confiden__7E5E8BFA3BB16A31");
            });

            modelBuilder.Entity<Constant>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("((1))");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<ConversationFile>(entity =>
            {
                entity.Property(e => e.FileName).IsUnicode(false);
            });

            modelBuilder.Entity<CriteriaFeedbackMapping>(entity =>
            {
                entity.HasOne(d => d.CriteriaMaster)
                    .WithMany(p => p.CriteriaFeedbackMappings)
                    .HasForeignKey(d => d.CriteriaMasterId)
                    .HasConstraintName("FK__CriteriaF__Crite__14E61A24");

                entity.HasOne(d => d.FeedbackDetail)
                    .WithMany(p => p.CriteriaFeedbackMappings)
                    .HasForeignKey(d => d.FeedbackDetailId)
                    .HasConstraintName("FK__CriteriaF__Feedb__15DA3E5D");
            });

            modelBuilder.Entity<CriteriaMaster>(entity =>
            {
                entity.HasOne(d => d.CriteriaType)
                    .WithMany(p => p.CriteriaMasters)
                    .HasForeignKey(d => d.CriteriaTypeId)
                    .HasConstraintName("FK__CriteriaM__Crite__16CE6296");
            });

            modelBuilder.Entity<CriteriaTypeMaster>(entity =>
            {
                entity.HasKey(e => e.CriteriaTypeId)
                    .HasName("PK__Criteria__824897A0AF113C51");
            });

            modelBuilder.Entity<Cycle>(entity =>
            {
                entity.Property(e => e.CycleName).IsUnicode(false);
            });

            modelBuilder.Entity<CycleSymbol>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.SymbolName).IsUnicode(false);
            });

            modelBuilder.Entity<DirectReporteesFilter>(entity =>
            {
                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.Color).IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.StatusName).IsUnicode(false);
            });

            modelBuilder.Entity<Email>(entity =>
            {
                entity.Property(e => e.EmailAddress).IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmailId).IsUnicode(false);

                entity.Property(e => e.EmployeeCode).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);
            });

            modelBuilder.Entity<EmployeeContactDetail>(entity =>
            {
                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeContactDetails)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeContactDetail_Employees");
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__ErrorLog__5E5486484DEF0F6D");
            });

            modelBuilder.Entity<FeedbackDetail>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CriteriaTypeId).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsOneToOneRequested).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.FeedbackOnType)
                    .WithMany(p => p.FeedbackDetails)
                    .HasForeignKey(d => d.FeedbackOnTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FeedbackD__Feedb__18B6AB08");

                entity.HasOne(d => d.FeedbackRequest)
                    .WithMany(p => p.FeedbackDetails)
                    .HasForeignKey(d => d.FeedbackRequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FeedbackD__Feedb__17C286CF");
            });

            modelBuilder.Entity<FeedbackOnTypeMaster>(entity =>
            {
                entity.HasKey(e => e.FeedbackOnTypeId)
                    .HasName("PK__Feedback__84E6FCBA32627A76");

                entity.Property(e => e.FeedbackOnTypeId).ValueGeneratedNever();

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<FeedbackRequest>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.FeedbackOnType)
                    .WithMany(p => p.FeedbackRequests)
                    .HasForeignKey(d => d.FeedbackOnTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FeedbackR__Feedb__19AACF41");

                entity.HasOne(d => d.RaisedType)
                    .WithMany(p => p.FeedbackRequests)
                    .HasForeignKey(d => d.RaisedTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FeedbackR__Raise__1A9EF37A");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.FeedbackRequests)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FeedbackR__Statu__1B9317B3");
            });

            modelBuilder.Entity<GoalKey>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CurrencyCode).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Owner).HasDefaultValueSql("((0))");

                entity.Property(e => e.TeamId).HasDefaultValueSql("((0))");

                entity.Property(e => e.WeightId).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<GoalKeyHistory>(entity =>
            {
                entity.Property(e => e.Progress).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<GoalObjective>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.LinkedObjectiveId).HasDefaultValueSql("((0))");

                entity.Property(e => e.Owner).HasDefaultValueSql("((0))");

                entity.Property(e => e.Sequence).HasDefaultValueSql("((0))");

                entity.Property(e => e.TeamId).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<GoalSequence>(entity =>
            {
                entity.HasKey(e => e.SequenceId)
                    .HasName("PK__GoalSequ__BAD61491B83D22A7");
            });

            modelBuilder.Entity<GoalStatusMaster>(entity =>
            {
                entity.HasKey(e => e.GoalStatusId)
                    .HasName("PK__GoalStat__2CDC179FEBAEE3AA");
            });

            modelBuilder.Entity<GoalTypeMaster>(entity =>
            {
                entity.HasKey(e => e.GoalTypeId)
                    .HasName("PK__GoalType__20722C92915E7D8D");
            });

            modelBuilder.Entity<GuidedTour>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<GuidedTourControl>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<JobsAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__JobsAudi__A17F2398A1FA40AD");
            });

            modelBuilder.Entity<KrStatusMaster>(entity =>
            {
                entity.HasKey(e => e.KrStatusId)
                    .HasName("PK__KrStatus__CC69686A47A2A4E8");
            });

            modelBuilder.Entity<Mail>(entity =>
            {
                entity.Property(e => e.MailId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MailAddressType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MailSentLog>(entity =>
            {
                entity.Property(e => e.MailSentLogId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MailerTemplate>(entity =>
            {
                entity.Property(e => e.MailerTemplateId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MetricDataMaster>(entity =>
            {
                entity.Property(e => e.DataId).ValueGeneratedOnAdd();

                entity.Property(e => e.Symbol).IsFixedLength(true);
            });

            modelBuilder.Entity<MetricMaster>(entity =>
            {
                entity.HasKey(e => e.MetricId)
                    .HasName("PK__MetricMa__561056A5320E2242");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.Property(e => e.ModuleName).IsUnicode(false);
            });

            modelBuilder.Entity<NoteFile>(entity =>
            {
                entity.Property(e => e.FileName).IsUnicode(false);
            });

            modelBuilder.Entity<NotificationsDetail>(entity =>
            {
               
                entity.Property(e => e.NotificationOnId).HasDefaultValueSql("((0))");

                entity.Property(e => e.NotificationOnTypeId).HasDefaultValueSql("((0))");

                entity.Property(e => e.NotificationsDetailsId).ValueGeneratedOnAdd();
                entity.Property(e => e.Actionable).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<OkrAutoSubmitLog>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<OkrNatureMaster>(entity =>
            {
                entity.HasKey(e => e.OkrNatureId)
                    .HasName("PK__OkrNatur__0098FA50D3E3103A");
            });

            modelBuilder.Entity<OkrTypeFilter>(entity =>
            {
                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.Color).IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.StatusName).IsUnicode(false);
            });

            modelBuilder.Entity<OkrstatusMaster>(entity =>
            {
                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.Color).IsUnicode(false);

                entity.Property(e => e.StatusName).IsUnicode(false);
            });

            modelBuilder.Entity<OnBoardingScreen>(entity =>
            {
                entity.HasKey(e => e.ScreenId)
                    .HasName("PK__OnBoardi__0AB60FA5599E6D7E");

                entity.Property(e => e.ControlType).IsUnicode(false);

                entity.Property(e => e.PageName).IsUnicode(false);
            });

            modelBuilder.Entity<OneToOneDetail>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Status).HasDefaultValueSql("((2))");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.PermissionId).ValueGeneratedNever();

                entity.Property(e => e.IsEditable).HasDefaultValueSql("((1))");

                entity.Property(e => e.PermissionName).IsUnicode(false);
            });

            modelBuilder.Entity<ProgressAudit>(entity =>
            {
                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<RaisedTypeMaster>(entity =>
            {
                entity.HasKey(e => e.RaisedTypeId)
                    .HasName("PK__RaisedTy__75AB84036AAA85B9");

                entity.Property(e => e.RaisedTypeId).ValueGeneratedNever();

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<RequestMaster>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("requestMaster_pk");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName).IsUnicode(false);
            });

            modelBuilder.Entity<StatusMaster>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__StatusMa__C8EE2063630ACAF0");

                entity.Property(e => e.StatusId).ValueGeneratedNever();

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<TaskDetail>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("PK__TaskDeta__7C6949B11FCC1766");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.BackGroundColorCode).IsUnicode(false);

                entity.Property(e => e.Colorcode).IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.LogoName).IsUnicode(false);

                entity.Property(e => e.ParentId).HasDefaultValueSql("((0))");

                entity.Property(e => e.TeamHead).HasDefaultValueSql("((0))");

                entity.Property(e => e.TeamName).IsUnicode(false);
            });

            modelBuilder.Entity<TeamProgress>(entity =>
            {
                entity.Property(e => e.TeamName).IsUnicode(false);
            });

            modelBuilder.Entity<TypeOfGoalCreation>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<UnLockLog>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            });
            modelBuilder.Entity<EmployeeTag>(entity =>
            {
                entity.Property(e => e.ModuleId).HasDefaultValueSql("((1))");
            });
            modelBuilder.Entity<RecognitionEmployeeTeamMapping>(entity =>
            {
                entity.Property(e => e.IsGivenByManager).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<WeightMaster>(entity =>
            {
                entity.HasKey(e => e.WeightId)
                    .HasName("PK__WeightMa__02A0F31BB0DEC531");
            });

#pragma warning disable S3251 // Implementations should be provided for "partial" methods
            OnModelCreatingPartial(modelBuilder);
#pragma warning restore S3251 // Implementations should be provided for "partial" methods
        }

#pragma warning disable S3251 // Implementations should be provided for "partial" methods
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
#pragma warning restore S3251 // Implementations should be provided for "partial" methods
    }
}
