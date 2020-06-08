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
    }
}