using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZenithBankATM.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnNameInTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValueCaptured",
                table: "Transactions",
                newName: "ValueDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValueDate",
                table: "Transactions",
                newName: "ValueCaptured");
        }
    }
}
