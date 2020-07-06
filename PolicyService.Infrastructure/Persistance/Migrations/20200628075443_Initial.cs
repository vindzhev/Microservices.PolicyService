using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PolicyService.Infrastructure.Persistance.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<Guid>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    AgentLogin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ValidityPeriod",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ValidFrom = table.Column<DateTime>(nullable: false),
                    ValidTo = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidityPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PolicyHolder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Pesel = table.Column<string>(nullable: true),
                    AddressId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyHolder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyHolder_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    PolicyValidityPeriodId = table.Column<Guid>(nullable: true),
                    PolicyHolderId = table.Column<Guid>(nullable: true),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    AgentLogin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_PolicyHolder_PolicyHolderId",
                        column: x => x.PolicyHolderId,
                        principalTable: "PolicyHolder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Offers_ValidityPeriod_PolicyValidityPeriodId",
                        column: x => x.PolicyValidityPeriodId,
                        principalTable: "ValidityPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PolicyVersion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PolicyId = table.Column<Guid>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    PolicyHolderId = table.Column<Guid>(nullable: true),
                    CoverPeriodId = table.Column<Guid>(nullable: true),
                    VersionValidityPeriodId = table.Column<Guid>(nullable: true),
                    TotalPremiumAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyVersion_ValidityPeriod_CoverPeriodId",
                        column: x => x.CoverPeriodId,
                        principalTable: "ValidityPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PolicyVersion_PolicyHolder_PolicyHolderId",
                        column: x => x.PolicyHolderId,
                        principalTable: "PolicyHolder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PolicyVersion_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PolicyVersion_ValidityPeriod_VersionValidityPeriodId",
                        column: x => x.VersionValidityPeriodId,
                        principalTable: "ValidityPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cover",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    OfferId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cover", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cover_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PolicyCover",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Premium = table.Column<decimal>(nullable: false),
                    CoverPeriodId = table.Column<Guid>(nullable: true),
                    PolicyVersionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyCover", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyCover_ValidityPeriod_CoverPeriodId",
                        column: x => x.CoverPeriodId,
                        principalTable: "ValidityPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PolicyCover_PolicyVersion_PolicyVersionId",
                        column: x => x.PolicyVersionId,
                        principalTable: "PolicyVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cover_OfferId",
                table: "Cover",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_PolicyHolderId",
                table: "Offers",
                column: "PolicyHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_PolicyValidityPeriodId",
                table: "Offers",
                column: "PolicyValidityPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyCover_CoverPeriodId",
                table: "PolicyCover",
                column: "CoverPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyCover_PolicyVersionId",
                table: "PolicyCover",
                column: "PolicyVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyHolder_AddressId",
                table: "PolicyHolder",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyVersion_CoverPeriodId",
                table: "PolicyVersion",
                column: "CoverPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyVersion_PolicyHolderId",
                table: "PolicyVersion",
                column: "PolicyHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyVersion_PolicyId",
                table: "PolicyVersion",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyVersion_VersionValidityPeriodId",
                table: "PolicyVersion",
                column: "VersionValidityPeriodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cover");

            migrationBuilder.DropTable(
                name: "PolicyCover");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "PolicyVersion");

            migrationBuilder.DropTable(
                name: "ValidityPeriod");

            migrationBuilder.DropTable(
                name: "PolicyHolder");

            migrationBuilder.DropTable(
                name: "Policies");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
