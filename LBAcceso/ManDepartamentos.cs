
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;


namespace LBAcceso
{
    public class ManDepartamentos
    {
        public static string getDepartamentos(string idD)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                if (idD.Equals("0"))
                    _comando.CommandText = @"select d.id, d.nombre, d.idEstado, e.nombre as Estado
                                            from Departamentos d, Estados e
                                            where d.idEstado = e.id
                                            order by d.nombre";
                else
                    _comando.CommandText = @"select d.id, d.nombre, d.idEstado, e.nombre as Estado
                                            from Departamentos d, Estados e
                                            where d.idEstado = e.id
                                            and d.id =  " + idD +" order by d.nombre";

                DataTable Dt = Metodos.EjecutarComandoSelect(_comando);

                foreach (DataRow row in Dt.Rows)
                {
                    lista.Add(new
                    {
                        id = row["id"].ToString().Trim(),
                        nombre = row["nombre"].ToString().Trim(),
                        idEstado = row["idEstado"].ToString().Trim(),
                        Estado = row["Estado"].ToString().Trim()
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

        public static string CrearDepartamento(string nombre, string idEstado, string idDepartamento)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                //string Fec = Fecha.Substring(6, 4) + "-" + Fecha.Substring(3, 2) + "-" + Fecha.Substring(0, 2);  
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = @"insert into Departamentos ([nombre],[idEstado],[idDepartamento])
                                        values('" + nombre + "'," + idEstado + "," + idDepartamento + ")";
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Departamento creado");
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

        public static string EditarDepartamento(string id, string nombre, string idEstado, string idDepartamento)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = "update Departamentos set nombre = '" + nombre + "', idEstado = " + idEstado + ", idDepartamento = " + idDepartamento + " where id=" + id;
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Departamento modificado");
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

        public static string EliminarDepartamento(string id)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = "delete Departamentos where id = " + id;
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Departamento eliminado");
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