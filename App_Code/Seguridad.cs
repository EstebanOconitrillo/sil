using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using System.Dynamic;
using JWT.Serializers;
using JWT;
using System;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;

public class Seguridad
{
    public static Usuario Usuario
    {
        get
        {
            dynamic payload = (IDictionary<string, object>)HttpContext.Current.Session["payload"];
            string token = (string)HttpContext.Current.Session["access_token"];
            Membresia membresia = (Membresia)HttpContext.Current.Session["membresia"];

            if (payload != null)
            {
                return new Usuario
                {
                    Iat = payload["iat"].ToString(),
                    Exp = payload["exp"].ToString(),
                    Sub = payload["sub"].ToString(),
                    Identificacion = payload["identificacion"].ToString(),
                    Org = payload["org"].ToString(),
                    Nombre = payload["nombre"].ToString(),
                    Iss = payload["iss"].ToString(),
                    AppId = payload["appid"].ToString(),
                    Token = token,
                    Membresia = membresia
                };
            }
            else
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return new Usuario
                {
                    Membresia = new Membresia { }
                };
            }
        }
    }

    public static void RequerirAuth()
    {
        var token = (HttpContext.Current.Request["access_token"] == null) ?
            HttpContext.Current.Session["access_token"] : HttpContext.Current.Request["access_token"];

        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            if (token != null && ValidarToken(token))
            {
                Autenticar(token);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request["access_token"]))
                {
                    HttpContext.Current.Response.Redirect("~/");
                }
            }
            else
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return;
            }
        }
        else
        {
            if (ValidarToken(token))
            {
                Autenticar(token);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request["access_token"]))
                {
                    HttpContext.Current.Response.Redirect("~/");
                }
            }
            else
            {
                FormsAuthentication.SignOut();
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return;
            }
        }
    }

    private static void Autenticar(object token)
    {
        try
        {
            string secreto = System.Configuration.ConfigurationManager.AppSettings["secreto"];

            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
            var payload = decoder.DecodeToObject(token.ToString(), secreto, verify: true) as IDictionary<string, object>;
            Membresia membresia;
            System.Net.ServicePointManager.CertificatePolicy = new CustomCertificatePolicy();

            using (WebClient c = new WebClient())
            {
                c.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                c.Encoding = Encoding.UTF8;

                string uri = string.Format(@"{0}/membresia", payload["iss"]+":8000");
                string response = c.DownloadString(uri);


                membresia = JsonConvert.DeserializeObject<Membresia>(response,
                   new JsonSerializerSettings
                   {
                       NullValueHandling = NullValueHandling.Ignore
                   });
            }

            HttpContext.Current.Session.Add("access_token", token);
            HttpContext.Current.Session.Add("payload", payload);
            HttpContext.Current.Session.Add("membresia", membresia);
            FormsAuthentication.SetAuthCookie(payload["sub"].ToString(), false);

            if (membresia.ACL.Count() > 0)
            {
                //if (!membresia.ACL.Keys.Contains(HttpContext.Current.Request.Url.AbsolutePath.ToLower()))
                if (!membresia.ACL.Contains(HttpContext.Current.Request.Url.AbsolutePath.ToLower()))
                {
                    HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
        }
        catch (SignatureVerificationException e)
        {
            Log(string.Format("LOGOUT por invalidación de token: {0}", e.Message));
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            HttpContext.Current.Response.StatusDescription = string.Format("No autorizado: {0}", e.Message);

            return;
        }
    }

    private static bool ValidarToken(object token)
    {
        if (token != null)
        {
            try
            {
                string secreto = System.Configuration.ConfigurationManager.AppSettings["secreto"];
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                var payload = decoder.DecodeToObject(token.ToString(), secreto, verify: true) as IDictionary<string, object>;

                return true;
            }
            catch (TokenExpiredException e)
            {
                // Log(string.Format("LOGOUT por invalidación de token: {0}", e.Message));
                FormsAuthentication.SignOut();
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                HttpContext.Current.Response.StatusDescription = string.Format("No autorizado: {0}", e.Message);

                return false;
            }
            catch (SignatureVerificationException e)
            {
                //   Log(string.Format("LOGOUT por invalidación de token: {0}", e.Message));
                FormsAuthentication.SignOut();
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                HttpContext.Current.Response.StatusDescription = string.Format("No autorizado: {0}", e.Message);

                return false;
            }
        }
        else
            return false;
    }

    public static bool EstaAuntenticado()
    {
        return HttpContext.Current.User.Identity.IsAuthenticated;
    }

    public static Usuario ObtenerUsuario()
    {
        if (HttpContext.Current.Session["payload"] != null)
        {
            dynamic usuario = JsonConvert.DeserializeObject<dynamic>(HttpContext.Current.Session["payload"].ToString());

            return JsonConvert.DeserializeObject<dynamic>(usuario.esquema);
        }
        else
        {
            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            return null;
        }
    }

    public static dynamic Administrar()
    {
        string response = "";

        try
        {
            using (WebClient c = new WebClient())
            {
                if (HttpContext.Current.Session["access_token"] != null)
                {
                    c.Headers[HttpRequestHeader.Authorization] = "Bearer " + HttpContext.Current.Session["access_token"];
                    c.Encoding = System.Text.Encoding.UTF8;
                    dynamic payload = (IDictionary<string, object>)HttpContext.Current.Session["payload"];
                    response = c.DownloadString(string.Format(@"{0}/aplicaciones/{1}/admin", payload["iss"], payload["appid"]));
                }
                else
                {
                    Log(string.Format("LOGOUT por vencimiento del tiempo de sesión"));
                    FormsAuthentication.SignOut();
                }
            }

            return response;
        }
        catch (Exception e)
        {
            Log(string.Format("{0}. {1}. {2}", "Ocurrió un error al intentar obtener interfaz de administración", e.Message, e.StackTrace));
            throw new HttpException(500,
                string.Format("{0}. {1}. {2}", "Ocurrió un error al intentar obtener interfaz de administración", e.Message, e.StackTrace));
        }
    }

    public static dynamic Bitacora()
    {
        string response = "";

        try
        {
            using (WebClient c = new WebClient())
            {
                if (HttpContext.Current.Session["access_token"] != null)
                {
                    c.Headers[HttpRequestHeader.Authorization] = "Bearer " + HttpContext.Current.Session["access_token"];
                    c.Encoding = System.Text.Encoding.UTF8;
                    dynamic payload = (IDictionary<string, object>)HttpContext.Current.Session["payload"];
                    //string url = string.Format(@"{0}/aplicaciones/{1}/bitacora", System.Configuration.ConfigurationManager.AppSettings["urlSso"], System.Configuration.ConfigurationManager.AppSettings["key"]);
                    string url = string.Format(@"{0}/aplicaciones/{1}/log", payload["iss"], payload["appid"]);
                    response = c.DownloadString(url);
                }
                else
                {
                    Log(string.Format("LOGOUT por vencimiento del tiempo de sesión"));
                    FormsAuthentication.SignOut();
                }
            }

            return response;
        }
        catch (Exception e)
        {
            Log(string.Format("{0}. {1}. {2}", "Ocurrió un error al intentar obtener interfaz de bitácora", e.Message, e.StackTrace));
            throw new HttpException(500,
                string.Format("{0}. {1}. {2}", "Ocurrió un error al intentar obtener interfaz de bitácora", e.Message, e.StackTrace));
        }
    }

    public static void Log(string mensaje)
    {
        string data = string.Format(@"user_id={0}&log={1}", HttpContext.Current.User.Identity.Name, mensaje);

        try
        {
            using (WebClient c = new WebClient())
            {
                c.Encoding = System.Text.Encoding.UTF8;
                c.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=utf-8";
                c.Headers[HttpRequestHeader.Authorization] = "Bearer " + Usuario.Token;  // Si el token del usuario es inválido no sería posible agregar log
                dynamic payload = (IDictionary<string, object>)HttpContext.Current.Session["payload"];
                string HtmlResult = c.UploadString(
                    string.Format(@"{0}/aplicaciones/{1}/log", payload["iss"], payload["appid"]),
                    data);
            }
        }
        catch (Exception e)
        {
            throw new HttpException(500,
                string.Format("{0}. {1}. {2}", "Ocurrió un error al intentar enviar un log a IU", e.Message, e.StackTrace));
        }
    }

    public static string[] ObtenerBotonesPorUrl(string url)
    {
        if ((Membresia)HttpContext.Current.Session["membresia"] != null)
            return Usuario.Membresia.Botones[url.ToLower()];
        else
            throw new HttpException(401, "Acceso no autorizado");
    }

    public static string[] ObtenerAccionesPorUrl(string url)
    {
        if ((Membresia)HttpContext.Current.Session["membresia"] != null)
            return Usuario.Membresia.Acciones[url.ToLower()];
        else
            throw new HttpException(401, "Acceso no autorizado");
    }
}

public class Usuario
{
    public string Iat { get; set; }
    public string Exp { get; set; }
    public string Sub { get; set; }
    public string Identificacion { get; set; }
    public string Org { get; set; }
    public string Nombre { get; set; }
    public string AppId { get; set; }
    public string Iss { get; set; }
    public string Token { get; set; }
    public Membresia Membresia { get; set; }
}

public class Esquema
{
    public string Key { get; set; }
    public bool Rg { get; set; }
    public string DisplayName { get; set; }
    public List<Esquema> Items { get; set; }
    public List<string> Acciones { get; set; }
    public List<string> Botones { get; set; }
    public string Url { get; set; }
}

[JsonConverter(typeof(MembresiaSerializer))]
public class Membresia
{
    public Esquema Esquema { get; set; }
    public string HtmlMenu { get; set; }
    public Dictionary<string, string[]> Botones { get; set; }
    public Dictionary<string, string[]> Acciones { get; set; }
    public string[] ACL { get; set; }
    public List<Rol> Roles { get; set; }
    public int[] RolesId { get; set; }
}

public class Rol
{
    public int Id { get; set; }
    public string Activo { get; set; }
    public string Nombre { get; set; }
}

public class BotonKey
{
    public string Url { get; set; }
    public string[] Botones { get; set; }
}

public class AccionKey
{
    public string Url { get; set; }
    public string[] Acciones { get; set; }
}

public class MembresiaSerializer : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        throw new NotImplementedException();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        try
        {
            JObject jsonObject = JObject.Load(reader);
            IEnumerable<JProperty> properties = jsonObject.Properties();
            List<BotonKey> botonesList = new List<BotonKey>();
            botonesList = JsonConvert.DeserializeObject<List<BotonKey>>(jsonObject["botones"].ToString());
            Dictionary<string, string[]> botones = botonesList.ToDictionary(x => x.Url, x => x.Botones);

            List<AccionKey> accionesList = new List<AccionKey>();
            accionesList = JsonConvert.DeserializeObject<List<AccionKey>>(jsonObject["acciones"].ToString());
            Dictionary<string, string[]> acciones = accionesList.ToDictionary(x => x.Url, x => x.Acciones);

            return new Membresia
            {
                Esquema = jsonObject["esquema"].ToObject<Esquema>(),
                HtmlMenu = jsonObject["htmlMenu"].ToString(),
                ACL = jsonObject["acl"].ToObject<string[]>(),
                Botones = botones,
                Acciones = acciones,
                Roles = jsonObject["roles"].ToObject<List<Rol>>(),
                RolesId = jsonObject["rolesId"].ToObject<int[]>()
            };
        }
        catch (Exception e)
        {
            throw new HttpException(500, string.Format("{0}. {1}. {2}.", "Error al intentar deserializar la información de la membresia",
                e.Message, e.StackTrace));
        }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

/**
 * Error:  The underlying connection was closed. Could not establish trust relationship with remote server.
 * Esta clase es una de las maneras de solucionarlo junto con: 
 * System.Net.ServicePointManager.CertificatePolicy = new CustomCertificatePolicy()
 * antes de cada llamada a servicio de IUSSO
 **/
public class CustomCertificatePolicy : ICertificatePolicy
{
    public bool CheckValidationResult(ServicePoint sp, X509Certificate cert, WebRequest req, int problem)
    {
        // forzar la aceptación del certificado digital
        return true;
    }
}