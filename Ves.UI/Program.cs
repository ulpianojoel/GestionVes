using Ves.BLL.Services;
using Ves.DAL.Config;
using Ves.DAL.Repositories;
using Ves.Services.Implementations;
using Ves.Domain.Entities;

// Connection strings for SQL Server 2019 instance
const string businessCs = "Server=DESKTOP-B7V8BS2\\SQLEXPRESS;Database=VesDB;User Id=test;Password=test;TrustServerCertificate=True;";
const string hashCs = "Server=DESKTOP-B7V8BS2\\SQLEXPRESS;Database=VesHashDB;User Id=test;Password=test;TrustServerCertificate=True;";

var factory = new SqlConnectionFactory(businessCs, hashCs);
var clientRepo = new ClientRepository(factory);
var fileRepo = new FileRecordRepository(factory);
var notifier = new ConsoleNotificationService();
var audit = new ConsoleAuditService();
var clientService = new ClientService(clientRepo, notifier, audit);
var fileService = new FileHashService(fileRepo);

Console.WriteLine("Sistema de gestión VES");
Console.WriteLine("1) Registrar cliente");
Console.WriteLine("2) Guardar hash de archivo");
Console.Write("Seleccione una opción: ");
var option = Console.ReadLine();

switch (option)
{
    case "1":
        Console.Write("Nombre: "); var name = Console.ReadLine() ?? string.Empty;
        Console.Write("Email: "); var email = Console.ReadLine() ?? string.Empty;
        var id = clientService.Register(new Client { Name = name, Email = email });
        Console.WriteLine($"Cliente registrado con ID {id}");
        break;
    case "2":
        Console.Write("Ruta del archivo: "); var path = Console.ReadLine() ?? string.Empty;
        fileService.SaveHash(path);
        fileService.ExportHashesToFile("hashes.txt");
        Console.WriteLine("Hash guardado y exportado a hashes.txt");
        break;
    default:
        Console.WriteLine("Opción inválida");
        break;
}
