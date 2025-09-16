using System;
using System.Collections.Generic;
using Ves.BLL.Services;
using Ves.BLL.Models;
using Ves.DAL.Config;
using Ves.DAL.Repositories;
using Ves.Services.Implementations;
using Ves.Domain.Entities;

// Connection strings for SQL Server 2019 instance
const string businessCs = "Server=DESKTOP-B7V8BS2\\SQLEXPRESS;Database=VesDB;User Id=ves_user;Password=ves_password;TrustServerCertificate=True;";
const string hashCs = "Server=DESKTOP-B7V8BS2\\SQLEXPRESS;Database=VesHashDB;User Id=hash_user;Password=hash_password;TrustServerCertificate=True;";

var factory = new SqlConnectionFactory(businessCs, hashCs);
var clientRepo = new ClientRepository(factory);
var fileRepo = new FileRecordRepository(factory);
var productRepo = new ProductRepository(factory);
var orderRepo = new OrderRepository(factory);
var detailRepo = new OrderDetailRepository(factory);
var saleRepo = new SaleRepository(factory);
var notifier = new ConsoleNotificationService();
var audit = new ConsoleAuditService();
var clientService = new ClientService(clientRepo, notifier, audit);
var fileService = new FileHashService(fileRepo);
var orderService = new OrderService(orderRepo, detailRepo, productRepo, saleRepo, audit);

Console.WriteLine("Sistema de gestión VES");
Console.WriteLine("1) Registrar cliente");
Console.WriteLine("2) Guardar hash de archivo");
Console.WriteLine("3) Crear pedido");
Console.WriteLine("4) Confirmar pedido");
Console.WriteLine("5) Cancelar pedido");
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
    case "3":
        Console.Write("ID del cliente: ");
        if (!int.TryParse(Console.ReadLine(), out var clientId))
        {
            Console.WriteLine("ID de cliente inválido.");
            break;
        }

        var items = new List<OrderItemRequest>();
        while (true)
        {
            Console.Write("ID de producto (ENTER para finalizar): ");
            var productInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(productInput))
            {
                break;
            }

            if (!int.TryParse(productInput, out var productId))
            {
                Console.WriteLine("ID de producto inválido.");
                continue;
            }

            Console.Write("Cantidad: ");
            var quantityInput = Console.ReadLine();
            if (!int.TryParse(quantityInput, out var quantity))
            {
                Console.WriteLine("Cantidad inválida.");
                continue;
            }

            items.Add(new OrderItemRequest(productId, quantity));
        }

        if (items.Count == 0)
        {
            Console.WriteLine("No se ingresaron productos para el pedido.");
            break;
        }

        try
        {
            var order = orderService.CreateOrder(clientId, items);
            Console.WriteLine($"Pedido creado con ID {order.Id} y total {order.Total}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear pedido: {ex.Message}");
        }

        break;
    case "4":
        Console.Write("ID del pedido: ");
        if (!int.TryParse(Console.ReadLine(), out var orderId))
        {
            Console.WriteLine("ID de pedido inválido.");
            break;
        }

        try
        {
            var sale = orderService.ConfirmOrder(orderId);
            Console.WriteLine($"Pedido confirmado. Venta {sale.Id} por un total de {sale.Total}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al confirmar pedido: {ex.Message}");
        }

        break;
    case "5":
        Console.Write("ID del pedido: ");
        if (!int.TryParse(Console.ReadLine(), out var cancelId))
        {
            Console.WriteLine("ID de pedido inválido.");
            break;
        }

        Console.Write("Motivo de cancelación: ");
        var reason = Console.ReadLine() ?? string.Empty;

        try
        {
            orderService.CancelOrder(cancelId, reason);
            Console.WriteLine("Pedido cancelado correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cancelar pedido: {ex.Message}");
        }

        break;
    default:
        Console.WriteLine("Opción inválida");
        break;
}
