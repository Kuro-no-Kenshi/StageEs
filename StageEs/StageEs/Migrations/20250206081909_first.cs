using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StageEs.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RagioneSociale = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PIVA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodFisc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Citta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Via = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "TestataDocumenti",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataDocumento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestataDocumenti", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_TestataDocumenti_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RigaDocumenti",
                columns: table => new
                {
                    RigaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentoId = table.Column<int>(type: "int", nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantita = table.Column<int>(type: "int", nullable: false),
                    TestataDocumentoDocumentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RigaDocumenti", x => x.RigaId);
                    table.ForeignKey(
                        name: "FK_RigaDocumenti_TestataDocumenti_TestataDocumentoDocumentId",
                        column: x => x.TestataDocumentoDocumentId,
                        principalTable: "TestataDocumenti",
                        principalColumn: "DocumentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RigaDocumenti_TestataDocumentoDocumentId",
                table: "RigaDocumenti",
                column: "TestataDocumentoDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TestataDocumenti_CustomerId",
                table: "TestataDocumenti",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RigaDocumenti");

            migrationBuilder.DropTable(
                name: "TestataDocumenti");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
