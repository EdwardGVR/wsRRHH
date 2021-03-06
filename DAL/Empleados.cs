﻿using System;
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

        public DataSet getDetallesEmpleado (int id)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "empleados.nombres AS Nombres, " +
                "empleados.apellidos AS Apellidos, " +
                "empleados.DUI AS DUI, " +
                "empleados.correo AS Email, " +
                "empleados.direccion AS Direccion, " +
                "telefonos_empleados.telefono AS Telefono, " +
                "contratos.salario AS Salario, " +
                "contratos.fecha_contratacion AS FechaContrato, " +
                "estados_contratos.estado AS EstadoContrato, " +
                "cargos_empleados.cargo_empleado AS Cargo, " +
                "departamentos.departamento AS Departamento " +
                "FROM empleados " +
                "JOIN telefonos_empleados ON telefonos_empleados.id_empleado = empleados.id_empleado " +
                "JOIN contratos ON contratos.id_empleado = empleados.id_empleado " +
                "JOIN estados_contratos ON estados_contratos.id_estado_contrato = contratos.id_estado_contrato " +
                "JOIN cargos_empleados ON cargos_empleados.id_cargo_empleado = empleados.id_cargo_empleado " +
                "JOIN departamentos ON departamentos.id_departamento = empleados.id_departamento " +
                "WHERE empleados.id_empleado = @id";
            query.Parameters.AddWithValue("@id", id);
            return cn.selectQuery(query);
        }

        public DataSet getEmpleadosByDpto (int idDpto)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = " SELECT " +
                "empleados.id_empleado AS ID, " +
                "empleados.nombres + \' \' + empleados.apellidos AS Empleado " +
                "FROM empleados " +
                "WHERE empleados.id_departamento = @id";
            query.Parameters.AddWithValue("@id", idDpto);
            return cn.selectQuery(query);
        } 

        public DataSet getEstadosContratos ()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT * FROM estados_contratos";
            return cn.selectQuery(query);
        }

        public int getCountEmpsByDpto (int idDpto)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT COUNT(*) FROM empleados WHERE id_departamento = @idDpto";
            query.Parameters.AddWithValue("@idDpto", idDpto);
            DataSet result = cn.selectQuery(query);
            int empsCount = int.Parse(result.Tables[0].Rows[0][0].ToString());
            return empsCount;
        }

        public int getCountEmps()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT COUNT(*) FROM empleados";
            DataSet result = cn.selectQuery(query);
            int empsCount = int.Parse(result.Tables[0].Rows[0][0].ToString());
            return empsCount;
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
        public void updateEmpleado (string nombres, string apellidos, string dui, string email, string telefono1, string telefono2, 
            string direccion, int idDpto, int idCargo, double salario, int idEstadoContrato, int idEmp)
        {
            // Guardar los datos principales
            SqlCommand query1 = new SqlCommand();
            query1.CommandText = "UPDATE empleados SET " +
                "nombres = @nombres, " +
                "apellidos = @apellidos, " +
                "DUI = @dui, " +
                "correo = @correo, " +
                "direccion = @direccion, " +
                "id_cargo_empleado = @idCargo, " +
                "id_departamento = @idDpto " +
                "WHERE id_empleado = @idEmp";
            query1.Parameters.AddWithValue("@nombres", nombres);
            query1.Parameters.AddWithValue("@apellidos", apellidos);
            query1.Parameters.AddWithValue("@dui", dui);
            query1.Parameters.AddWithValue("@correo", email);
            query1.Parameters.AddWithValue("@direccion", direccion);
            query1.Parameters.AddWithValue("@idCargo", idCargo);
            query1.Parameters.AddWithValue("@idDpto", idDpto);
            query1.Parameters.AddWithValue("@idEmp", idEmp);
            cn.insertQuery(query1);

            // Borrar los telefonos
            SqlCommand delTels = new SqlCommand();
            delTels.CommandText = "DELETE FROM telefonos_empleados WHERE id_empleado = @idEmp";
            delTels.Parameters.AddWithValue("@idEmp", idEmp);
            cn.deleteQuery(delTels);

            // GUardar los telefonos
            SqlCommand query3 = new SqlCommand();
            query3.CommandText = "INSERT INTO telefonos_empleados " +
                "(id_empleado, telefono) " +
                "VALUES (@id_empleado, @telefono1)";
            query3.Parameters.AddWithValue("@id_empleado", idEmp);
            query3.Parameters.AddWithValue("@telefono1", telefono1);
            cn.insertQuery(query3);

            SqlCommand query4 = new SqlCommand();
            query4.CommandText = "INSERT INTO telefonos_empleados " +
                "(id_empleado, telefono) " +
                "VALUES (@id_empleado, @telefono2)";
            query4.Parameters.AddWithValue("@id_empleado", idEmp);
            query4.Parameters.AddWithValue("@telefono2", telefono2);
            cn.insertQuery(query4);

            // Guardar el salario en contratos
            SqlCommand query5 = new SqlCommand();
            query5.CommandText = "UPDATE contratos SET " +
                "salario = @salario, " +
                "id_estado_contrato = @idEstadoContrato " +
                "WHERE id_empleado = @idEmp";
            query5.Parameters.AddWithValue("@salario", salario);
            query5.Parameters.AddWithValue("@idEstadoContrato", idEstadoContrato);
            query5.Parameters.AddWithValue("@idEmp", idEmp);
            cn.insertQuery(query5);
        }

        // DELETES
        public void deleteEmpleado (int idEmp)
        {
            // Borrar telefonos
            SqlCommand query1 = new SqlCommand();
            query1.CommandText = "DELETE FROM telefonos_empleados WHERE id_empleado = @idEmp";
            query1.Parameters.AddWithValue("@idEmp", idEmp);
            cn.deleteQuery(query1);

            // Borrar contrato
            SqlCommand query2 = new SqlCommand();
            query2.CommandText = "DELETE FROM contratos WHERE id_empleado = @idEmp";
            query2.Parameters.AddWithValue("@idEmp", idEmp);
            cn.deleteQuery(query2);

            // Borrar asignaciones a capacitaciones
            SqlCommand query3 = new SqlCommand();
            query3.CommandText = "DELETE FROM asignaciones_capacitaciones WHERE id_empleado = @idEmp";
            query3.Parameters.AddWithValue("@idEmp", idEmp);
            cn.deleteQuery(query3);

            // Borrar empleado
            SqlCommand query4 = new SqlCommand();
            query4.CommandText = "DELETE FROM empleados WHERE id_empleado = @idEmp";
            query4.Parameters.AddWithValue("@idEmp", idEmp);
            cn.deleteQuery(query4);
        }
    }
}