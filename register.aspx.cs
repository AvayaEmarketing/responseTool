using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

public partial class register : System.Web.UI.Page
{



    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public class Datos
    {

        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Empresa { get; set; }
        public string Puesto { get; set; }
        public string Telefono { get; set; }
        public string Extension { get; set; }
        public string q1 { get; set; }
        public string q2 { get; set; }
        public string q3 { get; set; }
        public string q4 { get; set; }
        public string q5 { get; set; }
        public string q6 { get; set; }
        public string q7 { get; set; }
        public string q8 { get; set; }
        public string q9 { get; set; }
        public string Register_date { get; set; }
        
    }

    [WebMethod]
    public static string getCountries()
    {
        string result;
        SqlDataReader datos;
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["calawebConnectionString"].ToString();
        string strSQL = "SELECT idCountry,Country from C_Country order by Country";
        SqlCommand cmd = new SqlCommand(strSQL, con);
        try
        {
            con.Open();
            datos = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(datos);
            result = DataTableToJSON(dt);
            //result = new JavaScriptSerializer().Serialize(dt);
            con.Close();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result = "";
        }
        finally
        {
            con.Close();
        }
        return result;

    }

    public static string DataTableToJSON(DataTable table)
    {
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        foreach (DataRow row in table.Rows)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (DataColumn col in table.Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            list.Add(dict);
        }
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Serialize(list);
    }

    [WebMethod]
    public static string getDatosReg(string tipo_usuario)
    {
        string result;
        SqlDataReader datos;
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["calawebConnectionString"].ToString();
        string strSQL = "SELECT id,Nombre,Puesto,Empresa,Email,Telefono,Extension,q1,q2,q3,q4,q5,q6,q7,q8,q9,Register_date FROM calaweb.Cala_Web.Esme_Survey012014 where tipo_cliente = @tipo_usuario";
        SqlCommand cmd = new SqlCommand(strSQL, con);
        cmd.Parameters.Add("@tipo_usuario", SqlDbType.VarChar, 20);
        cmd.Parameters["@tipo_usuario"].Value = tipo_usuario;

        try
        {
            con.Open();
            datos = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(datos);
            result = DataTableToJSON(dt);
            //result = JsonConvert.SerializeObject(dt);
            con.Close();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result = "";
        }
        finally
        {
            con.Close();
        }
        return result;

    }


                                                                                                                                                                                                             

    [WebMethod]
    public static string putData(string nombre, string email, string empresa, string titleContact, string telefono, string extension, string empleados, string rapidez, string datos, string videoconferencia, string vozydatos, string actualizar, string area, string presupuesto, string cantidad, string tipo_cliente)
    {
        string result = "";
        string resultado = "";
        DateTime datt = DateTime.Now;
        string fecha = "";
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["calawebConnectionString"].ToString();

        string strSQL = "SELECT CURRENT_TIMESTAMP AS registerDate";
        SqlCommand cmd = new SqlCommand(strSQL, con);
        try
        {
            con.Open();
            datt = (DateTime)cmd.ExecuteScalar();
            fecha = String.Format("{0:MM/dd/yyyy HH:mm:ss}", datt);
            con.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }

        //Se envian los datos a la consulta por parametro y no concatenandolos directamente para evitar inyección de código :D           
        string stmt = "INSERT INTO Cala_Web.Esme_Survey012014 (Nombre, Puesto, Empresa, Email, Telefono, Extension, Register_date, q1, q2, q3, q4,q5,q6,q7,q8,q9,Register_date2,tipo_cliente) VALUES(@Nombre, @Puesto, @Empresa, @Email, @Telefono, @Extension, @Register_date, @q1, @q2, @q3, @q4, @q5, @q6, @q7, @q8, @q9, @Register_date2, @tipo_cliente)";

        SqlCommand cmd2 = new SqlCommand(stmt, con);
        //cmd2.Parameters.Add("@Id", SqlDbType.Int);
        cmd2.Parameters.Add("@Nombre", SqlDbType.VarChar,100);
        cmd2.Parameters.Add("@Puesto", SqlDbType.VarChar, 150);
        cmd2.Parameters.Add("@Empresa", SqlDbType.VarChar, 150);
        cmd2.Parameters.Add("@Email", SqlDbType.VarChar, 100);
        cmd2.Parameters.Add("@Telefono", SqlDbType.VarChar, 30);
        cmd2.Parameters.Add("@Extension", SqlDbType.VarChar, 10);
        cmd2.Parameters.Add("@Register_date", SqlDbType.VarChar, 60);
        cmd2.Parameters.Add("@q1", SqlDbType.VarChar, 50);
        cmd2.Parameters.Add("@q2", SqlDbType.VarChar, 150);
        cmd2.Parameters.Add("@q3", SqlDbType.VarChar, 150);
        cmd2.Parameters.Add("@q4", SqlDbType.VarChar, 150);
        cmd2.Parameters.Add("@q5", SqlDbType.VarChar, 50);
        cmd2.Parameters.Add("@q6", SqlDbType.VarChar, 50);
        cmd2.Parameters.Add("@q7", SqlDbType.VarChar, 50);
        cmd2.Parameters.Add("@q8", SqlDbType.VarChar, 50);
        cmd2.Parameters.Add("@q9", SqlDbType.VarChar, 50);
        cmd2.Parameters.Add("@Register_date2", SqlDbType.DateTime);
        cmd2.Parameters.Add("@tipo_cliente", SqlDbType.VarChar, 20);
        

        //cmd2.Parameters["@Id"].Value = 0;
        cmd2.Parameters["@Nombre"].Value = nombre;
        cmd2.Parameters["@Puesto"].Value = titleContact;
        cmd2.Parameters["@Empresa"].Value = empresa;
        cmd2.Parameters["@Email"].Value = email;
        cmd2.Parameters["@Telefono"].Value = telefono;
        cmd2.Parameters["@Extension"].Value = extension;
        cmd2.Parameters["@Register_date"].Value = fecha;
        cmd2.Parameters["@q1"].Value = empleados;
        cmd2.Parameters["@q2"].Value = rapidez;
        cmd2.Parameters["@q3"].Value = datos;
        cmd2.Parameters["@q4"].Value = videoconferencia;
        cmd2.Parameters["@q5"].Value = vozydatos;
        cmd2.Parameters["@q6"].Value = actualizar;
        cmd2.Parameters["@q7"].Value = area;
        cmd2.Parameters["@q8"].Value = presupuesto;
        cmd2.Parameters["@q9"].Value = cantidad;
        cmd2.Parameters["@Register_date2"].Value = datt;
        cmd2.Parameters["@tipo_cliente"].Value = tipo_cliente;
        

        try
        {
            con.Open();
            cmd2.ExecuteNonQuery();
            con.Close();
            result = "ok";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result = "fail";
        }
        finally
        {
            con.Close();
        }

        if (result == "ok")
        {
            string nombrec = nombre;
            resultado = sendMails(nombrec,  email);
        }
        return resultado;
    }

