# Library of Books


Help people to borrow book faster the than normal statue.
We can search book in library then request, after that We can go to library and got that book.
1. Search Book
2. Pay loan price online

## Libraries :
○ ASP.Net Core 3.1
○ Entity framwork
○ Linq
○ Google authentication
○ OTP authentication
○ Pay Zarinpal
○ Bootstrap
○ jquery

## 1. Auto create data Base
~~~
public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var scop = host.Services.CreateScope())
            {
                var services = scop.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    // do something you can customize.
                    // For example, I will migrate the database.
                    context.Database.Migrate();
                    // seeding data base
                    SeedData.Seeding(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An Error occurred while seeding the data in database");
                }

            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().Build();

    }
~~~

