using BackEnd.Model;
using employeeproject.Model;
using Microsoft.EntityFrameworkCore;

namespace HRDashboardApplication.Model;

public class DataContext : DbContext {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<Employee>? EmployeeList { get; set; }
    public DbSet<Shift>? ShiftList { get; set; }
    public DbSet<Notification> NotificationsList { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresEnum<Status>();
        modelBuilder.Entity<Employee>().Property(p => p.Id).HasIdentityOptions(startValue: 4);
        modelBuilder.Entity<Employee>().HasData(
            new Employee {
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
            },
            new Employee {
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
            },
            new Employee {
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
            });
        
        modelBuilder.Entity<Shift>().Property(p => p.Id).HasIdentityOptions(startValue: 3);
        modelBuilder.Entity<Shift>().HasData(
            new Shift {
                Id = 1,
                Title = "Evening shift",
                Date = "2023-11-21",
                StartTime = "13:00",
                EndTime = "21:00",
            },
            new Shift {
                Id = 2,
                Title = "Morning shift",
                Date = "2023-11-16",
                StartTime = "8:00",
                EndTime = "16:00",
            },
            new Shift {
                Id = 3,
                Title = "All day shift",
                Date = "2023-11-09",
                StartTime = "13:00",
                EndTime = "21:00",
            });

        modelBuilder.Entity<EmployeeShift>().HasKey(key => new { key.EmployeeId, key.ShiftId });
        modelBuilder.Entity<EmployeeShift>().HasData(
            new EmployeeShift() { EmployeeId = 1, ShiftId = 1 },
            new EmployeeShift() { EmployeeId = 2, ShiftId = 2 },
            new EmployeeShift() { EmployeeId = 3, ShiftId = 3 });
        
        modelBuilder.Entity<Notification>().Property(p => p.NotificationId).HasIdentityOptions(startValue: 4);
        modelBuilder.Entity<Notification>().HasData(
            new Notification {
                NotificationId = 1,
                Message = "what happened",
                Date = new DateTime(),
                Type = "Employee Update"
            },
            new Notification {
                NotificationId = 2,
                Message = "what happened",
                Date = new DateTime(),
                Type = "Employee Update"
            },
            new Notification {
                NotificationId = 3,
                Message = "what",
                Date = new DateTime(),
                Type = "Shift Change"
            });
            
    }
}
 