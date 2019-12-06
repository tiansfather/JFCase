using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    ServiceName = table.Column<string>(nullable: true),
                    MethodName = table.Column<string>(nullable: true),
                    Parameters = table.Column<string>(nullable: true),
                    ExecutionTime = table.Column<DateTime>(nullable: false),
                    ExecutionDuration = table.Column<int>(nullable: false),
                    ClientIpAddress = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    BrowserInfo = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true),
                    ImpersonatorUserId = table.Column<long>(nullable: true),
                    ImpersonatorTenantId = table.Column<int>(nullable: true),
                    CustomData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Edition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    ToEmail = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Success = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginAttempt",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    TenancyName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    UserNameOrPhoneNumber = table.Column<string>(nullable: true),
                    ClientIpAddress = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    BrowserInfo = table.Column<string>(nullable: true),
                    Result = table.Column<byte>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginAttempt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureSetting",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EditionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureSetting_Edition_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Edition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseInitial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    CaseSourceId = table.Column<int>(nullable: false),
                    SubjectId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    Law = table.Column<string>(nullable: true),
                    Experience = table.Column<string>(nullable: true),
                    LawyerOpinion = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    PublisDate = table.Column<DateTime>(nullable: true),
                    ReadNumber = table.Column<int>(nullable: false),
                    PraiseNumber = table.Column<int>(nullable: false),
                    BeatNumber = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CaseStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseInitial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseNode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    RelType = table.Column<string>(nullable: true),
                    RelName = table.Column<string>(nullable: true),
                    RelValue = table.Column<string>(nullable: true),
                    BaseTreeId = table.Column<int>(nullable: false),
                    CaseInitialId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseNode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseNode_CaseInitial_CaseInitialId",
                        column: x => x.CaseInitialId,
                        principalTable: "CaseInitial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseSource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    SourceSN = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    Court1Id = table.Column<int>(nullable: true),
                    Court2Id = table.Column<int>(nullable: true),
                    AnYouId = table.Column<int>(nullable: true),
                    ValidDate = table.Column<DateTime>(nullable: false),
                    SourceFile = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    CaseSourceStatus = table.Column<int>(nullable: false),
                    OwerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TreeLabel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    BaseTreeId = table.Column<int>(nullable: false),
                    LabelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeLabel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseCard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CaseInitialId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseCard_CaseInitial_CaseInitialId",
                        column: x => x.CaseInitialId,
                        principalTable: "CaseInitial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseFine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    CaseInitialId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    MediaPath = table.Column<string>(nullable: true),
                    UserModifyTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseFine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseFine_CaseInitial_CaseInitialId",
                        column: x => x.CaseInitialId,
                        principalTable: "CaseInitial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseLabel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    RelType = table.Column<string>(nullable: true),
                    RelName = table.Column<string>(nullable: true),
                    RelValue = table.Column<string>(nullable: true),
                    LabelId = table.Column<int>(nullable: false),
                    CaseInitialId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseLabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseLabel_CaseInitial_CaseInitialId",
                        column: x => x.CaseInitialId,
                        principalTable: "CaseInitial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaseSourceHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    CaseSourceId = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseSourceHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseSourceHistory_CaseSource_CaseSourceId",
                        column: x => x.CaseSourceId,
                        principalTable: "CaseSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    EditionId = table.Column<int>(nullable: true),
                    TenancyName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ConnectionString = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Property = table.Column<string>(type: "json", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenant_Edition_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Edition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ColumnInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ModuleInfoId = table.Column<int>(nullable: false),
                    ColumnType = table.Column<int>(nullable: false),
                    EnableFieldPermission = table.Column<bool>(nullable: false),
                    ControlFormat = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    ColumnKey = table.Column<string>(nullable: true),
                    ColumnName = table.Column<string>(nullable: true),
                    Templet = table.Column<string>(nullable: true),
                    MaxFileNumber = table.Column<int>(nullable: false),
                    IsInterColumn = table.Column<bool>(nullable: false),
                    IsSystemColumn = table.Column<bool>(nullable: false),
                    DisplayFormat = table.Column<string>(nullable: true),
                    DefaultValue = table.Column<string>(nullable: true),
                    VerifyRules = table.Column<string>(nullable: true),
                    Renderer = table.Column<string>(nullable: true),
                    ValuePath = table.Column<string>(nullable: true),
                    DisplayPath = table.Column<string>(nullable: true),
                    DictionaryName = table.Column<string>(nullable: true),
                    CustomizeControl = table.Column<string>(nullable: true),
                    ControlParameter = table.Column<string>(nullable: true),
                    IsShownInList = table.Column<bool>(nullable: false),
                    IsShownInAdd = table.Column<bool>(nullable: false),
                    IsShownInEdit = table.Column<bool>(nullable: false),
                    IsShownInMultiEdit = table.Column<bool>(nullable: false),
                    IsShownInAdvanceSearch = table.Column<bool>(nullable: false),
                    IsShownInView = table.Column<bool>(nullable: false),
                    IsEnableSort = table.Column<bool>(nullable: false),
                    RelativeDataType = table.Column<int>(nullable: false),
                    RelativeDataString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColumnInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModuleButton",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ModuleInfoId = table.Column<int>(nullable: false),
                    ButtonKey = table.Column<string>(nullable: true),
                    ButtonName = table.Column<string>(nullable: true),
                    ButtonClass = table.Column<string>(nullable: true),
                    TitleTemplet = table.Column<string>(nullable: true),
                    ButtonActionType = table.Column<int>(nullable: false),
                    ButtonType = table.Column<int>(nullable: false),
                    ButtonActionUrl = table.Column<string>(nullable: true),
                    ButtonActionParam = table.Column<string>(nullable: true),
                    ConfirmMsg = table.Column<string>(nullable: true),
                    ButtonScript = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    RequirePermission = table.Column<bool>(nullable: false),
                    ClientShowCondition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleButton", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleButton_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    ModuleInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(maxLength: 2, nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    WorkLocation = table.Column<string>(nullable: true),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ToBeVerified = table.Column<bool>(nullable: false),
                    LockoutEndDate = table.Column<DateTime>(nullable: true),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    LastLoginTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaseTree",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: false),
                    BriefCode = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    TreeNodeType = table.Column<int>(nullable: false),
                    EnableMultiSelect = table.Column<bool>(nullable: false),
                    RelativeNodeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseTree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseTree_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseTree_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseTree_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseTree_BaseTree_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BaseTree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseTree_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaseType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    BriefName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseType_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseType_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseType_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseType_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dictionary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    DictionaryName = table.Column<string>(nullable: true),
                    DictionaryContent = table.Column<string>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dictionary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dictionary_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dictionary_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dictionary_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dictionary_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FileSize = table.Column<decimal>(nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                    table.ForeignKey(
                        name: "FK_File_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_File_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_File_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Label",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    LabelName = table.Column<string>(nullable: true),
                    LabelType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Label", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Label_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Label_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Label_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Label_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    IsInterModule = table.Column<bool>(nullable: false),
                    RequiredFeature = table.Column<string>(nullable: true),
                    ModuleKey = table.Column<string>(nullable: true),
                    EntityFullName = table.Column<string>(nullable: true),
                    ModuleName = table.Column<string>(nullable: true),
                    DefaultLimit = table.Column<int>(nullable: false),
                    Limits = table.Column<string>(nullable: true),
                    SortField = table.Column<string>(nullable: true),
                    SortType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleInfo_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleInfo_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleInfo_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewMiner",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    OpenId = table.Column<string>(nullable: true),
                    Avata = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    WorkLocation = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    Verified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewMiner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewMiner_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewMiner_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewMiner_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewMiner_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    NoticeTitle = table.Column<string>(nullable: true),
                    NoticeContent = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    NoticeType = table.Column<string>(nullable: true),
                    ToTenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notice_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notice_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notice_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notice_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: false),
                    BriefCode = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organization_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organization_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organization_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organization_Organization_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Organization_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ResourceName = table.Column<string>(nullable: true),
                    ResourceType = table.Column<string>(nullable: true),
                    ResourceContent = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resource_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resource_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resource_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resource_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    IsStatic = table.Column<bool>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ExtensionData = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Property = table.Column<string>(type: "json", nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    TemplateName = table.Column<string>(nullable: true),
                    TemplateType = table.Column<string>(nullable: true),
                    TemplateContent = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Template_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Template_User_DeleterUserId",
                        column: x => x.DeleterUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Template_User_LastModifierUserId",
                        column: x => x.LastModifierUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Template_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsGranted = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    RoleId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_TenantId_ExecutionDuration",
                table: "AuditLog",
                columns: new[] { "TenantId", "ExecutionDuration" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_TenantId_ExecutionTime",
                table: "AuditLog",
                columns: new[] { "TenantId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_TenantId_UserId",
                table: "AuditLog",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_CreatorUserId",
                table: "BaseTree",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_DeleterUserId",
                table: "BaseTree",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_LastModifierUserId",
                table: "BaseTree",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_ParentId",
                table: "BaseTree",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseTree_TenantId",
                table: "BaseTree",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseType_CreatorUserId",
                table: "BaseType",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseType_DeleterUserId",
                table: "BaseType",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseType_LastModifierUserId",
                table: "BaseType",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseType_TenantId",
                table: "BaseType",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseCard_CaseInitialId",
                table: "CaseCard",
                column: "CaseInitialId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseCard_CreatorUserId",
                table: "CaseCard",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseCard_DeleterUserId",
                table: "CaseCard",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseCard_LastModifierUserId",
                table: "CaseCard",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseCard_TenantId",
                table: "CaseCard",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFine_CaseInitialId",
                table: "CaseFine",
                column: "CaseInitialId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFine_CreatorUserId",
                table: "CaseFine",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFine_DeleterUserId",
                table: "CaseFine",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFine_LastModifierUserId",
                table: "CaseFine",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseFine_TenantId",
                table: "CaseFine",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_CaseSourceId",
                table: "CaseInitial",
                column: "CaseSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_CreatorUserId",
                table: "CaseInitial",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_DeleterUserId",
                table: "CaseInitial",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_LastModifierUserId",
                table: "CaseInitial",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_SubjectId",
                table: "CaseInitial",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseInitial_TenantId",
                table: "CaseInitial",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseLabel_CaseInitialId",
                table: "CaseLabel",
                column: "CaseInitialId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseLabel_LabelId",
                table: "CaseLabel",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseNode_BaseTreeId",
                table: "CaseNode",
                column: "BaseTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseNode_CaseInitialId",
                table: "CaseNode",
                column: "CaseInitialId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_AnYouId",
                table: "CaseSource",
                column: "AnYouId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_CityId",
                table: "CaseSource",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_Court1Id",
                table: "CaseSource",
                column: "Court1Id");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_Court2Id",
                table: "CaseSource",
                column: "Court2Id");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_CreatorUserId",
                table: "CaseSource",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_DeleterUserId",
                table: "CaseSource",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_LastModifierUserId",
                table: "CaseSource",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_OwerId",
                table: "CaseSource",
                column: "OwerId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSource_TenantId",
                table: "CaseSource",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSourceHistory_CaseSourceId",
                table: "CaseSourceHistory",
                column: "CaseSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSourceHistory_CreatorUserId",
                table: "CaseSourceHistory",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSourceHistory_DeleterUserId",
                table: "CaseSourceHistory",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSourceHistory_LastModifierUserId",
                table: "CaseSourceHistory",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseSourceHistory_TenantId",
                table: "CaseSourceHistory",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ColumnInfo_ModuleInfoId",
                table: "ColumnInfo",
                column: "ModuleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_CreatorUserId",
                table: "Dictionary",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_DeleterUserId",
                table: "Dictionary",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_LastModifierUserId",
                table: "Dictionary",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Dictionary_TenantId",
                table: "Dictionary",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureSetting_EditionId",
                table: "FeatureSetting",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_File_CreatorUserId",
                table: "File",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_File_DeleterUserId",
                table: "File",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_File_LastModifierUserId",
                table: "File",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Label_CreatorUserId",
                table: "Label",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Label_DeleterUserId",
                table: "Label",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Label_LastModifierUserId",
                table: "Label",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Label_TenantId",
                table: "Label",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleButton_CreatorUserId",
                table: "ModuleButton",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleButton_DeleterUserId",
                table: "ModuleButton",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleButton_LastModifierUserId",
                table: "ModuleButton",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleButton_ModuleInfoId",
                table: "ModuleButton",
                column: "ModuleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleButton_TenantId",
                table: "ModuleButton",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleData_CreatorUserId",
                table: "ModuleData",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleData_DeleterUserId",
                table: "ModuleData",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleData_LastModifierUserId",
                table: "ModuleData",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleData_ModuleInfoId",
                table: "ModuleData",
                column: "ModuleInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleInfo_CreatorUserId",
                table: "ModuleInfo",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleInfo_DeleterUserId",
                table: "ModuleInfo",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleInfo_LastModifierUserId",
                table: "ModuleInfo",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewMiner_CreatorUserId",
                table: "NewMiner",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewMiner_DeleterUserId",
                table: "NewMiner",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewMiner_LastModifierUserId",
                table: "NewMiner",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewMiner_TenantId",
                table: "NewMiner",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Notice_CreatorUserId",
                table: "Notice",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notice_DeleterUserId",
                table: "Notice",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notice_LastModifierUserId",
                table: "Notice",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notice_TenantId",
                table: "Notice",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_CreatorUserId",
                table: "Organization",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_DeleterUserId",
                table: "Organization",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_LastModifierUserId",
                table: "Organization",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_ParentId",
                table: "Organization",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_TenantId",
                table: "Organization",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_TenantId_Name",
                table: "Permissions",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_CreatorUserId",
                table: "Resource",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_DeleterUserId",
                table: "Resource",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_LastModifierUserId",
                table: "Resource",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_TenantId",
                table: "Resource",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatorUserId",
                table: "Role",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_DeleterUserId",
                table: "Role",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_LastModifierUserId",
                table: "Role",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_TenantId",
                table: "Role",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_TenantId_Name",
                table: "Settings",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Template_CreatorUserId",
                table: "Template",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_DeleterUserId",
                table: "Template",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_LastModifierUserId",
                table: "Template",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_TenantId",
                table: "Template",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_CreatorUserId",
                table: "Tenant",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_DeleterUserId",
                table: "Tenant",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_EditionId",
                table: "Tenant",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_LastModifierUserId",
                table: "Tenant",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeLabel_BaseTreeId",
                table: "TreeLabel",
                column: "BaseTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_TreeLabel_LabelId",
                table: "TreeLabel",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatorUserId",
                table: "User",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeleterUserId",
                table: "User",
                column: "DeleterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastModifierUserId",
                table: "User",
                column: "LastModifierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_OrganizationId",
                table: "User",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_User_TenantId",
                table: "User",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginAttempt_UserId_TenantId",
                table: "UserLoginAttempt",
                columns: new[] { "UserId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginAttempt_TenancyName_UserNameOrPhoneNumber_Result",
                table: "UserLoginAttempt",
                columns: new[] { "TenancyName", "UserNameOrPhoneNumber", "Result" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_TenantId_RoleId",
                table: "UserRole",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_TenantId_UserId",
                table: "UserRole",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInitial_User_CreatorUserId",
                table: "CaseInitial",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInitial_User_DeleterUserId",
                table: "CaseInitial",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInitial_User_LastModifierUserId",
                table: "CaseInitial",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInitial_Tenant_TenantId",
                table: "CaseInitial",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInitial_CaseSource_CaseSourceId",
                table: "CaseInitial",
                column: "CaseSourceId",
                principalTable: "CaseSource",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseInitial_BaseTree_SubjectId",
                table: "CaseInitial",
                column: "SubjectId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseNode_BaseTree_BaseTreeId",
                table: "CaseNode",
                column: "BaseTreeId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_User_CreatorUserId",
                table: "CaseSource",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_User_DeleterUserId",
                table: "CaseSource",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_User_LastModifierUserId",
                table: "CaseSource",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_User_OwerId",
                table: "CaseSource",
                column: "OwerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_Tenant_TenantId",
                table: "CaseSource",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_BaseTree_AnYouId",
                table: "CaseSource",
                column: "AnYouId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_BaseTree_CityId",
                table: "CaseSource",
                column: "CityId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_BaseTree_Court1Id",
                table: "CaseSource",
                column: "Court1Id",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSource_BaseTree_Court2Id",
                table: "CaseSource",
                column: "Court2Id",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TreeLabel_BaseTree_BaseTreeId",
                table: "TreeLabel",
                column: "BaseTreeId",
                principalTable: "BaseTree",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TreeLabel_Label_LabelId",
                table: "TreeLabel",
                column: "LabelId",
                principalTable: "Label",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseCard_User_CreatorUserId",
                table: "CaseCard",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseCard_User_DeleterUserId",
                table: "CaseCard",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseCard_User_LastModifierUserId",
                table: "CaseCard",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseCard_Tenant_TenantId",
                table: "CaseCard",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFine_User_CreatorUserId",
                table: "CaseFine",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFine_User_DeleterUserId",
                table: "CaseFine",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFine_User_LastModifierUserId",
                table: "CaseFine",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseFine_Tenant_TenantId",
                table: "CaseFine",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseLabel_Label_LabelId",
                table: "CaseLabel",
                column: "LabelId",
                principalTable: "Label",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSourceHistory_User_CreatorUserId",
                table: "CaseSourceHistory",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSourceHistory_User_DeleterUserId",
                table: "CaseSourceHistory",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSourceHistory_User_LastModifierUserId",
                table: "CaseSourceHistory",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseSourceHistory_Tenant_TenantId",
                table: "CaseSourceHistory",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenant_User_CreatorUserId",
                table: "Tenant",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenant_User_DeleterUserId",
                table: "Tenant",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenant_User_LastModifierUserId",
                table: "Tenant",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ColumnInfo_ModuleInfo_ModuleInfoId",
                table: "ColumnInfo",
                column: "ModuleInfoId",
                principalTable: "ModuleInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleButton_User_CreatorUserId",
                table: "ModuleButton",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleButton_User_DeleterUserId",
                table: "ModuleButton",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleButton_User_LastModifierUserId",
                table: "ModuleButton",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleButton_ModuleInfo_ModuleInfoId",
                table: "ModuleButton",
                column: "ModuleInfoId",
                principalTable: "ModuleInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleData_User_CreatorUserId",
                table: "ModuleData",
                column: "CreatorUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleData_User_DeleterUserId",
                table: "ModuleData",
                column: "DeleterUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleData_User_LastModifierUserId",
                table: "ModuleData",
                column: "LastModifierUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleData_ModuleInfo_ModuleInfoId",
                table: "ModuleData",
                column: "ModuleInfoId",
                principalTable: "ModuleInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Organization_OrganizationId",
                table: "User",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organization_User_CreatorUserId",
                table: "Organization");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_User_DeleterUserId",
                table: "Organization");

            migrationBuilder.DropForeignKey(
                name: "FK_Organization_User_LastModifierUserId",
                table: "Organization");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenant_User_CreatorUserId",
                table: "Tenant");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenant_User_DeleterUserId",
                table: "Tenant");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenant_User_LastModifierUserId",
                table: "Tenant");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "BaseType");

            migrationBuilder.DropTable(
                name: "CaseCard");

            migrationBuilder.DropTable(
                name: "CaseFine");

            migrationBuilder.DropTable(
                name: "CaseLabel");

            migrationBuilder.DropTable(
                name: "CaseNode");

            migrationBuilder.DropTable(
                name: "CaseSourceHistory");

            migrationBuilder.DropTable(
                name: "ColumnInfo");

            migrationBuilder.DropTable(
                name: "Dictionary");

            migrationBuilder.DropTable(
                name: "EmailLog");

            migrationBuilder.DropTable(
                name: "FeatureSetting");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "ModuleButton");

            migrationBuilder.DropTable(
                name: "ModuleData");

            migrationBuilder.DropTable(
                name: "NewMiner");

            migrationBuilder.DropTable(
                name: "Notice");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropTable(
                name: "TreeLabel");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserLoginAttempt");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "CaseInitial");

            migrationBuilder.DropTable(
                name: "ModuleInfo");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Label");

            migrationBuilder.DropTable(
                name: "CaseSource");

            migrationBuilder.DropTable(
                name: "BaseTree");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "Tenant");

            migrationBuilder.DropTable(
                name: "Edition");
        }
    }
}
