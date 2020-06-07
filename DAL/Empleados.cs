using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace wsRRHH.DAL
{
    public class Empleados
    {
        Conexion cn = new Conexion();


        // SELECTS
        public DataSet getEmpleados()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText =
                "SELECT " +
                "empleados.id_empleado AS ID, " +
                "empleados.nombres AS Nombres, " +
                "empleados.apellidos AS Apellidos, " +
                "empleados.correo AS Correo, " +
                "empleados.direccion AS Direccion, " +
                "cargos_empleados.cargo_empleado AS Cargo " +
                "FROM empleados " +
                "JOIN cargos_empleados " +
                "ON empleados.id_cargo_empleado = cargos_empleados.id_cargo_empleado";
            return cn.selectQuery(query);
        }

        public DataSet getDetallesEmpleado (string idType, int id = 0, string dui = "")
        {
            SqlCommand query = new SqlCommand();
            DataSet result = null;

            switch (idType)
            {
                case "id":
                    query.CommandText = "SELECT * FROM empleados WHERE id = @id";
                    query.Parameters.AddWithValue("@id", id);
                    result = cn.selectQuery(query);
                    break;
                case "dui":
                    query.CommandText = "SELECT * FROM empleados WHERE DUI = @dui";
                    query.Parameters.AddWithValue("@dui", dui);
                    result = cn.selectQuery(query);
                    break;
            }

            return result;
        }

        // INSERTS
        public void insertEmpleado (string nombres, string apellidos, string dui, string email, string telefono1, string telefono2, string direccion, int idDpto, int idCargo, double salario)
        {
            // Guardar los datos principales
            SqlCommand query1 = new SqlCommand();
            query1.CommandText = "INSERT INTO empleados " +
                "(nombres, apellidos, DUI, correo, direccion, id_cargo_empleado, id_departamento) " +
                "VALUES (@nombres, @apellidos, @dui, @email, @direccion, @idCargo, @idDpto)";
            query1.Parameters.AddWithValue("@nombres", nombres);
            query1.Parameters.AddWithValue("@apellidos", apellidos);
            query1.Parameters.AddWithValue("@dui", dui);
            query1.Parameters.AddWithValue("@email", email);
            query1.Parameters.AddWithValue("@direccion", direccion);
            query1.Parameters.AddWithValue("@idCargo", idCargo);
            query1.Parameters.AddWithValue("@idDpto", idDpto);
            cn.insertQuery(query1);

            // Obtener el id del empleado recien guardado
            SqlCommand query2 = new SqlCommand();
            query2.CommandText = "SELECT id_empleado FROM empleados WHERE dui = @dui";
            query2.Parameters.AddWithValue("@dui", dui);
            DataSet result = cn.selectQuery(query2);
            int idEmpleado = int.Parse(result.Tables[0].Rows[0][0].ToString());

            // GUardar los telefonos
            SqlCommand query3 = new SqlCommand();
            query3.CommandText = "INSERT INTO telefonos_empleados " +
                "(id_empleado, telefono) " +
                "VALUES (@id_empleado, @telefono1)";
            query3.Parameters.AddWithValue("@id_empleado", idEmpleado);
            query3.Parameters.AddWithValue("@telefono1", telefono1);
            cn.insertQuery(query3);

            SqlCommand query4 = new SqlCommand();
            query4.CommandText = "INSERT INTO telefonos_empleados " +
                "(id_empleado, telefono) " +
                "VALUES (@id_empleado, @telefono2)";
            query4.Parameters.AddWithValue("@id_empleado", idEmpleado);
            query4.Parameters.AddWithValue("@telefono2", telefono2);
            cn.insertQuery(query4);

            // Guardar el salario en contratos
            SqlCommand query5 = new SqlCommand();
            query5.CommandText = "INSERT INTO contratos " +
                "(id_empleado, salario) " +
                "VALUES (@id_empleado, @salario)";
            query5.Parameters.AddWithValue("@id_empleado", idEmpleado);
            query5.Parameters.AddWithValue("@salario", salario);
            cn.insertQuery(query5);
        }

        // UPDATES


        // DELETES


    }
}