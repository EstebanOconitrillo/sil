
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;


namespace LBAcceso
{
    public class ManEmpleados
    {
        public static string getEmpleados(string idE)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                if (idE.Equals("0"))
                    _comando.CommandText = @"select e.id, e.nombre, e.idUnidad, e.idRol, u.nombre as Unidad, r.nombre as Rol
                                            from Empleados e, Unidades u, Roles r
                                            where e.idUnidad = u.id and e.idRol = r.id
                                            order by e.nombre";
                else
                    _comando.CommandText = @"select e.id, e.nombre, e.idUnidad, e.idRol, u.nombre as Unidad, r.nombre as Rol
                                            from Empleados e, Unidades u, Roles r
                                            where e.idUnidad = u.id and e.idRol = r.id
                                            and e.id =  " + idE + " order by e.nombre";

                DataTable Dt = Metodos.EjecutarComandoSelect(_comando);

                foreach (DataRow row in Dt.Rows)
                {
                    lista.Add(new
                    {
                        id = row["id"].ToString().Trim(),
                        nombre = row["nombre"].ToString().Trim(),
                        idUnidad = row["idUnidad"].ToString().Trim(),
                        idRol = row["idRol"].ToString().Trim(),
                        Unidad = row["Unidad"].ToString().Trim(),
                        Rol = row["Rol"].ToString().Trim()
                    });
                }
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

        public static string CrearEmpleado(string nombre, string idUnidad, string idRol)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                //string Fec = Fecha.Substring(6, 4) + "-" + Fecha.Substring(3, 2) + "-" + Fecha.Substring(0, 2);  
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = @"insert into Empleados ([nombre],[idUnidad],[idRol])
                                        values('" + nombre + "'," + idUnidad + "," + idRol + ")";
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Empleado creado");
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

        public static string EditarEmpleado(string id, string nombre, string idUnidad, string idRol)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                //_comando.CommandText = "update Empleados set nombre = '" + nombre + "', idUnidad= " + idUnidad + ", idRol= " + idRol + " where id=" + id;
                _comando.CommandText = "update Empleados set nombre = '" + nombre + "', idUnidad= " + idUnidad + ", idRol= " + 4 + " where id=" + id;
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Empleado modificado");
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

        public static string EliminarEmpleado(string id)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = "delete Empleados where id = " + id;
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Empleado eliminado");
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

    }
}