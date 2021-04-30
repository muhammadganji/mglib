# Code first in Asp.net core and React
---
* Create new Template: **Asp.net core Web with React**
* Install Library from **Nuget Package Manager**:
```sh
Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
Microsoft.AspNetCore.SpaServices.Extensions
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
```
* Create model and Context and Initial seed data for database:
`Person.cs` , `AppDbContext.cs`, 

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


