public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Documento { get; set; }
    public string? Telefono { get; set; }
    public DateTime FechaAlta { get; set; } = DateTime.Now;
    public bool Activo { get; set; } = true;
}
