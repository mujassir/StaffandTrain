using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Mail;

using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

/// <summary>
/// Summary description for SendEmail
/// </summary>
namespace StaffandTrain.Common
{
    public class SendEmail
    { 
        private readonly SmtpClient _smtpClient;
        //---==== SendEmail Constructor ====----
        public SendEmail(string sectionName = "default")
        {
            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("mailSettings/" + sectionName);

            _smtpClient = new SmtpClient();

            if (section != null)
            {
                if (section.Network != null)
                {
                    _smtpClient.Host = section.Network.Host;
                    _smtpClient.Port = section.Network.Port;
                    _smtpClient.UseDefaultCredentials = section.Network.DefaultCredentials;
                    _smtpClient.Credentials = new NetworkCredential(section.Network.UserName, section.Network.Password, section.Network.ClientDomain);
                    _smtpClient.EnableSsl = false;
                    if (section.Network.TargetName != null)
                        _smtpClient.TargetName = section.Network.TargetName;
                }

                _smtpClient.DeliveryMethod = section.DeliveryMethod;
                if (section.SpecifiedPickupDirectory != null && section.SpecifiedPickupDirectory.PickupDirectoryLocation != null)
                    _smtpClient.PickupDirectoryLocation = section.SpecifiedPickupDirectory.PickupDirectoryLocation;
            }
        }


        public bool SendToEmail(string Subject, string Message, string EmailTo)
        {
            bool emailResult = false;
            string emailSubject = Subject;

            string emailtemplate = Message;
            try
            {
                var HostName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                emailtemplate = emailtemplate.Replace("[HostName]", HostName);
                emailtemplate = emailtemplate.Replace("[TodayDate]", DateTime.Now.ToString("dd MMM yyyy"));
                emailtemplate = emailtemplate.Replace("[CurrentTime]", DateTime.Now.ToString("HH:mm:ss tt"));

                // Replace Token by Dictionary key and values

                MailMessage oEmail = new MailMessage();
                oEmail.From = new MailAddress("dj@nearshore-staffing.com");
                oEmail.Subject = emailSubject;
                oEmail.Body = emailtemplate;
                oEmail.IsBodyHtml = true;
                oEmail.To.Add(EmailTo); //-- add To email
                
    //            ServicePointManager.ServerCertificateValidationCallback =
    //delegate (object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
    //         X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //{ return true; };

                _smtpClient.Send(oEmail);
                emailResult = true;
            }
            catch (Exception ex)
            {
               // Common cm = new Common();
               // cm.ErrorExceptionLogingByService(ex.ToString(), "ForgetPassword" + ":" + new StackTrace().GetFrame(0).GetMethod().Name, "ForgetPassword", "NA", "NA", "NA", "WEB");
                //CPPLException objExc = new CPPLException();
                //objExc.CPPLErrorExceptionLogingByService(ex.ToString(), GetType().Name + ":" + new StackTrace().GetFrame(0).GetMethod().Name, GetType().Name, "", emailType, emailtemplate);
                //emailResult = false;
            }
            return emailResult;
        }


    }

}