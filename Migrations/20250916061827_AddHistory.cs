using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class AddHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookReaders",
                table: "BookReaders");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BookReaders",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckedOutAt",
                table: "BookReaders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedAt",
                table: "BookReaders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookReaders",
                table: "BookReaders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookReaders_BookId",
                table: "BookReaders",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookReaders",
                table: "BookReaders");

            migrationBuilder.DropIndex(
                name: "IX_BookReaders_BookId",
                table: "BookReaders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BookReaders");

            migrationBuilder.DropColumn(
                name: "CheckedOutAt",
                table: "BookReaders");

            migrationBuilder.DropColumn(
                name: "ReturnedAt",
                table: "BookReaders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookReaders",
                table: "BookReaders",
                columns: new[] { "BookId", "ReaderId" });
        }
    }
}
