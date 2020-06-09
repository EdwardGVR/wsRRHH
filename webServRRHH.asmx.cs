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
        public DataSet getEstadosVacantes ()
        {
            return vacantes.getEstadosVacantes();
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
        public DataSet getDetallesRequisito (int idReq)
        {
            return vacantes.getDetallesRequisito(idReq);
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
        public DataSet getDepartamentos ()
        {
            return departamentos.getDepartamentos();
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
        public DataSet getPuestos ()
        {
            return departamentos.getPuestos();
        }

        [WebMethod]
        public bool validarLogin(string user, string clave)
        {
            return usuarios.validarLogin(user, clave);
        }

        [WebMethod]
        public DataSet getDetallesEmpleado (int id)
        {
            return empleados.getDetallesEmpleado(id);
        }

        [WebMethod]
        public DataSet getTiposEvaluaciones ()
        {
            return evaluaciones.getTiposEvaluaciones();
        }

        [WebMethod]
        public DataSet getNivelesUsuarios ()
        {
            return usuarios.getNiveles();
        }

        [WebMethod]
        public DataSet getDetallesCapacitacion (int idCap)
        {
            return capacitaciones.getDetallesCapacitacion(idCap);
        }

        [WebMethod]
        public DataSet getAsignCapacitaciones (int idCap)
        {
            return capacitaciones.getAsignCapacitaciones(idCap);
        } 

        [WebMethod]
        public DataSet getEmpleadosByDpto (int idDpto)
        {
            return empleados.getEmpleadosByDpto(idDpto);
        }

        [WebMethod]
        public Boolean validateCapEmp (int idCap, int idEmp)
        {
            return capacitaciones.validateCapEmp(idCap, idEmp);
        }

        [WebMethod]
        public DataSet getDetallesEvaluacion(int idEval)
        {
            return evaluaciones.getDetallesEvaluacion(idEval);
        }

        [WebMethod]
        public DataSet getAsignEvalApl (int idEval)
        {
            return evaluaciones.getAsignEvalApl(idEval);
        }

        [WebMethod]
        public DataSet getAplicantesByVac (int idEval)
        {
            return evaluaciones.getAplicantesByVac(idEval);
        }

        [WebMethod]
        public Boolean validateEvApl (int idEval, int idApl)
        {
            return evaluaciones.validateEvApl(idEval, idApl);
        }

        // UPDATES
        [WebMethod]
        public void updateVacante (string codVac, string newCodVac, string vacante, string descripcion, int idDpto, int idEstado, int cupo)
        {
            vacantes.updateVacante(codVac, newCodVac, vacante, descripcion, idDpto, idEstado, cupo);
        }

        [WebMethod]
        public void updateRequisito (int idReq, string requisito, string detalles, int idPrioridad)
        {
            vacantes.updateRequisito(idReq, requisito, detalles, idPrioridad);
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

        [WebMethod]
        public void insertEmpleado (string nombres, string apellidos, string dui, string email, string telefono1, string telefono2, string direccion, int idDpto, int idCargo, double salario)
        {
            empleados.insertEmpleado(nombres, apellidos, dui, email, telefono1, telefono2, direccion, idDpto, idCargo, salario);
        }

        [WebMethod]
        public void insertCapacitacion (string titulo, string descripcion, int cupo, int idDpto)
        {
            capacitaciones.insertCapacitacion(titulo, descripcion, cupo, idDpto);
        }

        [WebMethod]
        public void insertEvaluacion (string evaluacion, int idTipo, string objetivos, int maxScore)
        {
            evaluaciones.insertEvaluacion(evaluacion, idTipo, objetivos, maxScore);
        }

        [WebMethod]
        public void insertUsuario (string nombres, string apellidos, string email, string usuario, string password, int idNivel)
        {
            usuarios.insertUsuario(nombres, apellidos, email, usuario, password, idNivel);
        }

        [WebMethod]
        public void asignarEmpCap (int idCap, int idEmp, string codigo)
        {
            capacitaciones.asignarEmpCap(idCap, idEmp, codigo);
        }

        [WebMethod]
        public void asignarAplEval (int idEval, int idAplicante, string fechaEvaluacion, string horaEvaluacion)
        {
            evaluaciones.asignAplEval(idEval, idAplicante, fechaEvaluacion, horaEvaluacion);
        } 

        // DELETES
        [WebMethod]
        public void deleteVacante (string vacCode)
        {
            vacantes.deleteVacante(vacCode);
        }

        [WebMethod]
        public void deleteRequisito (int idReq)
        {
            vacantes.deleteRequisito(idReq);
        }
    }
}