    public static string sendMails(string nombre, string correo)
    {
        string result = "";
        try
        {
            string contenido = getContenidoMail(nombre);

            string rta_mail = SendMail(correo, "e-marketing@avaya.com", "Avaya Networking Briefing - Confirmation", contenido);

            result = "ok";
        }
        catch (Exception ex)
        {
            result = "false" + ex;
        }
        return result;
    }

    public static string SendMail(string to, string from, string subject, string contenido)
    {
        string respuesta = "";

        MailAddress sendfrom = new MailAddress(from);
        MailAddress sendto = new MailAddress(to);
        MailMessage message = new MailMessage();

        ContentType mimeType = new System.Net.Mime.ContentType("text/html");
        string body = HttpUtility.HtmlDecode(contenido);
        AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
        message.AlternateViews.Add(alternate);

        message.From = new MailAddress(from);
        message.To.Add(to);
        message.Subject = subject;

        SmtpClient client = new SmtpClient("localhost");

        try
        {
            client.Send(message);
            respuesta = "ok";

        }
        catch (SmtpException e)
        {
            respuesta = "fail";
            throw new SmtpException(e.Message);

        }
        return respuesta;
    }

    public static string getContenidoMail(string nombre)
    {
        string plantilla = getPlantilla();

        //Dictionary<string, string> dataIndex = new Dictionary<string, string>();
        //dataIndex.Add("{nombre}", nombre);
        //dataIndex.Add("{evento}", "");

        //string buscar = "";
        //string reemplazar = "";
        //string index = "";
        //Recorrer la plantilla del index para reemplazar el contenido
        //foreach (var datos in dataIndex)
        //{
        //    buscar = datos.Key;
        //    reemplazar = datos.Value;
        //    index = plantilla.Replace(buscar, reemplazar);
        //    plantilla = index;
        //}

        return plantilla;
    }

    public static string getPlantilla()
    {
        string fullPath = HttpContext.Current.Server.MapPath("~");
        string html = "";
        html = File.ReadAllText(fullPath + "\\mx\\events\\movieparinv\\email.html");
       
        return html;
    }

    [WebMethod(EnableSession = true)]
    public static string validarIngresoAdmin(string name, string pass, string app)
    {

        string result = validarIngreso(name, pass, app);
        if (result == "ok")
        {
            var sessionUsuario = HttpContext.Current.Session;
            sessionUsuario["ID"] = name;
        }
        return result;
    }

