using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class DateBorrow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReaders_Books_BookId",
                table: "BookReaders");

            migrationBuilder.DropForeignKey(
                name: "FK_BookReaders_Readers_ReaderId",
                table: "BookReaders");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "BookReaders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Fine",
                table: "BookReaders",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookReaders_Books_BookId",
                table: "BookReaders",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookReaders_Readers_ReaderId",
                table: "BookReaders",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReaders_Books_BookId",
                table: "BookReaders");

            migrationBuilder.DropForeignKey(
                name: "FK_BookReaders_Readers_ReaderId",
                table: "BookReaders");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "BookReaders");

            migrationBuilder.DropColumn(
                name: "Fine",
                table: "BookReaders");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReaders_Books_BookId",
                table: "BookReaders",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookReaders_Readers_ReaderId",
                table: "BookReaders",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
