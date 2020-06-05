﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Security.Cryptography;
using wsRRHH.DAL;

namespace wsRRHH
{
    /// <summary>
    /// Descripción breve de webServRRHH
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class webServRRHH : System.Web.Services.WebService
    {
        Conexion cn = new Conexion();
        Vacantes vacantes = new Vacantes();
        Capacitaciones capacitaciones = new Capacitaciones();
        Empleados empleados = new Empleados();
        Evaluaciones evaluaciones = new Evaluaciones();
        Usuarios usuarios = new Usuarios();
        Departamentos departamentos = new Departamentos();

        // CONSULTAS
        [WebMethod]
        public DataSet getVacantes(int top = 0)
        {
            return vacantes.getVacantes(top);
        }

        [WebMethod]
        public DataSet getPrioridadesRequisitos()
        {
            return vacantes.getPrioridades();
        }

        [WebMethod]
        public DataSet getDetallesVacante (string codVac)
        {
            return vacantes.getDetallesVacante(codVac);
        }

        [WebMethod]
        public DataSet getRequisitosVac (string codVac)
        {
            return vacantes.getRequisitosVac(codVac);
        }

        [WebMethod]
        // Obtiene el siguiente correlativo de la vacante segun departamento
        public int getCorrVac (int idDpto)
        {
            return vacantes.getCorrVac(idDpto);
        }

        [WebMethod]
        public int getIdVac (string codVac)
        {
            return vacantes.getIdVac(codVac);
        }

        [WebMethod]
        public DataSet getCapacitaciones(int top = 0)
        {
            return capacitaciones.getCapacitaciones(top);
        }

        [WebMethod]
        public DataSet getEmpleados()
        {
            return empleados.getEmpleados();
        }

        [WebMethod]
        public DataSet getEvaluaciones(int top = 0)
        {
            return evaluaciones.getEvaluaciones(top);
        }

        [WebMethod]
        public DataSet getUsuarios()
        {
            return usuarios.getUsuarios();
        }

        [WebMethod]
        public DataSet getNombresDepartamentos()
        {
            return departamentos.getNombresDepartamentos();
        }

        [WebMethod]
        public int getDptoID(string departamento)
        {
            return departamentos.getDptoID(departamento);
        }

        [WebMethod]
        public string getDptoAbv (int idDpto)
        {
            return departamentos.getDptoAbv(idDpto);
        }

        [WebMethod]
        public bool validarLogin(string user, string clave)
        {
            return usuarios.validarLogin(user, clave);
        }

        // INSERTS
        [WebMethod]
        public void insertVacante(string codVac, string vacante, int idDpto, int cupo, string descripcion)
        {
            vacantes.insertVacante(codVac, vacante, idDpto, cupo, descripcion);
        }

        [WebMethod]
        public void insertRequisito(int idVac, string codVac, string requisito, string detalles, int idPrioridad)
        {
            vacantes.insertRequisito(idVac, codVac, requisito, detalles, idPrioridad);
        }

        // DELETES
        [WebMethod]
        public void deleteVacante (string vacCode)
        {
            vacantes.deleteVacante(vacCode);
        }
    }
}
