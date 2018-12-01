using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace svanderest1_College_Strike.Data.FPMigrations
{
    public partial class addedMostofDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignment",
                schema: "FP",
                columns: table => new
                {
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                schema: "FP",
                columns: table => new
                {
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Phone = table.Column<long>(nullable: false),
                    eMail = table.Column<string>(maxLength: 255, nullable: false),
                    AssignmentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Member_Assignment_AssignmentID",
                        column: x => x.AssignmentID,
                        principalSchema: "FP",
                        principalTable: "Assignment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberPosition",
                schema: "FP",
                columns: table => new
                {
                    MemberID = table.Column<int>(nullable: false),
                    PositionID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberPosition", x => new { x.MemberID, x.PositionID });
                    table.ForeignKey(
                        name: "FK_MemberPosition_Member_MemberID",
                        column: x => x.MemberID,
                        principalSchema: "FP",
                        principalTable: "Member",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberPosition_Position_PositionID",
                        column: x => x.PositionID,
                        principalSchema: "FP",
                        principalTable: "Position",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shift",
                schema: "FP",
                columns: table => new
                {
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    MemberID = table.Column<int>(nullable: false),
                    AssignmentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Shift_Assignment_AssignmentID",
                        column: x => x.AssignmentID,
                        principalSchema: "FP",
                        principalTable: "Assignment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shift_Member_MemberID",
                        column: x => x.MemberID,
                        principalSchema: "FP",
                        principalTable: "Member",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Position_Title",
                schema: "FP",
                table: "Position",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_Name",
                schema: "FP",
                table: "Assignment",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Member_AssignmentID",
                schema: "FP",
                table: "Member",
                column: "AssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Member_eMail",
                schema: "FP",
                table: "Member",
                column: "eMail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MemberPosition_PositionID",
                schema: "FP",
                table: "MemberPosition",
                column: "PositionID");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_AssignmentID",
                schema: "FP",
                table: "Shift",
                column: "AssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_MemberID_Date",
                schema: "FP",
                table: "Shift",
                columns: new[] { "MemberID", "Date" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberPosition",
                schema: "FP");

            migrationBuilder.DropTable(
                name: "Shift",
                schema: "FP");

            migrationBuilder.DropTable(
                name: "Member",
                schema: "FP");

            migrationBuilder.DropTable(
                name: "Assignment",
                schema: "FP");

            migrationBuilder.DropIndex(
                name: "IX_Position_Title",
                schema: "FP",
                table: "Position");
        }
    }
}
