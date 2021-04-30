# Code first in Asp.net core and React
* Create new Template: **Asp.net core Web with React**
* Install Library from **Nuget Package Manager**:
```sh
Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
Microsoft.AspNetCore.SpaServices.Extensions
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
```
* Create model and Context and Initial seed data for database in folder `Models`:
`Person` , `AppDbContext`, `DbInitializer`

## Person.cs
```c#
public class Person
    {
        [Key]
        public int IdPerson { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
```

## AppDbContext.cs
```c#
public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        public DbSet<Person> persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("persons");
        }
    }
```

## DbInitializer.cs
```c#
public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.persons.Any())
            {
                return;   // DB has been seeded
            }

            var personsList = new Person[]
            {
            new Person{FirstName="Carson",LastName="Alexander"},
            new Person{FirstName="Meredith",LastName="Alonso"},
            new Person{FirstName="Arturo",LastName="Anand"},
            new Person{FirstName="Gytis",LastName="Barzdukas"},
            new Person{FirstName="Yan",LastName="Li"},
            new Person{FirstName="Peggy",LastName="Justice"},
            new Person{FirstName="Laura",LastName="Norman"},
            new Person{FirstName="Nino",LastName="Olivetto"}
            };
            foreach (Person p in personsList)
            {
                context.persons.Add(p);
            }
            context.SaveChanges();
            
        }
    }
```
* set ConnectoinString in `appsettings.json`:
```json
"ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost;Initial Catalog=DATABASENAME;User Id=USERNAME;Password=PASSWORD"
  }
```
* add database context to `Starup.cs`:
```c#
public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }
```


* Write `Program.cs` with this instruction:
```c#
public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateDbIfNotExists(host);
            host.Run();
        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
    }
```
---
# Plesk setting for database:
after publish Application and zip it then, 
* upload zip to folder of `httpdoc` and unzip it.
* create new database with these Setting: name `DATABASENAME` and username `USERNAME` and password `PASSWORD`

---
**Enjoy from coding**
این چالش نزدیک به دو ماه ازم وقت گرفت
دهم اردیبهشت 1400

