using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.CodeDom;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace wsRRHH.DAL
{
    public class Vacantes
    {
        Conexion cn = new Conexion();

        // SELECTS
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

        public DataSet getPrioridades()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT prioridad_requisito FROM prioridades_requisitos";
            return cn.selectQuery(query);
        }

        public DataSet getEstadosVacantes ()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT id_estado_vacante, estado FROM estados_vacantes";
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

        public int getIdVac (string codVac)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT id_vacante FROM vacantes WHERE codigo_vacante = @codVac";
            query.Parameters.AddWithValue("@codVac", codVac);
            DataSet result = cn.selectQuery(query);

            int idVac = int.Parse(result.Tables[0].Rows[0][0].ToString());
            return idVac;
        }

        public DataSet getDetallesVacante (string codVac)
        {
            SqlCommand query = new SqlCommand();
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
                    "WHERE vacantes.codigo_vacante = @codVac";
            query.Parameters.AddWithValue("@codVac", codVac);
            return cn.selectQuery(query);
        }

        public DataSet getRequisitosVac (string codVac)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "requisitos_vacantes.id_requisitos_vacante AS ID, " +
                "requisitos_vacantes.codigo_vacante AS Vacante, " +
                "requisitos_vacantes.requisito AS Requisito, " +
                "requisitos_vacantes.detalles AS Detalles, " +
                "prioridades_requisitos.prioridad_requisito AS Prioridad " +
                "FROM requisitos_vacantes " +
                "JOIN prioridades_requisitos ON requisitos_vacantes.id_prioridad_requisito = prioridades_requisitos.id_prioridad_requisito " +
                "WHERE requisitos_vacantes.codigo_vacante = @codVac";
            query.Parameters.AddWithValue("@codVac", codVac);
            return cn.selectQuery(query);
        }

        public DataSet getDetallesRequisito (int idReq)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "vacantes.vacante AS Vacante, " +
                "requisitos_vacantes.codigo_vacante AS \"Codigo Vacante\", " +
                "requisitos_vacantes.id_requisitos_vacante, " +
                "requisitos_vacantes.requisito AS Requisito, " +
                "requisitos_vacantes.detalles AS Detalles, " +
                "prioridades_requisitos.prioridad_requisito AS Prioridad " +
                "FROM requisitos_vacantes " +
                "JOIN vacantes ON requisitos_vacantes.codigo_vacante = vacantes.codigo_vacante " +
                "JOIN prioridades_requisitos ON prioridades_requisitos.id_prioridad_requisito = requisitos_vacantes.id_prioridad_requisito " +
                "WHERE requisitos_vacantes.id_requisitos_vacante = @idReq";
            query.Parameters.AddWithValue("@idReq", idReq);
            return cn.selectQuery(query);
        }

        public DataSet getAplicantesVac (int idVac)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "aplicantes.id_aplicante AS ID, " +
                "aplicantes.nombres AS Nombres, " +
                "aplicantes.apellidos AS Apellidos, " +
                "tipos_aplicantes.tipo_aplicante AS Tipo " +
                "FROM aplicaciones_vacantes " +
                "JOIN aplicantes ON aplicantes.id_aplicante = aplicaciones_vacantes.id_aplicante " +
                "JOIN tipos_aplicantes ON tipos_aplicantes.id_tipo_aplicante = aplicantes.id_tipo_aplicante " +
                "WHERE aplicaciones_vacantes.id_vacante = @idVac";
            query.Parameters.AddWithValue("idVac", idVac);
            return cn.selectQuery(query);
        }

        public DataSet getTiposAplicantes ()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT * FROM tipos_aplicantes";
            return cn.selectQuery(query);
        }

        public DataSet getResultadosAplicaciones ()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT * FROM resultados_aplicaciones";
            return cn.selectQuery(query);
        }

        // Devuelve verdadero si el dui no esta registrado
        public Boolean uniqueAplDui (string dui)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT COUNT(*) FROM aplicantes WHERE dui = @dui";
            query.Parameters.AddWithValue("@dui", dui);

            DataSet resutl = cn.selectQuery(query);
            int resCount = int.Parse(resutl.Tables[0].Rows[0][0].ToString());

            if (resCount > 0)
            {
                return false;
            } else
            {
                return true;
            }
        }

        public DataSet getDetallesAplicante (int idApl)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT " +
                "aplicantes.nombres, " +
                "aplicantes.apellidos, " +
                "aplicantes.email, " +
                "aplicantes.telefono, " +
                "aplicantes.direccion, " +
                "aplicantes.dui, " +
                "tipos_aplicantes.tipo_aplicante, " +
                "aplicaciones_vacantes.fecha_aplicacion, " +
                "resultados_aplicaciones.resultado_aplicacion " +
                "FROM aplicantes " +
                "JOIN tipos_aplicantes ON tipos_aplicantes.id_tipo_aplicante = aplicantes.id_tipo_aplicante " +
                "JOIN aplicaciones_vacantes ON aplicaciones_vacantes.id_aplicante = aplicantes.id_aplicante " +
                "JOIN resultados_aplicaciones ON resultados_aplicaciones.id_resultado_aplicacion = aplicaciones_vacantes.id_resultado_aplicacion " +
                "WHERE aplicantes.id_aplicante = @idApl";
            query.Parameters.AddWithValue("@idApl", idApl);
            return cn.selectQuery(query);
        }

        // INSERTS
        public void insertVacante(string codVac, string vacante, int idDpto, int cupo, string descripcion)
        {
            SqlCommand query1 = new SqlCommand();
            query1.CommandText = "INSERT INTO vacantes(codigo_vacante, vacante, id_departamento, cupo_vacante, descripcion) " +
                "VALUES(@codVac, @vacante, @idDpto, @cupo, @descripcion)";
            query1.Parameters.AddWithValue("@codVac", codVac);
            query1.Parameters.AddWithValue("@vacante", vacante);
            query1.Parameters.AddWithValue("@idDpto", idDpto);
            query1.Parameters.AddWithValue("@cupo", cupo);
            query1.Parameters.AddWithValue("@descripcion", descripcion);
            cn.insertQuery(query1);
        }

        public void insertRequisito (int idVac, string codVac, string requisito, string detalles, int idPrioridad)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "INSERT INTO requisitos_vacantes (id_vacante, codigo_vacante, requisito, detalles, id_prioridad_requisito) " +
                "VALUES(@idVac, @codVac, @reqVac, @detReqVac, @idPrioridadReqVac)";
            query.Parameters.AddWithValue("@idVac", idVac);
            query.Parameters.AddWithValue("@codVac", codVac);
            query.Parameters.AddWithValue("@reqVac", requisito);
            query.Parameters.AddWithValue("@detReqVac", detalles);
            query.Parameters.AddWithValue("@idPrioridadReqVac", idPrioridad);
            cn.insertQuery(query);
        }

        public void insertAplicante (int idVac, string nombre, string apellido, string correo, string telefono, string direccion, int idTipo, string dui)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "INSERT INTO aplicantes (id_tipo_aplicante, nombres, apellidos, email, telefono, direccion, dui) " +
                "VALUES (@idTipo, @nombre, @apellido, @email, @telefono, @direccion, @dui)";
            query.Parameters.AddWithValue("@idTipo", idTipo);
            query.Parameters.AddWithValue("@nombre", nombre);
            query.Parameters.AddWithValue("@apellido", apellido);
            query.Parameters.AddWithValue("@email", correo);
            query.Parameters.AddWithValue("@telefono", telefono);
            query.Parameters.AddWithValue("@direccion", direccion);
            query.Parameters.AddWithValue("@dui", dui);
            cn.insertQuery(query);

            SqlCommand query2 = new SqlCommand();
            query2.CommandText = "SELECT id_aplicante FROM aplicantes WHERE dui = @dui";
            query2.Parameters.AddWithValue("@dui", dui);
            DataSet result = cn.selectQuery(query2);
            int idApl = int.Parse(result.Tables[0].Rows[0][0].ToString());

            SqlCommand query3 = new SqlCommand();
            query3.CommandText = "INSERT INTO aplicaciones_vacantes (id_vacante, id_aplicante) " +
                "VALUES (@idVac, @idApl)";
            query3.Parameters.AddWithValue("@idVac", idVac);
            query3.Parameters.AddWithValue("@idApl", idApl);
            cn.insertQuery(query3);
        }

        // UPDATES
        public void updateVacante (string codVac, string newCodVac, string vacante, string descripcion, int idDpto, int idEstado, int cupo)
        {
            SqlCommand query1 = new SqlCommand();
            query1.CommandText = "UPDATE vacantes SET " +
                "vacante = @vacante, " +
                "codigo_vacante = @newCodVac, " +
                "descripcion = @descripcion, " +
                "id_departamento = @idDpto, " +
                "cupo_vacante = @cupo, " +
                "id_estado_vacante = @idEstado " +
                "WHERE codigo_vacante = @codVac";
            query1.Parameters.AddWithValue("@vacante", vacante);
            query1.Parameters.AddWithValue("@newCodVac", newCodVac);
            query1.Parameters.AddWithValue("@descripcion", descripcion);
            query1.Parameters.AddWithValue("@idDpto", idDpto);
            query1.Parameters.AddWithValue("@cupo", cupo);
            query1.Parameters.AddWithValue("@idEstado", idEstado);
            query1.Parameters.AddWithValue("@codVac", codVac);
            cn.updateQuery(query1);

            SqlCommand query2 = new SqlCommand();
            query2.CommandText = "UPDATE requisitos_vacantes SET " +
                "codigo_vacante = @newCodVac " +
                "WHERE codigo_vacante = @codVac";
            query2.Parameters.AddWithValue("@newCodVac", newCodVac);
            query2.Parameters.AddWithValue("@codVac", codVac);
            cn.updateQuery(query2);
        }

        public void updateRequisito (int idReq, string requisito, string detalles, int idPrioridad)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "UPDATE requisitos_vacantes SET " +
                "requisito = @requisito, " +
                "detalles = @detalles, " +
                "id_prioridad_requisito = @idPrioridad " +
                "WHERE id_requisitos_vacante = @idReq";
            query.Parameters.AddWithValue("@requisito", requisito);
            query.Parameters.AddWithValue("@detalles", detalles);
            query.Parameters.AddWithValue("@idPrioridad", idPrioridad);
            query.Parameters.AddWithValue("@idReq", idReq);
            cn.updateQuery(query);
        }

        public void updateAplicante (int idApl, string nombres, string apellidos, string dui, string correo, string direccion, string telefono, int idTipo, int idResultado)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "UPDATE aplicantes SET " +
                "nombres = @nombres, " +
                "apellidos = @apellidos, " +
                "email = @correo, " +
                "telefono = @telefono, " +
                "direccion = @direccion, " +
                "dui = @dui, " +
                "id_tipo_aplicante = @idTipo " +
                "WHERE id_aplicante = @idApl";
            query.Parameters.AddWithValue("@nombres", nombres);
            query.Parameters.AddWithValue("@apellidos", apellidos);
            query.Parameters.AddWithValue("@correo", correo);
            query.Parameters.AddWithValue("@telefono", telefono);
            query.Parameters.AddWithValue("@direccion", direccion);
            query.Parameters.AddWithValue("@dui", dui);
            query.Parameters.AddWithValue("@idTipo", idTipo);
            query.Parameters.AddWithValue("@idApl", idApl);
            cn.updateQuery(query);

            SqlCommand query2 = new SqlCommand();
            query2.CommandText = "UPDATE aplicaciones_vacantes SET " +
                "id_resultado_aplicacion = @idResult " +
                "WHERE id_aplicante = @idApl";
            query2.Parameters.AddWithValue("@idResult", idResultado);
            query2.Parameters.AddWithValue("@idApl", idApl);
            cn.updateQuery(query2);
        }

        // DELETES

        // Borra una vacante y sus requisitos
        public void deleteVacante (string codVac)
        {
            SqlCommand queryReq = new SqlCommand();
            queryReq.CommandText = "DELETE FROM requisitos_vacantes WHERE codigo_vacante = @codVac";
            queryReq.Parameters.AddWithValue("@codVac", codVac);
            cn.deleteQuery(queryReq);

            SqlCommand queryVac = new SqlCommand();
            queryVac.CommandText = "DELETE FROM vacantes WHERE codigo_vacante = @codVac";
            queryVac.Parameters.AddWithValue("@codVac", codVac);
            cn.deleteQuery(queryVac);
        }

        public void deleteRequisito (int idReq)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "DELETE FROM requisitos_vacantes WHERE id_requisitos_vacante = @idReq";
            query.Parameters.AddWithValue("@idReq", idReq);
            cn.deleteQuery(query);
        }

        public void deleteAplicante (int idApl)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "DELETE FROM aplicaciones_vacantes WHERE id_aplicante = @idApl";
            query.Parameters.AddWithValue("@idApl", idApl);
            cn.deleteQuery(query);

            SqlCommand query2 = new SqlCommand();
            query2.CommandText = "DELETE FROM aplicantes WHERE id_aplicante = @idApl";
            query2.Parameters.AddWithValue("@idApl", idApl);
            cn.deleteQuery(query2);
        }
    }
}