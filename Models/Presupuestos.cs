public class Presupuestos
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private List<PresupuestosDetalle> detalle;
    public Presupuestos(int idPresupuesto, string nombreDestinatario)
    {
        this.IdPresupuesto = idPresupuesto;
        this.NombreDestinatario = nombreDestinatario;
        this.Detalle = new List<PresupuestosDetalle>();
    }
    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public List<PresupuestosDetalle> Detalle { get => detalle; set => detalle = value; }
    public float montoPresupuesto(){
        return Detalle.Sum(d => d.Producto.Precio * d.Cantidad);
    } 
    public float montoPresupuestoConIva(){
        float montoSinIva = montoPresupuesto();
        double iva = 0.21;
        return (float)(montoSinIva*(1+iva));
    }
    public int cantidadProductos(){
    return Detalle.Sum(d => d.Cantidad);
    }
}