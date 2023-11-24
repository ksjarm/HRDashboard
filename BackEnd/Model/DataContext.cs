using BackEnd.Model;
using employeeproject.Model;
using Microsoft.EntityFrameworkCore;

namespace HRDashboardApplication.Model;

public class DataContext : DbContext {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    public DbSet<Employee>? EmployeeList { get; set; }
    public DbSet<Shift>? ShiftList { get; set; }
    public DbSet<Notification> NotificationsList { get; set; }
    public DbSet<EmployeeShift> EmployeeShiftsList { get; set; }
    public DbSet<User> UserList { get; set; }
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
                Photo = "../assets/profileimg2.png"
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
                Date = "2023-12-21",
                StartTime = "13:00",
                EndTime = "21:00",
                Valik= Valik.Onetime,
                
            },
            new Shift {
                Id = 2,
                Title = "Morning shift",
                Date = "2023-12-16",
                StartTime = "8:00",
                EndTime = "16:00",
                Valik=Valik.Onetime,
            },
            new Shift {
                Id = 3,
                Title = "Morning shift",
                Date = "2023-11-16",
                StartTime = "8:00",
                EndTime = "16:00",
                Valik=Valik.Onetime,
            },
            new Shift {
                Id = 4,
                Title = "Morning shift",
                Date = "2023-11-04",
                StartTime = "8:00",
                EndTime = "16:00",
                Valik=Valik.Onetime,
            },
            new Shift {
                Id = 5,
                Title = "All day shift",
                Date = "2023-12-09",
                StartTime = "13:00",
                EndTime = "21:00",
                Valik=Valik.Onetime,
            },
             new Shift {
                Id = 6,
                Title = "All day shift",
                Date = "2023-12-09",
                StartTime = "13:00",
                EndTime = "21:00",
                Valik=Valik.Onetime,
            });

        modelBuilder.Entity<EmployeeShift>().HasKey(key => new { key.EmployeeId, key.ShiftId });
        modelBuilder.Entity<EmployeeShift>().HasData(
            new EmployeeShift { EmployeeId = 1, ShiftId = 1 },
            new EmployeeShift { EmployeeId = 1, ShiftId = 3 },
            new EmployeeShift { EmployeeId = 1, ShiftId = 4 },
            new EmployeeShift { EmployeeId = 1, ShiftId = 5 },
            new EmployeeShift { EmployeeId = 2, ShiftId = 2 },
            new EmployeeShift { EmployeeId = 3, ShiftId = 6 });
        
        modelBuilder.Entity<Notification>().Property(p => p.NotificationId).HasIdentityOptions(startValue: 4);
        modelBuilder.Entity<Notification>().HasData(
            new Notification {
                NotificationId = 1,
                Message = "Annual performance reviews coming up",
                Date = new DateTime(2023, 11, 1, 14, 0, 0),
                Type = "Reminder"
            },
            new Notification {
                NotificationId = 2,
                Message = "You changed employee{id} adress",
                Date = new DateTime(2023, 11, 2, 9, 30, 0),
                Type = "Employee Update"
            },
            new Notification {
                NotificationId = 3,
                Message = "You changed employee{id} shift ",
                Date = new DateTime(2023, 11, 3, 16, 45, 0),
                Type = "Shift Change"
            });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Lianna",
                    Surname = "Tamm",
                    PhoneNumber = 56069403,
                    Adress = "Puskini 55-32",
                    Username = "lianna.tamm@company.ee",
                    Password = "St9tpNN2zrinRGNUgKWCy4JjZRFEorSQ0Zg3a/8m7k4=",
                    Role = "Staff HR"
                },
                new User
                {
                    Id = 2,
                    Name = "Irina",
                    Surname = "Loki",
                    PhoneNumber =55630285,
                    Adress = "E.Vilde 23-12",
                    Username = "irina.loki@company.ee",
                    Password = "zWoe4T9h2Hj9G4dyUtWwcKwV6zMR1Q0yr3Uch+xSze8=", // test2
                    Role = "Junior HR"
                }
            );


            
    }
}
 