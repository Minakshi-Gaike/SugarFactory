using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeariumTraders.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BulkEnquiries",
                columns: table => new
                {
                    EnquiryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequiredQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityUnit = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DeliveryLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RequirementDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnquiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulkEnquiries", x => x.EnquiryId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BulkEnquiries");
        }
    }
}
