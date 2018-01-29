using System.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace WcfService1
{
    [ServiceContract]
    public interface IService1
    {
        //ESTADOS
        [OperationContract]
        string getEstados(string IdE);

        [OperationContract]
        string CrearEstado(string nombre);

        [OperationContract]
        string EditarEstado(string nombre, string id);

        [OperationContract]
        string EliminarEstado(string id);

        //DEPARTAMENTOS
        [OperationContract]
        string getDepartamentos(string IdD);

        [OperationContract]
        string CrearDepartamento(string nombre, string idEstado, string idDepartamento);

        [OperationContract]
        string EditarDepartamento(string id, string nombre, string idEstado, string idDepartamento);

        [OperationContract]
        string EliminarDepartamento(string id);

        //REGISTROS
        [OperationContract]
        string getRegistros(string idL);

        [OperationContract]
        string CrearLineaLabor(string idEmpleado, string Fecha, string horaInicio, string horaFinal, string idSistema, string idAccion, string detalle, string observacion, string avance, string idUnidad, string idEstado);

        [OperationContract]
        string EditarRegistro(string idRegistro, string Fecha, string horaInicio, string horaFinal, string idSistema, string idAccion, string detalle, string observacion, string avance);

        [OperationContract]
        string EliminarRegistro(string id);

        //SISTEMAS
        [OperationContract]
        string getSistemas(string idS);

        [OperationContract]
        string CrearSistema(string nombre, string idEstado, string idUnidad);

        [OperationContract]
        string EditarSistema(string id, string nombre, string idEstado, string idUnidad);

        [OperationContract]
        string EliminarSistema(string id);

        //ACCIONES
        [OperationContract]
        string getAcciones(string idA);

        [OperationContract]
        string CrearAccion(string nombre, string idEstado);

        [OperationContract]
        string EditarAccion(string id, string nombre, string idEstado);

        [OperationContract]
        string EliminarAccion(string id);

        //UNIDADES
        [OperationContract]
        string getUnidades(string idU);

        [OperationContract]
        string CrearUnidad(string nombre, string idEstado, string idDepartamento);

        [OperationContract]
        string EditarUnidad(string id, string nombre, string idEstado, string idDepartamento);

        [OperationContract]
        string EliminarUnidad(string id);

        //EMPLEADOS
        [OperationContract]
        string getEmpleados(string IdE);

        [OperationContract]
        string CrearEmpleado(string nombre, string idUnidad, string idRol);

        [OperationContract]
        string EditarEmpleado(string id, string nombre, string idUnidad, string idRol);

        [OperationContract]
        string EliminarEmpleado(string id);
    }



}