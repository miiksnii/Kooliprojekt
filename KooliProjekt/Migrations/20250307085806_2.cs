using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kooliprojekt.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ProjectList");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ProjectList",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }
    }
}
