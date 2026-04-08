// Models/DTOs/DarDeBajaDto.cs
public class DarDeBajaDto
{
    public Guid RazonBajaId { get; set; }
    public decimal? ImporteVenta { get; set; }  // solo si cat_razon_baja.genera_ingreso = true
    public DateOnly FechaBaja { get; set; }
    public string? Notas { get; set; }
}

/* Solo esos 4 campos — los que tiene sentido que el cliente envíe. Los demás se asignan en el controlador:

Id → lo genera la base de datos
EjemplarId → viene de la URL ({id})
CreatedAt → DateTime.UtcNow
CreatedBy → tu Guid temporal por ahora */