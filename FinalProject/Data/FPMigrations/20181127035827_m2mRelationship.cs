using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Data.FPMigrations
{
    public partial class m2mRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_MemberPosition_PositionID",
                schema: "FP",
                table: "MemberPosition",
                column: "PositionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberPosition",
                schema: "FP");
        }
    }
}
