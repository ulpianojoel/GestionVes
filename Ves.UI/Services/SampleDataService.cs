using System;
using System.Collections.Generic;
using System.Linq;
using Ves.UI.Models;

namespace Ves.UI.Services;

public sealed class SampleDataService
{
    public IReadOnlyCollection<OperationItem> GetOperationPipeline() => new List<OperationItem>
    {
        new()
        {
            Title = "Validación de clientes",
            Description = "Revisión de documentación para nuevos clientes VES.",
            Status = "Pendiente",
            DueDate = DateTime.Today.AddDays(1)
        },
        new()
        {
            Title = "Cierre mensual",
            Description = "Consolidación de ventas y cobranzas.",
            Status = "En progreso",
            DueDate = DateTime.Today.AddDays(4)
        },
        new()
        {
            Title = "Auditoría interna",
            Description = "Seguimiento de observaciones de auditoría.",
            Status = "Bloqueado",
            DueDate = DateTime.Today.AddDays(7)
        }
    };

    public IReadOnlyCollection<NotificationMessage> GetNotifications() => new List<NotificationMessage>
    {
        new()
        {
            Title = "Actualización exitosa",
            Detail = "La sincronización con BusinessDb finalizó sin errores.",
            CreatedUtc = DateTime.UtcNow.AddMinutes(-15)
        },
        new()
        {
            Title = "Recordatorio de auditoría",
            Detail = "Faltan adjuntar comprobantes de gastos en 2 operaciones.",
            CreatedUtc = DateTime.UtcNow.AddHours(-2)
        },
        new()
        {
            Title = "Nueva solicitud",
            Detail = "Se asignó un nuevo cliente para validación inicial.",
            CreatedUtc = DateTime.UtcNow.AddHours(-6)
        }
    };

    public DashboardSummary BuildSummary(IEnumerable<UserAccount> users, IEnumerable<OperationItem> operations)
    {
        var userList = users.ToList();
        var operationList = operations.ToList();

        return new DashboardSummary
        {
            ActiveUsers = userList.Count(u => u.IsActive),
            InactiveUsers = userList.Count(u => !u.IsActive),
            PendingOperations = operationList.Count(o => string.Equals(o.Status, "Pendiente", StringComparison.OrdinalIgnoreCase)),
            Alerts = operationList.Count(o => string.Equals(o.Status, "Bloqueado", StringComparison.OrdinalIgnoreCase)),
            LastSyncLabel = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
        };
    }

    public IReadOnlyCollection<ReportSnapshot> GetReportSnapshots() => new List<ReportSnapshot>
    {
        new()
        {
            Title = "Altas aprobadas",
            Description = "Cantidad de nuevos clientes habilitados en el período.",
            Period = "Últimos 30 días",
            Trend = "+12% vs. mes anterior"
        },
        new()
        {
            Title = "Casos observados",
            Description = "Solicitudes que requieren documentación adicional.",
            Period = "Últimos 7 días",
            Trend = "-5% vs. semana anterior"
        },
        new()
        {
            Title = "Tiempo promedio de resolución",
            Description = "Horas promedio desde la carga hasta la aprobación.",
            Period = "Acumulado 2024",
            Trend = "48h promedio"
        }
    };
}
