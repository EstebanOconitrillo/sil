using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;


namespace LBAcceso
{
    public class RegistroLabores
    {
        public static string getRegistros(string idL)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                if (idL.Equals("0"))
                    _comando.CommandText = @"SELECT R.id,convert(varchar,R.fecha,103) Fecha,convert(varchar(5),R.horaInicio,108) horaInicio,
                                                    convert(varchar(5), R.horaFinal, 108) horaFinal,S.nombre AS Sistema, A.nombre AS Accion, R.detalle,
                                                    R.observacion,R.avance, E.nombre AS Estado,R.idSistema,R.idEstado,R.idUnidad,R.idAccion
                                            FROM registros R, sistemas S, Acciones A, Estados E
                                            WHERE R.idSistema = S.id
                                            AND R.idAccion = A.id
                                            AND R.idEstado = E.id order by R.fecha, R.horaInicio";
                else
                    _comando.CommandText = @"SELECT R.id,convert(varchar,R.fecha,103) Fecha,convert(varchar(5),R.horaInicio,108) horaInicio,
                                                    convert(varchar(5), R.horaFinal, 108) horaFinal,S.nombre AS Sistema, A.nombre AS Accion, R.detalle,
                                                    R.observacion,R.avance, E.nombre AS Estado,R.idSistema,R.idEstado,R.idUnidad,R.idAccion
                                            FROM registros R, sistemas S, Acciones A, Estados E
                                            WHERE R.idSistema = S.id
                                            AND R.idAccion = A.id
                                            AND R.idEstado = E.id and R.id = " + idL;

                DataTable Dt = Metodos.EjecutarComandoSelect(_comando);

                foreach (DataRow row in Dt.Rows)
                {
                    lista.Add(new
                    {
                        id = row["id"].ToString().Trim(),
                        Fecha = row["Fecha"].ToString().Trim(),
                        horaInicio = row["horaInicio"].ToString().Trim(),
                        horaFinal = row["horaFinal"].ToString().Trim(),
                        Sistema = row["Sistema"].ToString().Trim(),
                        Accion = row["Accion"].ToString().Trim(),
                        detalle = row["detalle"].ToString().Trim(),
                        observacion = row["observacion"].ToString().Trim(),
                        avance = row["avance"].ToString().Trim(),
                        Estado = row["Estado"].ToString().Trim(),
                        idSistema = row["idSistema"].ToString().Trim(),
                        idEstado = row["idEstado"].ToString().Trim(),
                        idUnidad = row["idUnidad"].ToString().Trim(),
                        idAccion = row["idAccion"].ToString().Trim()
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

        public static string CrearLineaLabor(string idEmpleado, string Fecha, string horaInicio, string horaFinal, string idSistema, string idAccion, string detalle, string observacion, string avance, string idUnidad,string idEstado)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                //string Fec = Fecha.Substring(6, 4) + "-" + Fecha.Substring(3, 2) + "-" + Fecha.Substring(0, 2);  
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = @"insert into Registros ([idEmpleado],[fecha],[horaInicio],[horaFinal],[idSistema],[idAccion],[detalle],[observacion],[avance],[idUnidad],[idEstado])
                                        values(" + idEmpleado + ",convert(date, '"+Fecha+"', 103),'" + horaInicio + "','" + horaFinal + "'," + idSistema + "," + idAccion + ",'" + detalle + "','" + observacion + "'," + avance + "," + idUnidad + "," + idEstado + ")";
                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Registro creado");
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

        public static string EditarRegistro(string idRegistro, string Fecha, string horaInicio, string horaFinal, string idSistema, string idAccion, string detalle, string observacion, string avance)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = @"update Registros set fecha = convert(date, '" + Fecha + "', 103), horaInicio = '" + horaInicio + "', horaFinal = '" + horaFinal + 
                                         "',idSistema=" + idSistema + ",idAccion=" + idAccion + ",detalle='" + detalle + "',observacion='" + observacion + "',avance="+avance+
                                         "where id="+idRegistro;

                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Registro modificado");
            }
            catch (Exception e)
            {
                lista.Add("Error: " + e.Message);
            }

            resultado = JsonConvert.SerializeObject(lista, Newtonsoft.Json.Formatting.Indented);
            return resultado;
        }

        public static string EliminarRegistro(string id)
        {//ejecuta una consulta a la BD
            string resultado = string.Empty;
            List<dynamic> lista = new List<dynamic>();
            try
            {
                SqlCommand _comando = Metodos.CrearComando();
                _comando.CommandText = "delete Registros where id ="+id;

                int res = Metodos.EjecutarComando(_comando);

                lista.Add("Exito: Registro eliminado");
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