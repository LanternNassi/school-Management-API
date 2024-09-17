using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class feesStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentFeesStructures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TermId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFeesStructures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentFeesStructures_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentFeesStructures_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeesStructures_StudentId",
                table: "StudentFeesStructures",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeesStructures_TermId",
                table: "StudentFeesStructures",
                column: "TermId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentFeesStructures");
        }
    }
}
