using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class terms_to_student : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_Terms_Students_StudentId",
            //     table: "Terms");

            // migrationBuilder.DropIndex(
            //     name: "IX_Terms_StudentId",
            //     table: "Terms");

            // migrationBuilder.DropColumn(
            //     name: "StudentId",
            //     table: "Terms");

            migrationBuilder.CreateTable(
                name: "StudentTerm",
                columns: table => new
                {
                    StudentsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TermsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTerm", x => new { x.StudentsId, x.TermsId });
                    table.ForeignKey(
                        name: "FK_StudentTerm_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTerm_Terms_TermsId",
                        column: x => x.TermsId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTerm_TermsId",
                table: "StudentTerm",
                column: "TermsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTerm");

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Terms",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Terms_StudentId",
                table: "Terms",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Terms_Students_StudentId",
                table: "Terms",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
