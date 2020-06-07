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

        // INSERTS
        public void insertEmpleado (string nombres, string apellidos, string email, string telefono1, string telefono2, string direccion, int idDpto, int idCargo, double salario)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "";
        }

        // UPDATES


        // DELETES


    }
}