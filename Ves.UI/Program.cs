using System;
using Ves.BLL.Services;
using Ves.DAL.Config;
using Ves.DAL.Interfaces;        // <- importante
using Ves.DAL.Repositories;
using Ves.Services.Implementations;
using Ves.Domain.Entities;

const string businessCs = @"Server=DESKTOP-B7V8BS2\SQLEXPRESS;Database=VesDB;User Id=ves_user;Password=ves_password;TrustServerCertificate=True;";
const string hashCs = @"Server=DESKTOP-B7V8BS2\SQLEXPRESS;Database=VesHashDB;User Id=hash_user;Password=hash_password;TrustServerCertificate=True;";

// fábricas separadas (cada una recibe 1 connection string)
IDbConnectionFactory businessFactory = new SqlConnectionFactory(businessCs);
IDbConnectionFactory hashFactory = new SqlConnectionFactory(hashCs);

// repositorios implementan interfaces
IClientRepository clientRepo = new ClientRepository(businessFactory);
IFileRecordRepository fileRepo = new FileRecordRepository(hashFactory);

// servicios transversales y de negocio
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
        Console.Write("Nombre: ");
        var name = Console.ReadLine() ?? string.Empty;
        Console.Write("Email: ");
        var email = Console.ReadLine() ?? string.Empty;

        var id = clientService.Register(new Client { Name = name, Email = email });
        Console.WriteLine($"Cliente registrado con ID {id}");
        break;

    case "2":
        Console.Write("Ruta del archivo: ");
        var path = Console.ReadLine() ?? string.Empty;

        fileService.SaveHash(path);
        fileService.ExportHashesToFile("hashes.txt");
        Console.WriteLine("Hash guardado y exportado a hashes.txt");
        break;

    default:
        Console.WriteLine("Opción inválida");
        break;
}
