//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Infrastructure.Data;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;

//public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//{
//    public AppDbContext CreateDbContext(string[] args)
//    {
//        var path = Path.Combine(Directory.GetCurrentDirectory(), "../API");

//        var config = new ConfigurationBuilder()
//            .SetBasePath(path)
//            .AddJsonFile("appsettings.json")
//            .Build();

//        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

//        optionsBuilder.UseSqlServer(
//            config.GetConnectionString("DefaultConnection"));

//        return new AppDbContext(optionsBuilder.Options);
//    }
//}