    public static string validarIngreso(string name, string pass, string app)
    {

        string resultado = "";
        string usuario;
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["calawebConnectionString"].ToString();

        string strSQL = "SELECT distinct usuario from UserData where usuario = @name and password = @pass and application = @app";
        SqlCommand cmd = new SqlCommand(strSQL, con);
        cmd.Parameters.Add("@name", SqlDbType.VarChar, 100);
        cmd.Parameters.Add("@pass", SqlDbType.VarChar, 300);
        cmd.Parameters.Add("@app", SqlDbType.VarChar, 100);
       
        //cmd2.Parameters["@Id"].Value = 0;
        cmd.Parameters["@name"].Value = name;
        cmd.Parameters["@pass"].Value = pass;
        cmd.Parameters["@app"].Value = app;


        try
        {
            con.Open();
            usuario = (String)cmd.ExecuteScalar();
            con.Close();
            if (name == usuario)
            {
                resultado = "ok";
            }
            else
            {
                resultado = "fail";
            }
        }
        catch (Exception ex)
        {
            resultado = "fail";
            Console.WriteLine(ex.Message);
        }
        finally
        {
            con.Close();
        }

        return resultado;
    }

    [WebMethod]
    public static string Convertir(string format, string tipo_usuario)
    {
        string resultado = "error0";
        try
        {
            string path = HttpContext.Current.Server.MapPath("~");
            path = path + "\\mx\\events\\movieparinv";
            if (format == "XLS")
            {
                resultado = toExcel(path,tipo_usuario);
            }
            
            HttpContext.Current.Response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });
        }
        catch (Exception e)
        {
            resultado = e.ToString();
        }
        return resultado;

    }

    public static string getContenido(string tipo_usuario)
    {
        string result = "";
        string resultado = getDatosReg(tipo_usuario);
        string tabla = "";
        var serializer = new JavaScriptSerializer();

        List<Datos> values = serializer.Deserialize<List<Datos>>(resultado);

        foreach (var root in values)
        {
            tabla += "<tr>";
            tabla += "<td>" + root.Nombre + "</td>";
            tabla += "<td>" + root.Email + "</td>";
            tabla += "<td>" + root.Empresa + "</td>";
            tabla += "<td>" + root.Puesto + "</td>";
            tabla += "<td>" + root.Telefono + "</td>";
            tabla += "<td>" + root.Extension + "</td>";
            tabla += "<td>" + root.q1 + "</td>";
            tabla += "<td>" + root.q2 + "</td>";
            tabla += "<td>" + root.q3 + "</td>";
            tabla += "<td>" + root.q4 + "</td>";
            tabla += "<td>" + root.q5 + "</td>";
            tabla += "<td>" + root.q6 + "</td>";
            tabla += "<td>" + root.q7 + "</td>";
            tabla += "<td>" + root.q8 + "</td>";
            tabla += "<td>" + root.q9 + "</td>";
            tabla += "<td>" + root.Register_date + "</td>";
            

            tabla += "</tr>";
        }



        result = tabla;
        return result;
    }

    [WebMethod(EnableSession = true)]
    public static string validaSession()
    {
        string result = "";
        var sessionUsuario = HttpContext.Current.Session;
        if (sessionUsuario["ID"] == null)
        {
            result = "fail";
        }
        else
        {
            result = sessionUsuario["ID"].ToString();
        }
        return result;
    }

    public static string toExcel(string path1, string tipo_usuario)
    {
        string respuesta = "";
        string path = path1 + "\\ExcelFiles\\";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        string nombre = DateTime.Now.ToString("yyyyMMddhhmmss") + "ExcelFiles.xls";
        string fullPath = path1 + "\\ExcelFiles\\" + nombre;
        string contenido = getContenido(tipo_usuario);
        
        string data = "<tr><th width=\"10%\">Nombre</th><th width=\"10%\">Email</th><th width=\"10%\">Empresa</th><th width=\"10%\">Puesto</th><th width=\"10%\">Telefono</th><th width=\"10%\">Extensión</th><th width=\"10%\">Respuesta1</th><th width=\"10%\">Respuesta2</th><th width=\"10%\">Respuesta3</th><th width=\"10%\">Respuesta4</th><th width=\"10%\">Respuesta 5</th><th width=\"10%\">Respuesta 6</th><th width=\"10%\">Respueta 7</th><th width=\"10%\">Respuesta 8</th><th width=\"10%\">Respuesta 9</th><th width=\"10%\">Fecha Registro</th></tr>";
        contenido = data + contenido;
        contenido = "<table border = '1' style=" + '"' + "font-family: Verdana,Arial,sans-serif; font-size: 12px;" + '"' + ">" + contenido + "</table></body></html>";

        try
        {
            FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write);


            using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
            {
                writer.Write(contenido);
            }
        }
        catch
        {
            respuesta = "fail";
        }
        finally
        {
            respuesta = nombre;
        }
        return respuesta;
    }
}

