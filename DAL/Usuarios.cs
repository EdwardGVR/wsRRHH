using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace wsRRHH.DAL
{
	public class Usuarios
	{
		Conexion cn = new Conexion();

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
    }
}