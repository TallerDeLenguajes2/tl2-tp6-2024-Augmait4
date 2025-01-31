using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
public interface IClienteRepository
{
    List<Cliente> GetAll();
    Cliente GetById(int id);
    void Created(Cliente cliente);
    void Delete(int id);

}
public class ClienteRepository : IClienteRepository
{
    private readonly string _stringConnection;

    public ClienteRepository(string stringConnection)
    {
        _stringConnection = stringConnection;
    }
    public List<Cliente> GetAll()
    {
        List<Cliente> clientes = new List<Cliente>();
        string query = @"SELECT ClienteId, Nombre, Email, Telefono FROM Cliente;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idCliente = reader.GetInt32(0);
                    string nombreCliente = reader.GetString(1);
                    string email = reader.GetString(2);
                    string tel = reader.GetString(3);
                    Cliente nuevoCliente = new Cliente(idCliente, nombreCliente, email, tel);
                    clientes.Add(nuevoCliente);
                }
            }
            connection.Close();
        }
        return clientes;
    }
    public Cliente GetById(int id)
    {
        Cliente cliente = null;
        string query = @"SELECT ClienteId, Nombre, Email, Telefono FROM  Cliente WHERE ClienteId = @id";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    cliente.ClienteId = reader.GetInt32(0);
                    cliente.Nombre = reader.GetString(1);
                    cliente.Email = reader.GetString(2);
                    cliente.Telefono = reader.GetString(3);
                }
            }
            connection.Close();
        }
        return cliente;
    }
    public void Created(Cliente clientes)
    {
        string query = @"INSERT INTO Cliente (Nombre, Email, Telefono)
                    VALUES ( @Nombre, @Email, @Telefono);";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@Nombre", clientes.Nombre));
            command.Parameters.Add(new SqliteParameter("@Email", clientes.Email ));
            command.Parameters.Add(new SqliteParameter("@Telefono", clientes.Telefono));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
    public void Delete(int id)
    {
        string query = @"DELETE FROM Cliente WHERE ClienteId = @id;";
        using (SqliteConnection connection = new SqliteConnection(_stringConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

}