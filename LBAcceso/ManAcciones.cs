using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;


namespace LBAcceso
{
    public class ManAcciones
    {
        public static string getAcciones(string idA)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                if (idA.Equals("0"))
                    _comando.CommandText = @"select a.id, a.nombre, a.idEstado, e.nombre as Estado
                                            from Acciones a, Estados e
                                            where a.idEstado = e.id
                                            order by a.nombre";
                else
                    _comando.CommandText = @"select a.id, a.nombre, a.idEstado, e.nombre as Estado
                                            from Acciones a, Estados e
                                            where a.idEstado = e.id
                                            and a.id =  " + idA + " order by a.nombre";

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

        public static string CrearAccion(string nombre, string idEstado)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                //string Fec = Fecha.Substring(6, 4) + "-" + Fecha.Substring(3, 2) + "-" + Fecha.Substring(0, 2);  
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = @"insert into Acciones ([nombre],[idEstado])
                                        values('" + nombre + "'," + idEstado + ")";
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Acción creada");
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

        public static string EditarAccion(string id, string nombre, string idEstado)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = "update Acciones set nombre = '" + nombre + "', idEstado = " + idEstado + " where id=" + id;
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Acción modificada");
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

        public static string EliminarAccion(string id)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = "delete Acciones where id = " + id;
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Acción eliminada");
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