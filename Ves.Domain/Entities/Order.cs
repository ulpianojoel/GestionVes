using System.Collections.Generic;
namespace Ves.Domain.Entities
{
  public class Order
  {
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public System.DateTime Fecha { get; set; } = System.DateTime.Now;
    public string? Observaciones { get; set; }
    public List<OrderItem> Items { get; set; } = new();
  }
}
