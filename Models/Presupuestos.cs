public class Presupuestos
{
    private int idPresupuesto;
    private Cliente cliente;
    private List<PresupuestosDetalle> detalle = new List<PresupuestosDetalle>();
    private const double IVA = 0.21;
    public Presupuestos()
    {

    }
    public Presupuestos(int idPresupuesto, string nombreDestinatario)
    {
        this.IdPresupuesto = idPresupuesto;
    }
    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public List<PresupuestosDetalle> Detalle { get => detalle; set => detalle = value; }
    internal Cliente Cliente { get => cliente; set => cliente = value; }

    public decimal MontoPresupuesto()
    {
        decimal monto = 0;
        foreach (var Presupuesto in Detalle)
        {
            monto += Presupuesto.Producto.Precio * Presupuesto.Cantidad;
        }
        return monto;
    }
    public decimal MontoPresupuestoConIVA()
    {
        decimal montoConIva = 0;
        foreach (var PresupuestoDetalle in Detalle)
        {
            decimal precio = PresupuestoDetalle.Producto.Precio * PresupuestoDetalle.Cantidad;
            montoConIva += precio * (1 + (decimal)IVA);
        }
        return montoConIva;
    }
    public int CantidadProductos()
    {
        int cantidad = 0;
        foreach (var Presupuesto in Detalle)
        {
            cantidad += Presupuesto.Cantidad;
        }
        return cantidad;
    }
}