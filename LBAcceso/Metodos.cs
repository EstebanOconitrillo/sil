
using System;
using System.Data;
using System.Data.SqlClient;

namespace LBAcceso
{
    public class Metodos
    {
        public static SqlCommand CrearComando()
        {
            string _cadenaConexion = Conexion.CadenaConexion;
            SqlConnection _conexion = new SqlConnection();
            _conexion.ConnectionString = _cadenaConexion;
            SqlCommand _comando = new SqlCommand();
            _comando = _conexion.CreateCommand();
            _comando.CommandType = CommandType.Text;
            return _comando;
        }
        public static SqlCommand CrearComandoProc(string proc)
        {//para ejecutar un SP
            string _cadenaConexion = Conexion.CadenaConexion;
            SqlConnection _conexion = new SqlConnection(_cadenaConexion);
            SqlCommand _comando = new SqlCommand(proc, _conexion);
            _comando.CommandType = CommandType.StoredProcedure;
            return _comando;
        }

        public static int EjecutarComando(SqlCommand comando)
        { //para ejecutar comando tipo update o delete
            try
            {
                comando.Connection.Open();
                return comando.ExecuteNonQuery();
            }
            catch { throw; }
            finally
            {
                comando.Connection.Dispose();
                comando.Connection.Close();
            }
        }

        public static DataTable EjecutarComandoSelect(SqlCommand comando)
        {//ejecuta consulta que devuelve un grupo de datos
            DataTable _tabla = new DataTable();
            try
            {
                comando.Connection.Open();
                SqlDataAdapter adaptador = new SqlDataAdapter();
                adaptador.SelectCommand = comando;
                adaptador.Fill(_tabla);
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { comando.Connection.Close(); }

            return _tabla;
        }

    }
}