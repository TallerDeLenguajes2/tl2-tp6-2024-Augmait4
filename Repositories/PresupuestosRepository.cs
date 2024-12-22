using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
public interface IPresupuestosRepository
{
    List<Presupuestos> GetAll();
    Presupuestos GetById(int id);
    void Created(Presupuestos presupuesto);
    void Delete(int id);

}
public class PresupuestosRepository : IPresupuestosRepository
{
    private readonly string _stringConnection;
    public PresupuestosRepository(string stringConnection)
    {
        _stringConnection = stringConnection;
    }
    public List<Presupuestos> GetAll()
    {
        List<Presupuestos> presupuestos = new List<Presupuestos>();
        string query = @"SELECT idPresupuesto, nombreDestinatario FROM Presupuestos;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idPresupuesto = reader.GetInt32(0);
                    string nombreDestinatario = reader.GetString(1);

                    Presupuestos presupuesto = new Presupuestos(idPresupuesto, nombreDestinatario);
                    presupuesto.Detalle = GetPresupuestosDetalles(idPresupuesto);
                    presupuestos.Add(presupuesto);
                }
            }
            connection.Close();
        }
        return presupuestos;
    }
    public Presupuestos GetById(int id)
    {
        Presupuestos presupuesto = null;
        string query = @"SELECT idPresupuesto, NombreDestinatario FROM  Presupuestos WHERE idPresupuesto = @id";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    int idPresupuesto = reader.GetInt32(0);
                    string nombreDestinatario = reader.GetString(1);
                    presupuesto = new Presupuestos(idPresupuesto, nombreDestinatario);
                    presupuesto.Detalle = GetPresupuestosDetalles(idPresupuesto);
                }
            }
            connection.Close();
        }
        return presupuesto;
    }
    public void Created(Presupuestos presupuestos)
    {
        string query = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion)
                    VALUES (@NombreDestinatario, @FechaCreacion);";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@NombreDestinatario", presupuestos.NombreDestinatario));
            command.Parameters.Add(new SqliteParameter("@FechaCreacion", DateTime.Today));

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void Delete(int id)
    {
        string query = @"DELETE FROM Presupuestos WHERE idPresupuesto = @id;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand commandDetalles = new SqliteCommand(@"DELETE FROM PresupuestoDetalles
                                                            WHERE idPresupuesto = @idPresupuesto;", connection);
            commandDetalles.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
            commandDetalles.ExecuteNonQuery();

            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    private List<PresupuestosDetalle> GetPresupuestosDetalles(int id)
    {
        List<PresupuestosDetalle> presupuestosDetalles = new List<PresupuestosDetalle>();
        string query = @"SELECT idPresupuesto, idProducto, cantidad, p.Descripcion, p.Precio 
                        FROM PresupuestosDetalle
                        INNER JOIN Productos p USING (idProducto);";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idProducto;
                    string descripcion;
                    int Precio;
                    Productos producto = new Productos(
                        idProducto = reader.GetInt32(0),
                        descripcion = reader.GetString(1),
                        Precio = reader.GetInt32(2));

                    int cantidad = reader.GetInt32(3);
                    presupuestosDetalles.Add(new PresupuestosDetalle(cantidad, producto));
                }
            }
            connection.Close();
        }
        return presupuestosDetalles;
    }
    public void AddProductToPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        string query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, cantidad)
                         VALUES (@idPresupuesto, @idProducto, @cantidad)";

        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
            command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
            command.Parameters.Add(new SqliteParameter("@cantidad", cantidad));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void Update(int id, Productos producto, int cantidad)
    {
        string query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad)
                         VALUES (@idPresupuesto, @idProducto, @Cantidad);";

        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();

            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@idPresupuesto", id));
            command.Parameters.Add(new SqliteParameter("@idProducto", producto.IdProducto));
            command.Parameters.Add(new SqliteParameter("@Cantidad", cantidad));
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}