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
using System.Security.Cryptography;


public partial class register : System.Web.UI.Page
{



    protected void Page_Load(object sender, EventArgs e)
    {

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

    public static string validarIngreso(string username, string pass, string app)
    {

        string resultado = "";
        string usuario;
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["calawebConnectionString"].ToString();

        string strSQL = "SELECT distinct username from Tbl_ResponseTool_Users where username = @username and password = @password ";
        SqlCommand cmd = new SqlCommand(strSQL, con);
        cmd.Parameters.Add("@username", SqlDbType.VarChar, 50);
        cmd.Parameters.Add("@password", SqlDbType.VarChar, 100);
       
        cmd.Parameters["@username"].Value = username;
        cmd.Parameters["@password"].Value = pass;

        try
        {
            con.Open();
            usuario = (String) cmd.ExecuteScalar();
            con.Close();
            if (username == usuario)
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

      
    public static string CreatePassword(int length)
    {
        string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        string res = "";
        Random rnd = new Random();
        while (0 < length--)
            res += valid[rnd.Next(valid.Length)];
        return res;
    }

    public static string sha256(string password)
    {
        SHA256Managed crypt = new SHA256Managed();
        string hash = String.Empty;
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
        foreach (byte bit in crypto)
        {
            hash += bit.ToString("x2");
        }
        return hash;
    }

    public static string validarEmail(string email) {
        string resultado = "";
        string usuario = "";
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["calawebConnectionString"].ToString();

        string strSQL = "SELECT distinct email_empresa from UserData where email_empresa = @email and application = 'sales_tool'";
        SqlCommand cmd = new SqlCommand(strSQL, con);
        cmd.Parameters.Add("@email", SqlDbType.VarChar, 150);
        cmd.Parameters["@email"].Value = email;
       

        try
        {
            con.Open();
            usuario = (String)cmd.ExecuteScalar();
            con.Close();
            if (email == usuario)
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
    public static string recoveryPassword(string email)
    {

        string resultado = "";
        string pass = CreatePassword(8);      //Para enviar al usuario en el correo
        string passEncript = sha256(pass);    //Para guardar en la base de datos


        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["calawebConnectionString"].ToString();
        if (validarEmail(email) == "ok")
        {
            string strSQL = "update UserData set password = @pass where email_empresa = @email and application = 'sales_tool'";
            SqlCommand cmd = new SqlCommand(strSQL, con);
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 150);
            cmd.Parameters.Add("@pass", SqlDbType.VarChar, 300);
            
            cmd.Parameters["@email"].Value = email;
            cmd.Parameters["@pass"].Value = passEncript;
            
            try
            {
                con.Open();
                cmd.ExecuteScalar();
                con.Close();
                resultado = "ok";
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
        }
        else {
            resultado = "fail";
        }

        if (resultado == "ok") {
            sendMails(email, pass);
        }
        return resultado;
    }

    public static string sendMails(string email, string pass)
    {
        string result = "";
        string title = "Avaya Response Tool - Recovery Password";
        
        string data = "<table width=\"500\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" align=\"left\" style=\"font-family: Arial, Helvetica, sans-serif; font-size: 12px;margin-top:14pt;margin-bottom:14pt;\"><tr><td style=\"color:#cc0000;\">Email User:</td><td>" + email + "</td></tr><tr><td style=\"color:#cc0000;\">New Password: </td><td>" + pass + "</td></tr></table>";
        try
        {
            string rta_mail = SendMail(email, "e-marketing@avaya.com", title, data);
            result = rta_mail;
        }
        catch (Exception ex)
        {
            result = "false" + ex.Message;
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
            respuesta = "fail" + e.Message;
            throw new SmtpException(e.Message);

        }
        return respuesta;
    }


}

