using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YolaGuide.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id_category = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubcategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.id_category);
                    table.ForeignKey(
                        name: "subcategory",
                        column: x => x.SubcategoryId,
                        principalTable: "category",
                        principalColumn: "id_category");
                });

            migrationBuilder.CreateTable(
                name: "fact",
                columns: table => new
                {
                    id_fact = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fact", x => x.id_fact);
                });

            migrationBuilder.CreateTable(
                name: "place",
                columns: table => new
                {
                    id_place = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    contact_information = table.Column<string>(type: "text", nullable: false),
                    adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    route_to_image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    yid_org = table.Column<long>(type: "bigint", nullable: false),
                    coordinates = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_place", x => x.id_place);
                });

            migrationBuilder.CreateTable(
                name: "route",
                columns: table => new
                {
                    id_route = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    cost = table.Column<decimal>(type: "money", nullable: false),
                    telephone = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_route", x => x.id_route);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id_user = table.Column<long>(type: "bigint", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false),
                    substate = table.Column<int>(type: "int", nullable: false),
                    language = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id_user);
                });

            migrationBuilder.CreateTable(
                name: "place_has_category",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    PlacesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_place_has_category", x => new { x.CategoriesId, x.PlacesId });
                    table.ForeignKey(
                        name: "FK_place_has_category_category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "category",
                        principalColumn: "id_category",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_place_has_category_place_PlacesId",
                        column: x => x.PlacesId,
                        principalTable: "place",
                        principalColumn: "id_place",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "route_has_place",
                columns: table => new
                {
                    PlacesId = table.Column<int>(type: "int", nullable: false),
                    RoutesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_route_has_place", x => new { x.PlacesId, x.RoutesId });
                    table.ForeignKey(
                        name: "FK_route_has_place_place_PlacesId",
                        column: x => x.PlacesId,
                        principalTable: "place",
                        principalColumn: "id_place",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_route_has_place_route_RoutesId",
                        column: x => x.RoutesId,
                        principalTable: "route",
                        principalColumn: "id_route",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_has_category",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_has_category", x => new { x.CategoriesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_user_has_category_category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "category",
                        principalColumn: "id_category",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_has_category_user_UsersId",
                        column: x => x.UsersId,
                        principalTable: "user",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_has_place",
                columns: table => new
                {
                    PlacesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_has_place", x => new { x.PlacesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_user_has_place_place_PlacesId",
                        column: x => x.PlacesId,
                        principalTable: "place",
                        principalColumn: "id_place",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_has_place_user_UsersId",
                        column: x => x.UsersId,
                        principalTable: "user",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_has_route",
                columns: table => new
                {
                    RoutesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_has_route", x => new { x.RoutesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_user_has_route_route_RoutesId",
                        column: x => x.RoutesId,
                        principalTable: "route",
                        principalColumn: "id_route",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_has_route_user_UsersId",
                        column: x => x.UsersId,
                        principalTable: "user",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_category_SubcategoryId",
                table: "category",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_place_has_category_PlacesId",
                table: "place_has_category",
                column: "PlacesId");

            migrationBuilder.CreateIndex(
                name: "IX_route_has_place_RoutesId",
                table: "route_has_place",
                column: "RoutesId");

            migrationBuilder.CreateIndex(
                name: "IX_user_has_category_UsersId",
                table: "user_has_category",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_user_has_place_UsersId",
                table: "user_has_place",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_user_has_route_UsersId",
                table: "user_has_route",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fact");

            migrationBuilder.DropTable(
                name: "place_has_category");

            migrationBuilder.DropTable(
                name: "route_has_place");

            migrationBuilder.DropTable(
                name: "user_has_category");

            migrationBuilder.DropTable(
                name: "user_has_place");

            migrationBuilder.DropTable(
                name: "user_has_route");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "place");

            migrationBuilder.DropTable(
                name: "route");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
