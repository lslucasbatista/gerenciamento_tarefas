using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GerenciamentoProjeto.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "descricao",
                table: "historico",
                newName: "comentario");

            migrationBuilder.RenameColumn(
                name: "data_criacao",
                table: "historico",
                newName: "data_modificacao");

            migrationBuilder.AddColumn<int>(
                name: "cargo_id",
                table: "usuario",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "usuario",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "data_atualizacao",
                table: "tarefa",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "usuario_id",
                table: "tarefa",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "alteracao",
                table: "historico",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "original",
                table: "historico",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "cargo",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descricao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargo", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_usuario_cargo_id",
                table: "usuario",
                column: "cargo_id");

            migrationBuilder.CreateIndex(
                name: "IX_tarefa_usuario_id",
                table: "tarefa",
                column: "usuario_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tarefa_usuario_usuario_id",
                table: "tarefa",
                column: "usuario_id",
                principalTable: "usuario",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_usuario_cargo_cargo_id",
                table: "usuario",
                column: "cargo_id",
                principalTable: "cargo",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tarefa_usuario_usuario_id",
                table: "tarefa");

            migrationBuilder.DropForeignKey(
                name: "FK_usuario_cargo_cargo_id",
                table: "usuario");

            migrationBuilder.DropTable(
                name: "cargo");

            migrationBuilder.DropIndex(
                name: "IX_usuario_cargo_id",
                table: "usuario");

            migrationBuilder.DropIndex(
                name: "IX_tarefa_usuario_id",
                table: "tarefa");

            migrationBuilder.DropColumn(
                name: "cargo_id",
                table: "usuario");

            migrationBuilder.DropColumn(
                name: "email",
                table: "usuario");

            migrationBuilder.DropColumn(
                name: "data_atualizacao",
                table: "tarefa");

            migrationBuilder.DropColumn(
                name: "usuario_id",
                table: "tarefa");

            migrationBuilder.DropColumn(
                name: "alteracao",
                table: "historico");

            migrationBuilder.DropColumn(
                name: "original",
                table: "historico");

            migrationBuilder.RenameColumn(
                name: "data_modificacao",
                table: "historico",
                newName: "data_criacao");

            migrationBuilder.RenameColumn(
                name: "comentario",
                table: "historico",
                newName: "descricao");
        }
    }
}
