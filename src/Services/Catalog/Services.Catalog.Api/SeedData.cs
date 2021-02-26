using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services.Catalog.Api.DbContexts;
using Services.Catalog.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Catalog.Api
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {

            using (var dbContext = new CatalogDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<CatalogDbContext>>()))
            {
                if (dbContext.Trucks.Any())
                {
                    return;   // DB has been seeded
                }

                await PopulateTestData(dbContext);

            }
        }

        private static async Task PopulateTestData(CatalogDbContext dbContext)
        {
           await PopulateTrucks(dbContext);
            await PopulateCategories(dbContext);
            if (dbContext.Trucks.Any())
            {
                if (dbContext.Categories.Any())
                {
                    await PopulateTruckCategories(dbContext);
                }
                await PopulatePhotos(dbContext);
            }
        }

        private static async Task PopulateTrucks(CatalogDbContext dbContext)
        {
            string truckSql = "insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1986 Fire Truck', 1986, 'Chrome grill version. It has working headlights, taillights, and a flashing red light on the cab of the fire truck', 100.99, 2, '1986_1.jpg') " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1987 18 Wheeler Bank', 1987, 'It has clearance and running lights in addition to the headlights and taillights', 11.99, 7, '1987_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1988 Truck and Racer', 1988, 'It has marker lights, headlights, and taillights', 11.99, 5, '1988_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1989 Fire Truck Bank', 1989, 'It has headlights, taillights and emergency flashing lights in addition to a unique dual tone siren', 11.99, 2, '1989_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1990 Tanker Truck', 1990, 'It has 34 lights, lighted Hess logos, a backup alert along with a horn', 11.99, 2, '1990_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1991 Truck and Racer', 1991, 'It has 27 different red, white, and yellow lights', 11.99, 2, '1991_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1992 18 Wheeler and Racer', 1992, 'Lights...', 11.99, 3, '1992_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1994 Rescue Truck', 1994, 'Lights... . Sounds include a back up alert, truck horn, and an emergency siren ', 11.99, 22, '1994_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1995 Truck and Helicopter', 1995, 'Lights... Chopper has a searchlight, and flashing beacon lights ', 11.99, 22, '1995_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1996 Emergency Truck', 1996, 'It has a searchlight, flashers, emergency lights, a backup alert along with a siren', 11.99, 20, '1996_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1997 Truck and Racers', 1997, 'Lights... Racers have working headlights and taillights', 11.99, 17, '1997_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1998 Recreation Van', 1998, 'Rv has headlights, taillights, and market lights.', 11.99, 2, '1998_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 1999 Truck and Space Shuttle with Satellite', 1999, 'Truck has working headlights, taillights, and running lights. Shuttle has lights, sounds and a satellite with solar panels', 11.99, 18, '1999_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2000 Fire Truck', 2000, 'It has  working headlights, taillights and emergency flashing lights, a backup alert along with a horn', 11.99, 2, '2000_1.jpg'); " +
            " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2001 Helicopter With Motorcycle and Cruiser', 2001, 'Helicopter that has internal, external lights and emergency flasher', 11.99, 8, '2001_1.jpg'); " +
            "insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2002 Truck and Airplane', 2002, ' The truck has front and rear lights', 11.99, 9, '2002_1.jpg'); " +
            "insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2003 18 Wheeler Truck and Race Cars', 2003, 'The truck has running lights, front and rear lights, an internally lit trailer.', 11.99, 14, '2003_1.jpg'); " +
            "insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2004 Sport Utility Vehicle & Motorcycles', 2004, 'Lights... . Bikes have working lights', 11.99, 1, '2004_1.jpg'); " +
            "insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2005 Emergency Truck with Rescue Vehicle', 2005, 'Truck has front and rear lights, emergency flashing red lights, and three different emergency sirens sounds.', 11.99, 1, '2005_1.jpg'); " +
            "insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2006 Truck and Helicopter', 2006, 'Truck has front and rear lights and landing lights as well as 31 separate working lights on the trailer.', 11.99, 8, '2006_1.jpg'); " +
            "insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2008 Toy Truck & Front End Loader', 2008, 'Truck has headlights, taillights, and running lights. it also has a real-sounding ignition, back up alert, hydraulic arm sound, horn', 11.99, 1, '2008_1.jpg'); " +
            "insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2009 Toy Race Car and Racer', 2009, 'Car has working headlights, taillights, and running lights along with horn, ignition, and engine acceleration sounds', 11.99, 7, '2009_1.jpg'); " +
            "insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2011 Toy Truck and Racer', 2011, 'Truck has 34 functioning lights along with ignition, back up alert, hydraulic ramp and a horn sound.', 11.99, 4, '2011_1.jpg'); " +
           " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2012 Helicopter and Rescue SUV', 2012, 'The chopper has working lights along with searchlights. Sounds include, landing and take off', 11.99, 1, '2012_1.jpg'); " +
           " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2013 Truck & Tractor', 2013, 'The truck has  front and rear lights. Sounds include truck horn, ignition sound, beeping back up warning sound. Tractor has front and rear lights', 11.99, 1, '2013_1.jpg'); " +
           " insert into dbo.trucks(Name, Year, Description, Price, Quantity, DefaultPhotoPath) values('Hess 2014 Toy Truck & Space Cruiser With Scout', 2014, 'Truck has front, rear and running lights. Sounds include the truck starting, a horn, and the warning sound as the ramp comes out.', 11.99, 1, '2014_1.jpg'); ";

            await dbContext.Database.ExecuteSqlRawAsync(truckSql);
            //dbContext.Trucks.FromSqlRaw(truckSql);
        }

        private static async Task PopulateCategories(CatalogDbContext dbContext)
        {
            string categoriesSql = "insert into dbo.Categories(Name,IsMiniTruck, [Order]) values('1980s Hess Trucks',0, 1980) " +
               "insert into dbo.Categories(Name,IsMiniTruck, [Order]) values('1990s Hess Trucks',0, 1990) " +
               "insert into dbo.Categories(Name,IsMiniTruck, [Order]) values('2000s Hess Trucks',0, 2000) " +
               "insert into dbo.Categories(Name,IsMiniTruck, [Order]) values('2010s Hess Trucks',0, 2010) ";
            await dbContext.Database.ExecuteSqlRawAsync(categoriesSql);
         }

        static async Task<string> TruckCategorySqlByDecade(CatalogDbContext dbContext,int decadeStart)
        {
            var decadeTrucks =await dbContext.Trucks.Where (t => t.Year >= decadeStart && t.Year < decadeStart + 10).ToListAsync();
            var decadeCategory =await dbContext.Categories.FirstOrDefaultAsync(c => c.Order == decadeStart);
            StringBuilder sb = new StringBuilder();
            foreach (var truck in decadeTrucks)
            {
                var truckCategorySql = $"insert into CategoryTruck(CategoriesCategoryId, TrucksTruckId) values({decadeCategory.CategoryId}, '{truck.TruckId}');";
                sb.Append(truckCategorySql);
            }
             return sb.ToString();
        }

        private static async Task PopulateTruckCategories(CatalogDbContext dbContext)
        {
            int[] decades = new int[4] { 1980, 1990, 2000, 2010 };
            for (int i = 0; i < decades.Length; i++)
            {
                var truckCategorySql = await TruckCategorySqlByDecade(dbContext, decades[i]);
                await dbContext.Database.ExecuteSqlRawAsync(truckCategorySql);
            }
        }

        private static async Task PopulatePhotos(CatalogDbContext dbContext)
        {
            foreach(var truck in dbContext.Trucks)
            {
                var photoSql = $"insert into photos(photoPath, TruckId) values('/images{truck.Year}_1.jpg','{truck.TruckId}') insert into photos(photoPath, TruckId) values('/images{truck.Year}_2.jpg','{truck.TruckId}')";
                await dbContext.Database.ExecuteSqlRawAsync(photoSql);
            }
        }
    }
}
