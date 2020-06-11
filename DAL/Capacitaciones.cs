using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace wsRRHH.DAL
{
    public class Capacitaciones
    {
        Conexion cn = new Conexion();

        // SELECTS
        public DataSet getCapacitaciones(int top = 0)
        {
            SqlCommand query = new SqlCommand();

            if (top > 0)
            {
                query.CommandText = "SELECT " +
                    "TOP (@top)" +
                    "capacitaciones.id_capacitacion AS ID, " +
                    "capacitaciones.titulo AS Capacitacion, " +
                    "capacitaciones.cupo AS Cupo, " +
                    "departamentos.departamento AS Departamento, " +
                    "capacitaciones.fecha_creacion AS \"Fecha creada\", " +
                    "estados_capacitaciones.estado AS Estado, " +
                    "capacitaciones.descripcion AS Descripcion " +
                    "FROM capacitaciones " +
                    "JOIN departamentos ON capacitaciones.id_departamento = departamentos.id_departamento " +
                    "JOIN estados_capacitaciones ON capacitaciones.id_estado_capacitacion = estados_capacitaciones.id_estado_capacitacion";
                query.Parameters.AddWithValue("@top", top);
            }
            else
            {
                query.CommandText = "SELECT " +
                    "capacitaciones.id_capacitacion AS ID, " +
                    "capacitaciones.titulo AS Capacitacion, " +
                    "capacitaciones.cupo AS Cupo, " +
                    "departamentos.departamento AS Departamento, " +
                    "capacitaciones.fecha_creacion AS \"Fecha creada\", " +
                    "estados_capacitaciones.estado AS Estado, " +
                    "capacitaciones.descripcion AS Descripcion " +
                    "FROM capacitaciones " +
                    "JOIN departamentos ON capacitaciones.id_departamento = departamentos.id_departamento " +
                    "JOIN estados_capacitaciones ON capacitaciones.id_estado_capacitacion = estados_capacitaciones.id_estado_capacitacion";
            }
            return cn.selectQuery(query);
        }

        public DataSet getDetallesCapacitacion (int idCap)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "capacitaciones.titulo, " +
                "capacitaciones.cupo, " +
                "capacitaciones.descripcion, " +
                "capacitaciones.fecha_creacion, " +
                "departamentos.departamento, " +
                "estados_capacitaciones.estado " +
                "FROM capacitaciones " +
                "JOIN departamentos ON departamentos.id_departamento = capacitaciones.id_departamento " +
                "JOIN estados_capacitaciones ON estados_capacitaciones.id_estado_capacitacion = capacitaciones.id_estado_capacitacion " +
                "WHERE capacitaciones.id_capacitacion = @idCap";
            query.Parameters.AddWithValue("@idCap", idCap);
            return cn.selectQuery(query);
        }

        public DataSet getAsignCapacitaciones (int idCap)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "asignaciones_capacitaciones.id_asignacion_cap, " +
                "empleados.nombres, " +
                "empleados.apellidos, " +
                "empleados.correo, " +
                "departamentos.departamento AS \"Departamento empleado\" " +
                "FROM asignaciones_capacitaciones " +
                "JOIN empleados ON empleados.id_empleado = asignaciones_capacitaciones.id_empleado " +
                "JOIN departamentos ON departamentos.id_departamento = empleados.id_departamento " +
                "WHERE asignaciones_capacitaciones.id_capacitacion = @idCap";
            query.Parameters.AddWithValue("@idCap", idCap);
            return cn.selectQuery(query);
        }

        public DataSet getDetallesAsignCap (int idAsign)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "asignaciones_capacitaciones.id_asignacion_cap, " +
                "asignaciones_capacitaciones.id_capacitacion, " +
                "empleados.nombres, " +
                "empleados.apellidos, " +
                "empleados.correo, " +
                "empleados.DUI, " +
                "telefonos_empleados.telefono AS Telefono, " +
                "departamentos.departamento AS \"Departamento empleado\", " +
                "asignaciones_capacitaciones.fecha_asignacion " +
                "FROM asignaciones_capacitaciones " +
                "JOIN empleados ON empleados.id_empleado = asignaciones_capacitaciones.id_empleado " +
                "JOIN telefonos_empleados ON telefonos_empleados.id_empleado = empleados.id_empleado " +
                "JOIN departamentos ON departamentos.id_departamento = empleados.id_departamento " +
                "WHERE asignaciones_capacitaciones.id_asignacion_cap = @idAsign";
            query.Parameters.AddWithValue("@idAsign", idAsign);
            return cn.selectQuery(query);
        }

        public Boolean validateCapEmp (int idCap, int idEmp)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT COUNT(*) " +
                "FROM asignaciones_capacitaciones " +
                "WHERE id_capacitacion = @idCap AND id_empleado = @idEmp";
            query.Parameters.AddWithValue("@idCap", idCap);
            query.Parameters.AddWithValue("@idEmp", idEmp);

            DataSet result = cn.selectQuery(query);

            int resCount = int.Parse(result.Tables[0].Rows[0][0].ToString());

            if (resCount >= 1)
            {
                return false;
            } else
            {
                return true;
            }
        }

        public DataSet getEstadosCapacitaciones ()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT * FROM estados_capacitaciones";
            return cn.selectQuery(query);
        }

        // INSERTS
        public void insertCapacitacion (string titulo, string descripcion, int cupo, int idDpto)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "INSERT INTO capacitaciones " +
                "(titulo, cupo, id_departamento, descripcion) " +
                "VALUES (@titulo, @cupo, @id_departamento, @descripcion)";
            query.Parameters.AddWithValue("@titulo", titulo);
            query.Parameters.AddWithValue("@cupo", cupo);
            query.Parameters.AddWithValue("@id_departamento", idDpto);
            query.Parameters.AddWithValue("@descripcion", descripcion);
            cn.insertQuery(query);
        }

        public void asignarEmpCap (int idCap, int idEmpleado, string codigo)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "INSERT INTO asignaciones_capacitaciones " +
                "(id_capacitacion, id_empleado, codigo) " +
                "VALUES (@idCap, @idEmpleado, @codigo)";
            query.Parameters.AddWithValue("@idCap", idCap);
            query.Parameters.AddWithValue("@idEmpleado", idEmpleado);
            query.Parameters.AddWithValue("@codigo", codigo);
            cn.insertQuery(query);
        }

        // UPDATES
        public void updateCapacitacion (int idCap, string tiutlo, string decripcion, int cupo, int idDpto, int idEstado)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "UPDATE capacitaciones SET " +
                "titulo = @titulo, " +
                "cupo = @cupo, " +
                "id_departamento = @idDpto, " +
                "id_estado_capacitacion = @idEstado, " +
                "descripcion = @descripcion " +
                "WHERE id_capacitacion = @idCap";
            query.Parameters.AddWithValue("@titulo", tiutlo);
            query.Parameters.AddWithValue("@cupo", cupo);
            query.Parameters.AddWithValue("@idDpto", idDpto);
            query.Parameters.AddWithValue("@idEstado", idEstado);
            query.Parameters.AddWithValue("@descripcion", decripcion);
            query.Parameters.AddWithValue("@idCap", idCap);
            cn.updateQuery(query);
        }

        // DELETES
        public void deleteAsignCap (int idAsign)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "DELETE FROM asignaciones_capacitaciones WHERE id_asignacion_cap = @idAsign";
            query.Parameters.AddWithValue("@idAsign", idAsign);
            cn.deleteQuery(query);
        }

        public void deleteCapacitacion (int idCap)
        {
            // Borrar las asignaciones de la capacitacion
            SqlCommand query = new SqlCommand();
            query.CommandText = "DELETE FROM asignaciones_capacitaciones WHERE id_capacitacion = @idCap";
            query.Parameters.AddWithValue("@idCap", idCap);
            cn.deleteQuery(query);

            // Borrar la capacitacion
            SqlCommand query2 = new SqlCommand();
            query2.CommandText = "DELETE FROM capacitaciones WHERE id_capacitacion = @idCap";
            query2.Parameters.AddWithValue("@idCap", idCap);
            cn.deleteQuery(query2);
        }
    }
}