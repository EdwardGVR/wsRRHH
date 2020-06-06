﻿using System;
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

        // INSERTS
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
    }
}