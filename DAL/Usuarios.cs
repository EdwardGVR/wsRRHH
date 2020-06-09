using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace wsRRHH.DAL
{
	public class Usuarios
	{
		Conexion cn = new Conexion();

        // SELECTS
        public DataSet getUsuarios()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "usuarios.id_usuario AS ID, " +
                "usuarios.nombres AS Nombres, " +
                "usuarios.apellidos AS Apellidos, " +
                "usuarios.correo AS Correo, " +
                "usuarios.usuario AS Usuario, " +
                "estados_usuarios.estado_usuario AS Estado, " +
                "niveles_usuarios.nivel_usuario AS Nivel, " +
                "usuarios.fecha_creacion AS \"Fecha registro\" " +
                "FROM usuarios " +
                "JOIN estados_usuarios ON usuarios.id_estado_usuario = estados_usuarios.id_estado_usuario " +
                "JOIN niveles_usuarios ON usuarios.id_nivel = niveles_usuarios.id_nivel_usuario";
            return cn.selectQuery(query);
        }

        public bool validarLogin(string user, string clave)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT * FROM usuarios WHERE usuario = @user AND password = @clave";
            query.Parameters.AddWithValue("@user", user);
            query.Parameters.AddWithValue("@clave", clave);
            query.Connection = cn.conectar();
            cn.abrir();
            SqlDataReader queryResult = query.ExecuteReader();
            if (queryResult.Read())
            {
                Console.WriteLine("Encontrado");
                cn.cerrar();
                return true;
            }
            else
            {
                Console.WriteLine("No encontrado");
                cn.cerrar();
                return false;
            }

        }

        public DataSet getNiveles ()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT * FROM niveles_usuarios";
            return cn.selectQuery(query);
        }

        public DataSet getDetallesUsuario (int idUsuario)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "usuarios.nombres, " +
                "usuarios.apellidos, " +
                "usuarios.correo, " +
                "usuarios.usuario, " +
                "usuarios.fecha_creacion, " +
                "estados_usuarios.estado_usuario, " +
                "niveles_usuarios.nivel_usuario " +
                "FROM usuarios " +
                "JOIN estados_usuarios ON estados_usuarios.id_estado_usuario = usuarios.id_estado_usuario " +
                "JOIN niveles_usuarios ON niveles_usuarios.id_nivel_usuario = usuarios.id_nivel " +
                "WHERE usuarios.id_usuario = @idUsuario";
            query.Parameters.AddWithValue("@idUsuario", idUsuario);
            return cn.selectQuery(query);
        }

        // INSERTS
        public void insertUsuario (string nombres, string apellidos, string email, string usuario, string password, int idNivel)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "INSERT INTO usuarios " +
                "(nombres, apellidos, correo, usuario, password, id_nivel) " +
                "VALUES (@nombres, @apellidos, @correo, @usuario, @password, @idNivel)";
            query.Parameters.AddWithValue("@nombres", nombres);
            query.Parameters.AddWithValue("@apellidos", apellidos);
            query.Parameters.AddWithValue("@correo", email);
            query.Parameters.AddWithValue("@usuario", usuario);
            query.Parameters.AddWithValue("@password", password);
            query.Parameters.AddWithValue("@idNivel", idNivel);
            cn.insertQuery(query);
        }
    }
}