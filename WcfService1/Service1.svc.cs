

using System;


namespace WcfService1
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Service1 : IService1
    {
        // Métodos de Estados
        public string getEstados(string idE)
        {
            //string datos = AccesoDatos.AccesoDatos.getEstados(idE);
            string datos = LBAcceso.ManEstados.getEstados(idE);
            return datos.ToString();
        }

        public string CrearEstado(string nombre)
        {
            string datos = LBAcceso.ManEstados.CrearEstado(nombre);
            return datos.ToString();
        }

        public string EditarEstado(string nombre, string id)
        {
            string datos = LBAcceso.ManEstados.EditarEstado(nombre, id);
            return datos.ToString();
        }

        public string EliminarEstado(string id)
        {
            string datos = LBAcceso.ManEstados.EliminarEstado(id);
            return datos.ToString();
        }

        // Métodos de Departamentos
        public string getDepartamentos(string idD)
        {
            string datos = LBAcceso.ManDepartamentos.getDepartamentos(idD);
            return datos.ToString();
        }

        public string CrearDepartamento(string nombre, string idEstado, string idDepartamento)
        {
            string datos = LBAcceso.ManDepartamentos.CrearDepartamento(nombre, idEstado, idDepartamento);
            return datos.ToString();
        }

        public string EditarDepartamento(string id, string nombre, string idEstado, string idDepartamento)
        {
            string datos = LBAcceso.ManDepartamentos.EditarDepartamento(id, nombre, idEstado, idDepartamento);
            return datos.ToString();
        }

        public string EliminarDepartamento(string id)
        {
            string datos = LBAcceso.ManDepartamentos.EliminarDepartamento(id);
            return datos.ToString();
        }

        //Métodos de Registros
        public string getRegistros(string idL)
        {
            string datos = LBAcceso.RegistroLabores.getRegistros(idL);
            return datos.ToString();
        }

        public string CrearLineaLabor(string idEmpleado, string Fecha, string horaInicio, string horaFinal, string idSistema, string idAccion, string detalle, string observacion, string avance, string idUnidad, string idEstado)
        {
            string datos = LBAcceso.RegistroLabores.CrearLineaLabor(idEmpleado, Fecha, horaInicio, horaFinal, idSistema, idAccion, detalle, observacion, avance, idUnidad, idEstado);
            return datos.ToString();
        }

        public string EditarRegistro(string idRegistro, string Fecha, string horaInicio, string horaFinal, string idSistema, string idAccion, string detalle, string observacion, string avance)
        {
            string datos = LBAcceso.RegistroLabores.EditarRegistro(idRegistro, Fecha, horaInicio, horaFinal, idSistema, idAccion, detalle, observacion, avance);
            return datos.ToString();
        }

        public string EliminarRegistro(string id)
        {
            string datos = LBAcceso.RegistroLabores.EliminarRegistro(id);
            return datos.ToString();
        }

        //Métodos de Sistemas
        public string getSistemas(string idS)
        {
            string datos = LBAcceso.ManSistemas.getSistemas(idS);
            return datos.ToString();
        }

        public string CrearSistema(string nombre, string idEstado, string idUnidad)
        {
            string datos = LBAcceso.ManSistemas.CrearSistema(nombre, idEstado, idUnidad);
            return datos.ToString();
        }

        public string EditarSistema(string id, string nombre, string idEstado, string idUnidad)
        {
            string datos = LBAcceso.ManSistemas.EditarSistema(id, nombre, idEstado, idUnidad);
            return datos.ToString();
        }

        public string EliminarSistema(string id)
        {
            string datos = LBAcceso.ManSistemas.EliminarSistema(id);
            return datos.ToString();
        }

        //Métodos de Acciones
        public string getAcciones(string idA)
        {
            string datos = LBAcceso.ManAcciones.getAcciones(idA);
            return datos.ToString();
        }

        public string CrearAccion(string nombre, string idEstado)
        {
            string datos = LBAcceso.ManAcciones.CrearAccion(nombre, idEstado);
            return datos.ToString();
        }

        public string EditarAccion(string id, string nombre, string idEstado)
        {
            string datos = LBAcceso.ManAcciones.EditarAccion(id, nombre, idEstado);
            return datos.ToString();
        }

        public string EliminarAccion(string id)
        {
            string datos = LBAcceso.ManAcciones.EliminarAccion(id);
            return datos.ToString();
        }

        //Métodos de Unidades
        public string getUnidades(string idU)
        {
            string datos = LBAcceso.ManUnidades.getUnidades(idU);
            return datos.ToString();
        }

        public string CrearUnidad(string nombre, string idEstado, string idDepartamento)
        {
            string datos = LBAcceso.ManUnidades.CrearUnidad(nombre, idEstado, idDepartamento);
            return datos.ToString();
        }

        public string EditarUnidad(string id, string nombre, string idEstado, string idDepartamento)
        {
            string datos = LBAcceso.ManUnidades.EditarUnidad(id, nombre, idEstado, idDepartamento);
            return datos.ToString();
        }

        public string EliminarUnidad(string id)
        {
            string datos = LBAcceso.ManUnidades.EliminarUnidad(id);
            return datos.ToString();
        }

        // Métodos de Empleados
        public string getEmpleados(string idE)
        {
            string datos = LBAcceso.ManEmpleados.getEmpleados(idE);
            return datos.ToString();
        }

        public string CrearEmpleado(string nombre, string idUnidad, string idRol)
        {
            string datos = LBAcceso.ManEmpleados.CrearEmpleado(nombre, idUnidad, idRol);
            return datos.ToString();
        }

        public string EditarEmpleado(string id, string nombre, string idUnidad, string idRol)
        {
            string datos = LBAcceso.ManEmpleados.EditarEmpleado(id, nombre, idUnidad, idRol);
            return datos.ToString();
        }

        public string EliminarEmpleado(string id)
        {
            string datos = LBAcceso.ManEmpleados.EliminarEmpleado(id);
            return datos.ToString();
        }
    }
}
