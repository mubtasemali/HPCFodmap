using Duende.IdentityServer.EntityFramework.Options;
using HPCFodmapProject.Server.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HPCFodmapProject.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)

        {
        }

        //adding this breaks the log in but this is needed for adding the tables
        // adding to add table
     
        public DbSet<ApplicationUser> ApplicationUser => Set<ApplicationUser>();

        public DbSet<Food> Food => Set<Food>();

        public DbSet<FoodIngredients> FoodIngredients => Set<FoodIngredients>();

        public DbSet<Ingredients> Ingredients => Set<Ingredients>();

        public DbSet<Intake> Intake => Set<Intake>();

        public DbSet<WhiteList> WhiteList => Set<WhiteList>();


    }
}