using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace wsRRHH.DAL
{
    public class Vacantes
    {
        Conexion cn = new Conexion();

        public DataSet getVacantes (int top = 0)
        {
            SqlCommand query = new SqlCommand();

            if (top > 0)
            {
                query.CommandText = "SELECT " +
                    "TOP (@top)" +
                    "vacantes.id_vacante AS ID, " +
                    "vacantes.codigo_vacante AS Codigo, " +
                    "vacantes.vacante AS Vacante, " +
                    "departamentos.departamento AS Departamento, " +
                    "vacantes.cupo_vacante AS Cupo, " +
                    "vacantes.descripcion AS Descripcion, " +
                    "vacantes.fehca_creacion AS \"Fecha creacion\", " +
                    "estados_vacantes.estado AS Estado " +
                    "FROM vacantes " +
                    "JOIN departamentos ON vacantes.id_departamento = departamentos.id_departamento " +
                    "JOIN estados_vacantes ON vacantes.id_estado_vacante = estados_vacantes.id_estado_vacante " +
                    "ORDER BY vacantes.id_vacante DESC";
                query.Parameters.AddWithValue("@top", top);
            }
            else
            {
                query.CommandText = "SELECT " +
                    "vacantes.id_vacante AS ID, " +
                    "vacantes.codigo_vacante AS Codigo, " +
                    "vacantes.vacante AS Vacante, " +
                    "departamentos.departamento AS Departamento, " +
                    "vacantes.cupo_vacante AS Cupo, " +
                    "vacantes.descripcion AS Descripcion, " +
                    "vacantes.fehca_creacion AS \"Fecha creacion\", " +
                    "estados_vacantes.estado AS Estado " +
                    "FROM vacantes " +
                    "JOIN departamentos ON vacantes.id_departamento = departamentos.id_departamento " +
                    "JOIN estados_vacantes ON vacantes.id_estado_vacante = estados_vacantes.id_estado_vacante " +
                    "ORDER BY vacantes.id_vacante DESC";
            }

            return cn.selectQuery(query);
        }

        public void insertVacante(string codVac, string vacante, int idDpto, int cupo, string descripcion)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "INSERT INTO vacantes(codigo_vacante, vacante, id_departamento, cupo_vacante, descripcion) " +
                "VALUES(@codVac, @vacante, @idDpto, @cupo, @descripcion)";
            query.Parameters.AddWithValue("@codVac", codVac);
            query.Parameters.AddWithValue("@vacante", vacante);
            query.Parameters.AddWithValue("@idDpto", idDpto);
            query.Parameters.AddWithValue("@cupo", cupo);
            query.Parameters.AddWithValue("@descripcion", descripcion);
            cn.insertQuery(query);
        }

        public DataSet getPrioridades ()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT prioridad_requisito FROM prioridades_requisitos";
            return cn.selectQuery(query);
        }

        // Obtiene el siguiente correlativo de la vacante segun departamento
        public int getCorrVac(int idDpto)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT COUNT(*) FROM vacantes WHERE id_departamento = @idDepartamento";
            query.Parameters.AddWithValue("@idDepartamento", idDpto);
            DataSet result = cn.selectQuery(query);

            int corrVac = int.Parse(result.Tables[0].Rows[0][0].ToString());
            return corrVac + 1;
        }
    }
}