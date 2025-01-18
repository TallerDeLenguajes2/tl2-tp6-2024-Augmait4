public class PresupuestosDetalle
{
    private int cantidad;
    private Productos producto;

    public PresupuestosDetalle(int cantidad, Productos producto)
    {
        this.Cantidad = cantidad;
        this.Producto = producto;
    }
    public int Cantidad { get => cantidad; set => cantidad = value; }
    public Productos Producto { get => producto; set => producto = value; }
}