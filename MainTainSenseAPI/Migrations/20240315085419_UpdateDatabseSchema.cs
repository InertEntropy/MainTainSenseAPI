using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainTainSenseAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accesslevels",
                columns: table => new
                {
                    accesslevelid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accesslevelname = table.Column<string>(type: "text(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accesslevels", x => x.accesslevelid);
                });

            migrationBuilder.CreateTable(
                name: "assettypes",
                columns: table => new
                {
                    assettypeid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ismachine = table.Column<int>(type: "INTEGER", nullable: false),
                    assettypename = table.Column<string>(type: "text(50)", nullable: false),
                    assettypedescription = table.Column<string>(type: "TEXT", nullable: true),
                    active = table.Column<int>(type: "INTEGER", nullable: false),
                    updatedby = table.Column<string>(type: "text(50)", nullable: true),
                    lastupdate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assettypes", x => x.assettypeid);
                });

            migrationBuilder.CreateTable(
                name: "buildings",
                columns: table => new
                {
                    buildingid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    buildingname = table.Column<string>(type: "text(100)", nullable: false),
                    buildingdescription = table.Column<string>(type: "text(255)", nullable: true),
                    updatedby = table.Column<string>(type: "text(50)", nullable: true),
                    lastupdate = table.Column<string>(type: "TEXT", nullable: true),
                    isactive = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buildings", x => x.buildingid);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    categoryid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    categorydescription = table.Column<string>(type: "TEXT", nullable: false),
                    isactive = table.Column<int>(type: "INTEGER", nullable: false),
                    updatedby = table.Column<string>(type: "text(50)", nullable: true),
                    lastupdate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.categoryid);
                });

            migrationBuilder.CreateTable(
                name: "checklists",
                columns: table => new
                {
                    checklistid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    checklistname = table.Column<string>(type: "text(50)", nullable: false),
                    isactive = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checklists", x => x.checklistid);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    departmentid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    departmentname = table.Column<string>(type: "text(50)", nullable: false),
                    isactive = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.departmentid);
                });

            migrationBuilder.CreateTable(
                name: "frequency",
                columns: table => new
                {
                    frequencyid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    frequencydescription = table.Column<string>(type: "TEXT", nullable: false),
                    intervalvalue = table.Column<int>(type: "INTEGER", nullable: false),
                    timeunit = table.Column<string>(type: "TEXT", nullable: false),
                    isactive = table.Column<int>(type: "INTEGER", nullable: true),
                    dayofmonth = table.Column<int>(type: "INTEGER", nullable: true),
                    dayofweek = table.Column<string>(type: "TEXT", nullable: true),
                    frequencymonth = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_frequency", x => x.frequencyid);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    permissionid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    permissionname = table.Column<string>(type: "text(50)", nullable: false),
                    permissionsdescription = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.permissionid);
                });

            migrationBuilder.CreateTable(
                name: "preventivemaintenance",
                columns: table => new
                {
                    pmid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    pmdescription = table.Column<string>(type: "TEXT", nullable: false),
                    assetid = table.Column<int>(type: "INTEGER", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    lastcompleteddate = table.Column<string>(type: "TEXT", nullable: false),
                    nextduedate = table.Column<string>(type: "TEXT", nullable: false),
                    notes = table.Column<string>(type: "TEXT", nullable: true),
                    frequencyid = table.Column<int>(type: "INTEGER", nullable: false),
                    updatedby = table.Column<string>(type: "text(50)", nullable: true),
                    lastupdate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preventivemaintenance", x => x.pmid);
                });

            migrationBuilder.CreateTable(
                name: "priorities",
                columns: table => new
                {
                    priorityid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    priorityname = table.Column<string>(type: "text(20)", nullable: false),
                    prioritylevel = table.Column<int>(type: "INTEGER", nullable: false),
                    colorcode = table.Column<string>(type: "text(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priorities", x => x.priorityid);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    roleid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    rolename = table.Column<string>(type: "text(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.roleid);
                });

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    statusid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    statusname = table.Column<string>(type: "text(20)", nullable: false),
                    statusdescription = table.Column<string>(type: "TEXT", nullable: true),
                    isactive = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.statusid);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    teamid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    teamname = table.Column<string>(type: "text(50)", nullable: false),
                    department = table.Column<string>(type: "text(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.teamid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "text(50)", nullable: false),
                    firstname = table.Column<string>(type: "text(50)", nullable: false),
                    lastname = table.Column<string>(type: "text(50)", nullable: false),
                    lastlogin = table.Column<string>(type: "TEXT", nullable: true),
                    departmentid = table.Column<int>(type: "INTEGER", nullable: false),
                    isactive = table.Column<bool>(type: "boolean", nullable: true),
                    email = table.Column<string>(type: "text(100)", nullable: true),
                    updatedby = table.Column<string>(type: "text(50)", nullable: true),
                    lastupdate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "pmtemplates",
                columns: table => new
                {
                    templateid = table.Column<int>(type: "INTEGER", nullable: true),
                    templatename = table.Column<string>(type: "text(50)", nullable: false),
                    assettypeid = table.Column<int>(type: "INTEGER", nullable: false),
                    pmtemplatedescription = table.Column<string>(type: "TEXT", nullable: true),
                    isactive = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_pmtemplates_assettypes_assettypeid",
                        column: x => x.assettypeid,
                        principalTable: "assettypes",
                        principalColumn: "assettypeid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    locationid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    locationname = table.Column<string>(type: "text(100)", nullable: false),
                    locationdescription = table.Column<string>(type: "TEXT", nullable: false),
                    buildingid = table.Column<int>(type: "INTEGER", nullable: true),
                    isactive = table.Column<double>(type: "REAL", nullable: true),
                    locationpath = table.Column<string>(type: "text(255)", nullable: true),
                    parentlocationid = table.Column<int>(type: "INTEGER", nullable: true),
                    updatedby = table.Column<string>(type: "text(50)", nullable: true),
                    lastupdate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.locationid);
                    table.ForeignKey(
                        name: "FK_locations_buildings_buildingid",
                        column: x => x.buildingid,
                        principalTable: "buildings",
                        principalColumn: "buildingid");
                    table.ForeignKey(
                        name: "FK_locations_locations_parentlocationid",
                        column: x => x.parentlocationid,
                        principalTable: "locations",
                        principalColumn: "locationid");
                });

            migrationBuilder.CreateTable(
                name: "checklistitems",
                columns: table => new
                {
                    itemid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    checklistid = table.Column<int>(type: "INTEGER", nullable: false),
                    checklistitemsdescription = table.Column<string>(type: "TEXT", nullable: false),
                    iscompleted = table.Column<int>(type: "INTEGER", nullable: true),
                    sortorder = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_checklistitems", x => x.itemid);
                    table.ForeignKey(
                        name: "FK_checklistitems_checklists_checklistid",
                        column: x => x.checklistid,
                        principalTable: "checklists",
                        principalColumn: "checklistid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "templatetasks",
                columns: table => new
                {
                    templateid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    templatetasksdescription = table.Column<string>(type: "TEXT", nullable: false),
                    frequencyid = table.Column<int>(type: "INTEGER", nullable: true),
                    checklistid = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_templatetasks", x => x.templateid);
                    table.ForeignKey(
                        name: "FK_templatetasks_checklists_checklistid",
                        column: x => x.checklistid,
                        principalTable: "checklists",
                        principalColumn: "checklistid");
                    table.ForeignKey(
                        name: "FK_templatetasks_frequency_frequencyid",
                        column: x => x.frequencyid,
                        principalTable: "frequency",
                        principalColumn: "frequencyid");
                });

            migrationBuilder.CreateTable(
                name: "pmchecklists",
                columns: table => new
                {
                    checklistid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    pmid = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pmchecklists", x => x.checklistid);
                    table.ForeignKey(
                        name: "FK_pmchecklists_preventivemaintenance_pmid",
                        column: x => x.pmid,
                        principalTable: "preventivemaintenance",
                        principalColumn: "pmid");
                });

            migrationBuilder.CreateTable(
                name: "rolepermissions",
                columns: table => new
                {
                    permissionid = table.Column<int>(type: "INTEGER", nullable: false),
                    roleid = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_rolepermissions_permissions_permissionid",
                        column: x => x.permissionid,
                        principalTable: "permissions",
                        principalColumn: "permissionid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rolepermissions_roles_roleid",
                        column: x => x.roleid,
                        principalTable: "roles",
                        principalColumn: "roleid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    messagesid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    parenttmessageid = table.Column<int>(type: "INTEGER", nullable: true),
                    recipientid = table.Column<int>(type: "INTEGER", nullable: false),
                    messagetext = table.Column<string>(type: "TEXT", nullable: true),
                    isdeletedforsender = table.Column<int>(type: "INTEGER", nullable: true),
                    senderid = table.Column<int>(type: "INTEGER", nullable: false),
                    isread = table.Column<int>(type: "INTEGER", nullable: true),
                    subject = table.Column<string>(type: "text(50)", nullable: true),
                    creationtime = table.Column<string>(type: "TEXT", nullable: true),
                    isdeletedforrecipient = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.messagesid);
                    table.ForeignKey(
                        name: "FK_messages_messages_parenttmessageid",
                        column: x => x.parenttmessageid,
                        principalTable: "messages",
                        principalColumn: "messagesid");
                    table.ForeignKey(
                        name: "FK_messages_users_recipientid",
                        column: x => x.recipientid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messages_users_senderid",
                        column: x => x.senderid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teammembers",
                columns: table => new
                {
                    teamid = table.Column<int>(type: "INTEGER", nullable: true),
                    userid = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_teammembers_teams_teamid",
                        column: x => x.teamid,
                        principalTable: "teams",
                        principalColumn: "teamid");
                    table.ForeignKey(
                        name: "FK_teammembers_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid");
                });

            migrationBuilder.CreateTable(
                name: "userroles",
                columns: table => new
                {
                    roleid = table.Column<int>(type: "INTEGER", nullable: true),
                    userid = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_userroles_roles_roleid",
                        column: x => x.roleid,
                        principalTable: "roles",
                        principalColumn: "roleid");
                    table.ForeignKey(
                        name: "FK_userroles_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    assetid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    assettypeid = table.Column<int>(type: "INTEGER", nullable: false),
                    serialnumber = table.Column<string>(type: "text(50)", maxLength: 50, nullable: true),
                    assetlocationid = table.Column<int>(type: "INTEGER", nullable: true),
                    assetstatus = table.Column<int>(type: "text(50)", nullable: false),
                    assetdescription = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    assetname = table.Column<string>(type: "text(100)", maxLength: 50, nullable: false),
                    installdate = table.Column<string>(type: "TEXT", nullable: true),
                    updatedby = table.Column<string>(type: "text(50)", nullable: true),
                    lastupdate = table.Column<string>(type: "TEXT", nullable: true),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assets", x => x.assetid);
                    table.ForeignKey(
                        name: "FK_assets_assettypes_assettypeid",
                        column: x => x.assettypeid,
                        principalTable: "assettypes",
                        principalColumn: "assettypeid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assets_locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "locations",
                        principalColumn: "locationid");
                });

            migrationBuilder.CreateTable(
                name: "workorders",
                columns: table => new
                {
                    workorderid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    workorderdescription = table.Column<string>(type: "TEXT", nullable: false),
                    requestedby = table.Column<string>(type: "text(50)", nullable: true),
                    createddate = table.Column<string>(type: "TEXT", nullable: true),
                    assetid = table.Column<int>(type: "INTEGER", nullable: true),
                    priorityid = table.Column<int>(type: "INTEGER", nullable: true),
                    scheduleddate = table.Column<string>(type: "TEXT", nullable: true),
                    notes = table.Column<string>(type: "TEXT", nullable: true),
                    assignedteamid = table.Column<int>(type: "INTEGER", nullable: true),
                    userid = table.Column<int>(type: "INTEGER", nullable: true),
                    statusid = table.Column<int>(type: "INTEGER", nullable: true),
                    completiondate = table.Column<string>(type: "TEXT", nullable: true),
                    duedate = table.Column<string>(type: "TEXT", nullable: true),
                    lastupdate = table.Column<string>(type: "TEXT", nullable: true),
                    updatedby = table.Column<string>(type: "text(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workorders", x => x.workorderid);
                    table.ForeignKey(
                        name: "FK_workorders_assets_assetid",
                        column: x => x.assetid,
                        principalTable: "assets",
                        principalColumn: "assetid");
                    table.ForeignKey(
                        name: "FK_workorders_priorities_priorityid",
                        column: x => x.priorityid,
                        principalTable: "priorities",
                        principalColumn: "priorityid");
                    table.ForeignKey(
                        name: "FK_workorders_status_statusid",
                        column: x => x.statusid,
                        principalTable: "status",
                        principalColumn: "statusid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_assets_assettypeid",
                table: "assets",
                column: "assettypeid");

            migrationBuilder.CreateIndex(
                name: "IX_assets_LocationId",
                table: "assets",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_checklistitems_checklistid",
                table: "checklistitems",
                column: "checklistid");

            migrationBuilder.CreateIndex(
                name: "IX_locations_buildingid",
                table: "locations",
                column: "buildingid");

            migrationBuilder.CreateIndex(
                name: "IX_locations_parentlocationid",
                table: "locations",
                column: "parentlocationid");

            migrationBuilder.CreateIndex(
                name: "IX_messages_parenttmessageid",
                table: "messages",
                column: "parenttmessageid");

            migrationBuilder.CreateIndex(
                name: "IX_messages_recipientid",
                table: "messages",
                column: "recipientid");

            migrationBuilder.CreateIndex(
                name: "IX_messages_senderid",
                table: "messages",
                column: "senderid");

            migrationBuilder.CreateIndex(
                name: "IX_pmchecklists_pmid",
                table: "pmchecklists",
                column: "pmid");

            migrationBuilder.CreateIndex(
                name: "IX_pmtemplates_assettypeid",
                table: "pmtemplates",
                column: "assettypeid");

            migrationBuilder.CreateIndex(
                name: "IX_rolepermissions_permissionid",
                table: "rolepermissions",
                column: "permissionid");

            migrationBuilder.CreateIndex(
                name: "IX_rolepermissions_roleid",
                table: "rolepermissions",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "IX_teammembers_teamid",
                table: "teammembers",
                column: "teamid");

            migrationBuilder.CreateIndex(
                name: "IX_teammembers_userid",
                table: "teammembers",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_templatetasks_checklistid",
                table: "templatetasks",
                column: "checklistid");

            migrationBuilder.CreateIndex(
                name: "IX_templatetasks_frequencyid",
                table: "templatetasks",
                column: "frequencyid");

            migrationBuilder.CreateIndex(
                name: "IX_userroles_roleid",
                table: "userroles",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "IX_userroles_userid",
                table: "userroles",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_workorders_assetid",
                table: "workorders",
                column: "assetid");

            migrationBuilder.CreateIndex(
                name: "IX_workorders_priorityid",
                table: "workorders",
                column: "priorityid");

            migrationBuilder.CreateIndex(
                name: "IX_workorders_statusid",
                table: "workorders",
                column: "statusid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accesslevels");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "checklistitems");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "pmchecklists");

            migrationBuilder.DropTable(
                name: "pmtemplates");

            migrationBuilder.DropTable(
                name: "rolepermissions");

            migrationBuilder.DropTable(
                name: "teammembers");

            migrationBuilder.DropTable(
                name: "templatetasks");

            migrationBuilder.DropTable(
                name: "userroles");

            migrationBuilder.DropTable(
                name: "workorders");

            migrationBuilder.DropTable(
                name: "preventivemaintenance");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "checklists");

            migrationBuilder.DropTable(
                name: "frequency");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "assets");

            migrationBuilder.DropTable(
                name: "priorities");

            migrationBuilder.DropTable(
                name: "status");

            migrationBuilder.DropTable(
                name: "assettypes");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "buildings");
        }
    }
}
