using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace wsRRHH.DAL
{
    public class Departamentos
    {
        Conexion cn = new Conexion();

        public DataSet getNombresDepartamentos()
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT departamento FROM departamentos";
            return cn.selectQuery(query);
        }

        public int getDptoID(string departamento)
        {
            int id;
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT id_departamento FROM departamentos WHERE departamento = @departamento";
            query.Parameters.AddWithValue("@departamento", departamento);
            DataSet result = cn.selectQuery(query);
            id = int.Parse(result.Tables[0].Rows[0][0].ToString());
            return id;
        }

        //Obtiene la abreviatura del departamento
        public string getDptoAbv(int idDpto)
        {
            SqlCommand query = new SqlCommand();
            query.CommandText = "SELECT abreviatura FROM departamentos WHERE id_departamento = @idDpto";
            query.Parameters.AddWithValue("@idDpto", idDpto);
            DataSet result = cn.selectQuery(query);
            return result.Tables[0].Rows[0][0].ToString();
        }
    }
}