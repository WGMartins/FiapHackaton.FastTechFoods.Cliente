using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FastTechFoodsCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurante",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    CardapioId = table.Column<Guid>(type: "uuid", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurante", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",

                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    SenhaHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cardapio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RestauranteId = table.Column<Guid>(type: "uuid", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cardapio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cardapio_Restaurante_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RestauranteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FormaDeEntrega = table.Column<int>(type: "integer", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedido_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedido_Restaurante_RestauranteId",
                        column: x => x.RestauranteId,
                        principalTable: "Restaurante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemDeCardapio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardapioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDeCardapio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDeCardapio_Cardapio_CardapioId",
                        column: x => x.CardapioId,
                        principalTable: "Cardapio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemDePedido",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AlteradoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDePedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemDePedido_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cardapio_RestauranteId",
                table: "Cardapio",
                column: "RestauranteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemDeCardapio_CardapioId",
                table: "ItemDeCardapio",
                column: "CardapioId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDePedido_PedidoId",
                table: "ItemDePedido",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_ClienteId",
                table: "Pedido",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_RestauranteId",
                table: "Pedido",
                column: "RestauranteId");

            migrationBuilder.Sql("INSERT INTO \"Restaurante\"(\"Id\", \"Nome\", \"CardapioId\", \"CriadoEm\", \"AlteradoEm\") VALUES('fa00c0cb-9c6d-4410-877b-5dc403a2aee9', 'RESTAURANTE CENTRO', 'd5dad53a-56c8-4f65-abd1-27209d99bdc5', LOCALTIMESTAMP, null)");
            migrationBuilder.Sql("INSERT INTO \"Usuario\"(\"Id\", \"Email\", \"Cpf\", \"SenhaHash\", \"Role\", \"CriadoEm\", \"AlteradoEm\") VALUES(gen_random_uuid(), 'cliente@teste.com', '02051673063', '$2a$11$s7x5kOb0gUsP6v8cptIaF..RW84lfsKxBQ7nHJNhTMI/EpqtEjYu.', 'Cliente', LOCALTIMESTAMP, null)");
            migrationBuilder.Sql("INSERT INTO \"Usuario\"(\"Id\", \"Email\", \"Cpf\", \"SenhaHash\", \"Role\", \"CriadoEm\", \"AlteradoEm\") VALUES(gen_random_uuid(), 'gerente@teste.com', null, '$2a$11$s7x5kOb0gUsP6v8cptIaF..RW84lfsKxBQ7nHJNhTMI/EpqtEjYu.', 'Gerente', '20250716', null )");
            migrationBuilder.Sql("INSERT INTO \"Usuario\"(\"Id\", \"Email\", \"Cpf\", \"SenhaHash\", \"Role\", \"CriadoEm\", \"AlteradoEm\") VALUES(gen_random_uuid(), 'atendente@teste.com', null, '$2a$11$s7x5kOb0gUsP6v8cptIaF..RW84lfsKxBQ7nHJNhTMI/EpqtEjYu.', 'Atendente', '20250716', null )");
            migrationBuilder.Sql("INSERT INTO \"Cliente\"(\"Id\", \"Nome\", \"CriadoEm\", \"AlteradoEm\") VALUES('638acaf4-7d8e-4302-9f27-996fcea9c9f6', 'FULANO DA SILVA', LOCALTIMESTAMP, null)");
            migrationBuilder.Sql("INSERT INTO \"Cardapio\"(\"Id\", \"RestauranteId\", \"CriadoEm\", \"AlteradoEm\") VALUES('d5dad53a-56c8-4f65-abd1-27209d99bdc5', 'fa00c0cb-9c6d-4410-877b-5dc403a2aee9', LOCALTIMESTAMP, null)");
            migrationBuilder.Sql("INSERT INTO \"ItemDeCardapio\"(\"Id\", \"CardapioId\", \"Nome\", \"Valor\", \"Descricao\", \"Tipo\", \"CriadoEm\", \"AlteradoEm\") VALUES('c60b730a-7c4b-4821-87c5-5785eb1e4929', 'd5dad53a-56c8-4f65-abd1-27209d99bdc5', 'X-BURGER', '29.90', 'HAMBURGUER DE CARNE BOVINA', 1, LOCALTIMESTAMP, null)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemDeCardapio");

            migrationBuilder.DropTable(
                name: "ItemDePedido");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Cardapio");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Restaurante");
        }
    }
}
