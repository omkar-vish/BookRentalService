using BookRentalService.Model;
using Hangfire;
using Hangfire.Console;

public static class ServiceCollectionExtensions
{
    public static void AppDBInitialiser(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<BookRentalDbContext>();
            bool IsDbsaveRequired = false;
            try
            {
                if (!context.Books.Any())
                {
                    var books = new List<Book>
            {
                new Book
            {
                Id = 1,
                Title = "The Great Gatsby",
                Author = "F. Scott Fitzgerald",
                ISBN = "9780743273565",
                Genre = "Classics",
                IsAvailable = true,
                TimesRented = 0,
                RowVersion = DateTime.UtcNow
            },
            new Book
            {
                Id = 2,
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                ISBN = "9780060935467",
                Genre = "Classics",
                IsAvailable = true,
                TimesRented = 0,
                RowVersion = DateTime.UtcNow
            },
            new Book
            {
                Id = 3,
                Title = "1984",
                Author = "George Orwell",
                ISBN = "9780451524935",
                Genre = "Dystopian",
                IsAvailable = true,
                TimesRented = 0,
                RowVersion = DateTime.UtcNow
            },
            new Book
            {
                Id = 4,
                Title = "Pride and Prejudice",
                Author = "Jane Austen",
                ISBN = "9780141199078",
                Genre = "Romance",
                IsAvailable = true,
                TimesRented = 0,
                RowVersion = DateTime.UtcNow
            },
            new Book
            {
                Id = 5,
                Title = "The Catcher in the Rye",
                Author = "J.D. Salinger",
                ISBN = "9780316769488",
                Genre = "Classics",
                IsAvailable = true,
                TimesRented = 0,
                RowVersion = DateTime.UtcNow
            },
            new Book
            {
                Id = 6,
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                ISBN = "9780547928227",
                Genre = "Fantasy",
                IsAvailable = true,
                TimesRented = 0,
                RowVersion = DateTime.UtcNow
            },
            new Book
            {
                Id = 7,
                Title = "Fahrenheit 451",
                Author = "Ray Bradbury",
                ISBN = "9781451673319",
                Genre = "Science Fiction",
                IsAvailable = true,
                TimesRented = 0,
                RowVersion = DateTime.UtcNow
            },
            new Book
            {
                Id = 8,
                Title = "The Book Thief",
                Author = "Markus Zusak",
                ISBN = "9780375842207",
                Genre = "Historical Fiction",
                IsAvailable = true,
                TimesRented = 0,
                RowVersion = DateTime.UtcNow
            },
            new Book
            {
                Id = 9,
                Title = "Moby-Dick",
                Author = "Herman Melville",
                ISBN = "9781503280786",
                Genre = "Classics",
                IsAvailable = true,
                TimesRented = 0,
                RowVersion = DateTime.UtcNow
            },
            new Book
            {
                Id = 10,
                Title = "War and Peace",
                Author = "Leo Tolstoy",
                ISBN = "9781400079988",
                Genre = "Historical Fiction",
                IsAvailable = true,
                TimesRented = 0,
                RowVersion = DateTime.UtcNow
            }


            };
                    context.Books.AddRange(books);
                    IsDbsaveRequired = true;
                }

                if (!context.Users.Any())
                {
                    var users = new List<User>
            {
                 new User
            {
                Id = 1,
                Name = "Alice Johnson",
                Email = "alice.johnson@example.com",
                Rentals = new List<Rental>()
            },
            new User
            {
                Id = 2,
                Name = "Bob Smith",
                Email = "bob.smith@example.com",
                Rentals = new List<Rental>()
            },
            new User
            {
                Id = 3,
                Name = "Carol Williams",
                Email = "carol.williams@example.com",
                Rentals = new List<Rental>()
            },
            new User
            {
                Id = 4,
                Name = "David Brown",
                Email = "david.brown@example.com",
                Rentals = new List<Rental>()
            },
            new User
            {
                Id = 5,
                Name = "Eve Davis",
                Email = "eve.davis@example.com",
                Rentals = new List<Rental>()
            }
            };
                    context.Users.AddRange(users);
                    IsDbsaveRequired = true;
                }

                if (IsDbsaveRequired)
                {
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The Seed data initializer fails to run due to exception  " + ex);
            }
        }
    }

    public static void ConfigureHangfire(this IServiceCollection services)
    {
        services.AddHangfire(configuration =>
        {
            configuration.UseInMemoryStorage();
            configuration.UseConsole();

        });
        services.AddHangfireServer(options =>
        {
            options.SchedulePollingInterval = TimeSpan.FromSeconds(5);
            options.WorkerCount = 1;
        });

        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
        {
            Attempts = 2
        });
    }

    public static void ConfigureRecurringJobs(this WebApplication app)
    {
        RecurringJob.AddOrUpdate<MarkOverDueBookRentalsRecurringJob>(
            MarkOverDueBookRentalsRecurringJob.JobId,
            x => x.ExecuteMarkOverdueRentalsAsync(),
            "0 0 * * *");

        RecurringJob.AddOrUpdate<SendNotificationOfOverDueRecurringJob>(
            MarkOverDueBookRentalsRecurringJob.JobId,
            x => x.ExecuteSendNotificationOfOverDueAsync(),
            "0 0 * * *");

        RecurringJob.TriggerJob(MarkOverDueBookRentalsRecurringJob.JobId);
        RecurringJob.TriggerJob(SendNotificationOfOverDueRecurringJob.JobId);

    }
}