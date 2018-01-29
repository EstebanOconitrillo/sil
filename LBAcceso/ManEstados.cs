using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;


namespace LBAcceso
{
    public class ManEstados
    {
        public static string getEstados(string idE)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                if (idE.Equals("0"))
                    _comando.CommandText = "select id,nombre from Estados order by nombre";
                else
                    _comando.CommandText = "select id,nombre from Estados where id = " + idE;

                DataTable Dt = Metodos.EjecutarComandoSelect(_comando);

                foreach (DataRow row in Dt.Rows)
                {
                    lista.Add(new
                    {
                        id = row["id"].ToString().Trim(),
                        Nombre = row["nombre"].ToString().Trim()
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

        public static string CrearEstado(string nombre)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = "insert into Estados(nombre) values('" + nombre + "')";
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Estado creado");
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

        public static string EditarEstado(string nombre, string id)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = "update Estados set nombre = '" + nombre + "' where id = " + id;
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Estado modificado");
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

        public static string EliminarEstado(string id)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = "delete Estados where id = " + id;
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Estado eliminado");
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