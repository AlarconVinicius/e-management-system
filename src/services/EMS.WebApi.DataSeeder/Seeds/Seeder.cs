using Bogus;
using Bogus.Extensions.Brazil;
using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EMS.WebApi.DataSeeder.Seeds;
public class Seeder : ISeeder
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IPlanRepository _planRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IServiceAppointmentRepository _serviceAppointmentRepository;
    private readonly Faker _faker;
    public Seeder(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IEmployeeRepository employeeRepository, IPlanRepository planRepository, ICompanyRepository companyRepository, IClientRepository clientRepository, IServiceRepository serviceRepository, IServiceAppointmentRepository serviceAppointmentRepository)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _employeeRepository = employeeRepository;
        _planRepository = planRepository;
        _companyRepository = companyRepository;
        _clientRepository = clientRepository;
        _serviceRepository = serviceRepository;
        _serviceAppointmentRepository = serviceAppointmentRepository;
        _faker = new Faker("pt_BR");
    }

    public async Task<List<Plan>> GenerateDefaultPlans()
    {
        // Generate Default Plans
        var plans = new List<Plan>
        {
            new Plan
            {
                Id = new Guid("7d67df76-2d4e-4a47-a19c-08eb80a9060b"),
                Title = "Plano Básico",
                Subtitle = "Ideal para profissionais liberais",
                Price = 15.00M,
                Benefits = "Clientes ilimitados,Agendamentos,Produtos Ilimitados,Notificação por e-mail,Controle Financeiro",
                IsActive = true
            },
            new Plan
            {
                Id = new Guid("78162be3-61c4-4959-89d7-5ebfb476427e"),
                Title = "Plano Intermediário",
                Subtitle = "Ideal para pequenas clinicas e Petshop",
                Price = 35.00M,
                Benefits = "Tudo do plano básico,+ Notificação por SMS (500/mes),+Gerencia dos colaboradores (até 10)",
                IsActive = true
            },
            new Plan
            {
                Id = new Guid("6ecaaa6b-ad9f-422c-b3bb-6013ec27b4bb"),
                Title = "Plano Avançado",
                Subtitle = "Ideal para clinicas, dentistas, petshop entre outros",
                Price = 60.00M,
                Benefits = "Todo do plano Intermediário,+ Notificação via Whatsapp (1000/mes),+Integração de pagamento com Stripe, MercadoPago e PagSeguro",
                IsActive = true
            }
        };
        await _planRepository.AddRangeAsync(plans);
        return plans;
    }
    public async Task<Guid> GenerateDefaultCompany(Guid planId)
    {
        // Generate Default Company
        var company = new Company(planId, "E-Management System", "EMS", _faker.Person.Cpf(false), true);
        await _companyRepository.AddAsync(company);

        return company.Id;
    }
    public async Task<Employee> GenerateDefaultEmployee(Guid companyId)
    {
        // Generate Default Employee
        var userId = Guid.NewGuid();
        var userEmail = "ems@email.com"; 
        var employee = new Employee(userId, companyId, _faker.Person.FirstName, _faker.Person.LastName, userEmail, _faker.Phone.PhoneNumber("9########"), _faker.Person.Cpf(false), ERole.Admin, _faker.Random.Decimal(5000, 10000));

        await _employeeRepository.AddAsync(employee);
        return employee;
    }
    public async Task GenerateDefaultUser(Employee employee)
    {
        // Generate Default User
        var userId = employee.Id.ToString();
        var defaultPassword = "Senha@123";
        var user = new IdentityUser
        {
            Id = userId,
            UserName = employee.Email.Address,
            Email = employee.Email.Address,
            EmailConfirmed = true
        };
        var userClaim = new Claim("Tenant", employee.CompanyId.ToString());
        var roleAdmin = new IdentityRole("Admin");

        var userResult = await _userManager.CreateAsync(user, defaultPassword);
        if (userResult.Succeeded)
        {
            await _userManager.AddClaimAsync(user, userClaim);
        }

        var roleResult = await _roleManager.CreateAsync(roleAdmin);
        if (roleResult.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }
    }
    public async Task<List<Employee>> GenerateEmployees(Guid companyId)
    {
        // Generate Default Employees
        var employeeFaker = new Faker<Employee>("pt_BR")
            .CustomInstantiator(f => new Employee(
                Guid.NewGuid(),
                companyId,
                f.Person.FirstName,
                f.Person.LastName,
                f.Person.Email.ToLower(),
                f.Phone.PhoneNumber("9########"),
                f.Person.Cpf(false),
                ERole.Employee,
                f.Random.Decimal(1000, 5000)
            ));
        var employees = employeeFaker.Generate(4);

        await _employeeRepository.AddRangeAsync(employees);
        return employees;
    }
    public async Task<List<Client>> GenerateClients(Guid companyId)
    {
        // Generate Default Clients
        var clientFaker = new Faker<Client>("pt_BR")
            .CustomInstantiator(f => new Client(
                Guid.NewGuid(),
                companyId,
                f.Person.FirstName,
                f.Person.LastName,
                f.Person.Email.ToLower(),
                f.Phone.PhoneNumber("9########"),
                f.Person.Cpf(false),
                ERole.Client
            ));

        var clients = clientFaker.Generate(50);

        await _clientRepository.AddRangeAsync(clients);
        return clients;
    }
    public async Task<List<Service>> GenerateServices(Guid companyId)
    {
        // Generate Default Services
        var serviceNames = new List<string>
            {
                "Limpeza de Pele Profunda",
                "Hidratação Facial",
                "Peeling Químico",
                "Microagulhamento",
                "Aplicação de Botox"
            };
        var serviceFaker = new Faker<Service>("pt_BR")
                .CustomInstantiator(f => new Service(
                    Guid.NewGuid(),
                    companyId,
                    f.PickRandom(serviceNames),
                    f.Random.Decimal(500, 1500),
                    TimeSpan.FromMinutes(f.PickRandom(new[] { 30, 45, 60, 75, 90, 105, 120 })),
                    f.Random.Bool()
                ));

        var services = serviceFaker.Generate(5);

        await _serviceRepository.AddRangeAsync(services);
        return services;
    }
    public async Task<List<ServiceAppointment>> GenerateServiceAppointments(Guid companyId, List<Service> services, List<Employee> employees, List<Client> clients)
    {
        // Generate Default Service Appointments
        var appointmentFaker = new Faker<ServiceAppointment>("pt_BR")
            .CustomInstantiator(f =>
            {
                var service = f.PickRandom(services);
                var appointmentStart = f.Date.Between(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31));
                return new ServiceAppointment(
                    Guid.NewGuid(),
                    companyId,
                    f.PickRandom(employees).Id,
                    f.PickRandom(clients).Id,
                    service.Id,
                    appointmentStart,
                    appointmentStart.Add(service.Duration),
                    f.PickRandom<EServiceStatus>()
                );
            });

        var appointments = appointmentFaker.Generate(150);

        await _serviceAppointmentRepository.AddRangeAsync(appointments);
        return appointments;
    }
    public async Task GenerateFirstData()
    {
        var plans = await GenerateDefaultPlans();
        var companyId = await GenerateDefaultCompany(plans[2].Id);
        var employee = await GenerateDefaultEmployee(companyId);
        await GenerateDefaultUser(employee);
        var employees = await GenerateEmployees(companyId);
        var clients = await GenerateClients(companyId);
        var services = await GenerateServices(companyId);
        employees.Add(employee);
        await GenerateServiceAppointments(companyId, services, employees, clients);
    }
}
