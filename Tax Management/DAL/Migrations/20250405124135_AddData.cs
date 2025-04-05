using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into TaxRates(Municipality, StartDate, EndDate, Rate, Type)
                                    values('Copenhagen', '2024-01-01T00:00:00', '2024-12-31T00:00:00', 0.2 , 3),
                                          ('Copenhagen', '2024-05-01T00:00:00', '2024-05-31T00:00:00', 0.4, 2),
                                          ('Copenhagen', '2024-01-01T00:00:00', '2024-01-01T00:00:00', 0.1, 0),
                                          ('Copenhagen', '2024-12-31T00:00:00', '2024-12-31T00:00:00', 0.1, 0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
