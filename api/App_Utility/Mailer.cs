using PostmarkDotNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace api.App_Utility
{
    public class Mailer
    {

        public static async Task sendForgotPasswordPostmark(string user_email, string password)
        {
            try
            {
                var postmark = new PostmarkClient(ConfigurationManager.AppSettings["PostMark.ApiKey"]);

                var message = new PostmarkMessage
                {
                    To = user_email,
                    From = ConfigurationManager.AppSettings["PostMark.From"], // This must be a verified sender signature
                    Subject = "Forgot Password",
                    HtmlBody = @"  <b>Forgot password,</b>
                            <p>
                               Your Temporary Password is
                               <b>" + password + @"</b>
                            </p>
                        "
                };

                var response = await postmark.SendMessageAsync(message);

            }
            catch (Exception ex)
            {
                string c = ex.Message;
                ErrorLogs error = new ErrorLogs();
                error.createLogs(ex);
            }

        }

        public static void sendForgotPassword(string user_email, string password)
        {
            try
            {

                MailMessage mailMessage = new MailMessage(new MailAddress(ConfigurationManager.AppSettings["email"], ConfigurationManager.AppSettings["sender"]), new MailAddress(user_email));
                mailMessage.Subject = "Forgot Password";
                mailMessage.Body = EmailLayout("Forgot password", "Your Temporary Password is", password);
                mailMessage.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["Host"];
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = ConfigurationManager.AppSettings["username"];
                NetworkCred.Password = ConfigurationManager.AppSettings["password"];
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = NetworkCred;
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["Port"]);
                smtp.Send(mailMessage);


            }
            catch (Exception ex)
            {
                string c = ex.Message;
                ErrorLogs error = new ErrorLogs();
                error.createLogs(ex);
            }

        }

        public static string EmailLayout(string Title, string Content, string Variable)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Mail/Email.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Title}", Title);
            body = body.Replace("{Content}", Content);
            body = body.Replace("{Variable}", Variable);

            return body;
        }
    }
}