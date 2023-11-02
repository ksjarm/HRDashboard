using BackEnd.Model;
using employeeproject.Model;
using Microsoft.EntityFrameworkCore;

namespace WorkoutApplication.Model;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }
    public DbSet<Employee>? EmployeeList { get; set; }
    public DbSet<Shift>? ShiftList { get; set; }
    public DbSet<Notification> NotificationsList { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresEnum<Status>();

        modelBuilder.Entity<Employee>().Property(p => p.Id).HasIdentityOptions(startValue: 4);

        modelBuilder.Entity<Employee>().HasOne(e => e.Shift).WithOne(s => s.Employee).HasForeignKey<Shift>(s => s.EmployeeId);

        modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                Name = "Erik",
                Surname = "Taam",
                Gender = Gender.Male,
                DateOfBirth = "13.09.1997",
                Email = "erik.taam@company.com",
                PhoneNumber = "55648836",
                Adress = "A.H.Tammsaare tee 56-13",
                Position = "Warehouse worker",
                Salary = 1200,
                Status = Status.Active,
                ShiftId = 1
            },
            new Employee
            {
                Id = 2,
                Name = "Mari",
                Surname = "Saar",
                Gender = Gender.Female,
                DateOfBirth = "18.03.1984",
                Email = "mari.saar@company.com",
                PhoneNumber = "56789432",
                Adress = "Akadeemia tee 23-15",
                Position = "Vendor",
                Salary = 1500,
                Status = Status.OnMaternityLeave,
                ShiftId = 2
            },
            new Employee
            {
                Id = 3,
                Name = "Rene",
                Surname = "Dall",
                Email = "rene.dall@company.com",
                Gender = Gender.Male,
                DateOfBirth = "06.01.2001",
                PhoneNumber = "58762309",
                Adress = "E.Vilde tee 56-12",
                Position = "Vendor",
                Salary = 1500,
                Status = Status.Active,
                ShiftId = 3
            });
        
        modelBuilder.Entity<Shift>().Property(p => p.Id).HasIdentityOptions(startValue: 3);
        modelBuilder.Entity<Shift>().HasData(
            new Shift {
                Id = 1,
                Title = "Evening shift",
                Date = DateTime.Now.Date,
                StartTime = new TimeSpan(13, 0, 0),
                EndTime = new TimeSpan(21, 0, 0),
                EmployeeId = 1
            },
            new Shift {
                Id = 2,
                Title = "Morning shift",
                Date = DateTime.Now.Date,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
                EmployeeId = 2
            },
            new Shift {
                Id = 3,
                Title = "All day shift",
                Date = new DateTime(2023, 12, 1).Date,
                StartTime = new TimeSpan(13, 0, 0),
                EndTime = new TimeSpan(21, 0, 0),
                EmployeeId = 3
            });
        
        modelBuilder.Entity<Notification>().Property(p => p.NotificationId).HasIdentityOptions(startValue: 3);

        modelBuilder.Entity<Notification>().HasData(
            new Notification {
                NotificationId = 1,
                Message = "what happened",
                Date = new DateTime(),
                Type = "Employee Update"
            },
            new Notification {
                NotificationId = 2,
                Message = "what",
                Date = new DateTime(),
                Type = "Shift Change"
            });
    }
}
