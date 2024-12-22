using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SQLitePCL;
public interface IProductoRepository
{
    List<Productos> GetAll();
    Productos GetById(int id);
    void Created(Productos producto);
    void Update(int idProducto, Productos producto);
    void Delete(int idProducto);
    bool ExistsByTitle(string descripcion);
}
public class ProductoRepository : IProductoRepository
{
    private readonly string _stringConnection;
    public ProductoRepository(string stringConnection)
    {
        _stringConnection = stringConnection;
    }
    public List<Productos> GetAll()
    {
        List<Productos> productos = new List<Productos>();
        string query = @"SELECT idProducto, Descripcion, Precio FROM Productos;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    productos.Add(new Productos
                    {
                        IdProducto = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),
                        Precio = reader.GetInt32(2)
                    }
                    );
                }
            }
            connection.Close();
        }
        return productos;
    }
public Productos GetById(int id)
{
    Productos productos = null;
    string query = @"SELECT idProducto, Descripcion, Precio FROM Productos WHERE idProducto = @id;";
    using (SqliteConnection connection = new SqliteConnection(_stringConnection))
    {
        connection.Open();
        SqliteCommand command = new SqliteCommand(query, connection);
        command.Parameters.Add(new SqliteParameter("@id", id));
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (reader.Read()) // Usar 'if' para un único registro
            {
                productos = new Productos(); // Inicializa el objeto
                productos.IdProducto = reader.GetInt32(0);
                productos.Descripcion = reader.GetString(1);
                productos.Precio = reader.GetInt32(2);
            }
        }
    }
    return productos;
}

    public void Created(Productos producto)
    {
        string query = @"INSERT 
                        INTO Productos (Descripcion, Precio) 
                        VALUES ( @Descripcion, @Precio);";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void Update(int idProducto, Productos producto)
    {
        string query = @"UPDATE Productos
                        SET Descripcion = @Descripcion, Precio = @Precio
                        WHERE idProducto = @idProducto;";
        if (ExistsByTitle(producto.Descripcion))
        {
            return;
        }
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@Descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter("@Precio", producto.Precio));
            command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void Delete(int idProducto)
    {
        string query = "DELETE FROM Producto WHERE idProducto = @idProducto;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@idProducto", idProducto));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public bool ExistsByTitle(string descripcion)
    {
        string query = @"SELECT COUNT(1)
                         FROM Productos
                         WHERE Descripcion = @Descripcion;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@Descripcion", descripcion));
            int count = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            return count > 0;
        }
    }
}