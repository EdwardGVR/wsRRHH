using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace wsRRHH.DAL
{
    public class Evaluaciones
    {
        Conexion cn = new Conexion();

        // SELECTS
        public DataSet getEvaluaciones(int top = 0)
        {
            SqlCommand query = new SqlCommand();

            if (top > 0)
            {
                query.CommandText = "SELECT " +
                "TOP (@top)" +
                "evaluaciones.id_evaluacion AS ID, " +
                "evaluaciones.evaluacion AS Evaluacion, " +
                "tipos_evaluaciones.tipo_evaluacion AS \"Tipo evaluacion\", " +
                "evaluaciones.objetivos AS Objetivos, " +
                "evaluaciones.puntaje_maximo AS \"Nota Maxima\" " +
                "FROM evaluaciones " +
                "JOIN tipos_evaluaciones ON evaluaciones.id_tipo_evaluacion = tipos_evaluaciones.id_tipo_evaluacion";
                query.Parameters.AddWithValue("@top", top);
            }
            else
            {
                query.CommandText = "SELECT " +
                "evaluaciones.id_evaluacion AS ID, " +
                "evaluaciones.evaluacion AS Evaluacion, " +
                "tipos_evaluaciones.tipo_evaluacion AS \"Tipo evaluacion\", " +
                "evaluaciones.objetivos AS Objetivos, " +
                "evaluaciones.puntaje_maximo AS \"Nota Maxima\" " +
                "FROM evaluaciones " +
                "JOIN tipos_evaluaciones ON evaluaciones.id_tipo_evaluacion = tipos_evaluaciones.id_tipo_evaluacion";
            }

            return cn.selectQuery(query);
        }

        public DataSet getTiposEvaluaciones()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT * FROM tipos_evaluaciones";
            return cn.selectQuery(query);
        }

        public DataSet getDetallesEvaluacion(int idEval)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "evaluaciones.evaluacion AS Evaluacion, " +
                "evaluaciones.objetivos AS Objetivo, " +
                "evaluaciones.puntaje_maximo AS MaxScore, " +
                "evaluaciones.fecha_creacion AS FechaCreacion, " +
                "tipos_evaluaciones.tipo_evaluacion AS TipoEval " +
                "FROM evaluaciones " +
                "JOIN tipos_evaluaciones ON evaluaciones.id_tipo_evaluacion = tipos_evaluaciones.id_tipo_evaluacion " +
                "WHERE evaluaciones.id_evaluacion = @idEval";
            query.Parameters.AddWithValue("@idEval", idEval);
            return cn.selectQuery(query);
        }

        public DataSet getAsignEvalApl(int idEval)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "asignaciones_evaluaciones.id_asignacion_eval AS ID, " +
                "aplicantes.nombres AS Nombres, " +
                "aplicantes.apellidos AS Apellidos, " +
                "aplicantes.email AS Email " +
                "FROM asignaciones_evaluaciones " +
                "JOIN aplicantes ON asignaciones_evaluaciones.id_aplicante = aplicantes.id_aplicante " +
                "WHERE asignaciones_evaluaciones.id_evaluacion = @idEval";
            query.Parameters.AddWithValue("@idEval", idEval);
            return cn.selectQuery(query);
        }

        public DataSet getAplicantesByVac(int idVac)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "aplicantes.id_aplicante AS ID, " +
                "aplicantes.nombres + \' \' + aplicantes.apellidos AS Aplicante " +
                "FROM aplicantes " +
                "JOIN aplicaciones_vacantes ON aplicaciones_vacantes.id_aplicante = aplicantes.id_aplicante " +
                "WHERE aplicaciones_vacantes.id_vacante = @idVac";
            query.Parameters.AddWithValue("@idVac", idVac);
            return cn.selectQuery(query);
        }

        public Boolean validateEvApl(int idEval, int idApl)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT COUNT(*) " +
                "FROM asignaciones_evaluaciones " +
                "WHERE id_evaluacion = @idEval AND id_aplicante = @idApl";
            query.Parameters.AddWithValue("@idEval", idEval);
            query.Parameters.AddWithValue("@idApl", idApl);

            DataSet result = cn.selectQuery(query);

            int resCount = int.Parse(result.Tables[0].Rows[0][0].ToString());

            if (resCount >= 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public DataSet getDetallesAsignEval(int idAsign)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "asignaciones_evaluaciones.id_evaluacion, " +
                "asignaciones_evaluaciones.fecha_evaluacion, " +
                "asignaciones_evaluaciones.hora_evaluacion, " +
                "aplicantes.nombres, " +
                "aplicantes.apellidos, " +
                "aplicantes.email, " +
                "aplicantes.telefono " +
                "FROM asignaciones_evaluaciones " +
                "JOIN aplicantes ON aplicantes.id_aplicante = asignaciones_evaluaciones.id_aplicante " +
                "WHERE asignaciones_evaluaciones.id_asignacion_eval = @idAsign";
            query.Parameters.AddWithValue("@idAsign", idAsign);
            return cn.selectQuery(query);
        }

        // INSERTS
        public void insertEvaluacion (string evaluacion, int idTipo, string objetivos, int maxScore)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "INSERT INTO evaluaciones " +
                "(evaluacion, id_tipo_evaluacion, objetivos, puntaje_maximo) " +
                "VALUES (@evaluacion, @idTipo, @objetivos, @maxScore)";
            query.Parameters.AddWithValue("@evaluacion", evaluacion);
            query.Parameters.AddWithValue("@idTipo", idTipo);
            query.Parameters.AddWithValue("@objetivos", objetivos);
            query.Parameters.AddWithValue("@maxScore", maxScore);
            cn.insertQuery(query);
        }

        public void asignAplEval (int idEval, int idApl, string fechaEval, string horaEval)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "INSERT INTO asignaciones_evaluaciones " +
                "(id_evaluacion, id_aplicante, fecha_evaluacion, hora_evaluacion) " +
                "VALUES (@idEval, @idAplicante, @fechaEvaluacion, @horaEvaluacion)";
            query.Parameters.AddWithValue("@idEval", idEval);
            query.Parameters.AddWithValue("@idAPlicante", idApl);
            query.Parameters.AddWithValue("@fechaEvaluacion", fechaEval);
            query.Parameters.AddWithValue("@horaEvaluacion", horaEval);
            cn.insertQuery(query);
        }
    }
}