using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace wsRRHH
{
    public class Conexion
    {
        // SQL Laptop
        private static string dbUser = "DESKTOP-9LV57DI\\SQLEXPRESS";
        // SQL Desktop
        //private static string dbUser = "DESKTOP-F0AK4UN";
        private static string db = "etps4_rrhh";
        string cadena = $"Data Source = {dbUser}; Initial Catalog = {db}; Integrated Security = True";
        SqlConnection conexion;

        public SqlConnection conectar()
        {
            conexion = new SqlConnection();
            conexion.ConnectionString = cadena;
            return conexion;
        }

        public void abrir()
        {
            try
            {
                conexion.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error de BD: " + e.Message);
            }
        }

        public void cerrar()
        {
            try
            {
                conexion.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error de BD: " + e.Message);
            }
        }

        public DataSet selectQuery(SqlCommand query)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                SqlCommand q = new SqlCommand();
                q = query;
                q.Connection = conectar();
                adapter.SelectCommand = q;
                abrir();
                adapter.Fill(ds);
                cerrar();
                return ds;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ds;
            }
        }

        public Array DStoArr(DataSet ds)
        {
            ArrayList myArrayList = new ArrayList();
            foreach (DataRow dtRow in ds.Tables[0].Rows)
            {
                myArrayList.Add(dtRow);
            }
            return myArrayList.ToArray();
        }

        public void insertQuery(SqlCommand query)
        {
            try
            {
                SqlCommand q = new SqlCommand();
                q = query;
                q.Connection = conectar();
                abrir();
                query.ExecuteNonQuery();
                cerrar();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}