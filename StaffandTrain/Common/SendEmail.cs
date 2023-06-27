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
using StaffandTrain.DataModel;
using DocumentFormat.OpenXml.Drawing.Diagrams;

/// <summary>
/// Summary description for SendEmail
/// </summary>
namespace StaffandTrain.Common
{
    public class SendEmail
    {
        SATConn context = new SATConn();

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

        public static void SendSMTPEmail(string recipientEmail, string subject, string body)
        {
            // Email parameters
            string senderEmail = "goodmorning@nearshore-usa.com";
            string senderPassword = "Ideas321!";

            // SMTP server details
            string smtpHost = "mail.nearshore-usa.com";
            int smtpPort = 2525; // Port number for the SMTP server
            bool enableSsl = false; // Set it to true if your SMTP server requires SSL/TLS

            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(senderEmail, "Nearshore USA Morning App");
                    mailMessage.To.Add(recipientEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtpClient.EnableSsl = enableSsl;
                        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtpClient.Send(mailMessage);
                    }

                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    
        public void ScheduleWorkerEmail()
        {
            // Email those workers which 15 minutes are remaining to login
            DateTime currentTime = DateTime.Now;
            DateTime endTime = currentTime.AddMinutes(15); // Update 15 value as per required minutes

            var workers = context.Workers.Where(e => e.CheckIn > currentTime.TimeOfDay && e.CheckIn <= endTime.TimeOfDay).ToList();
            if (workers.Count() > 0)
            {
                foreach (var worker in workers)
                {
                    var subject = "Quick Reminder: 15 Minutes Left to Log In!"; // Update 15 value as per required minutes
                    var body = "Please log in at your designated time.";
                    SendSMTPEmail(worker.Email, subject, body);
                }
            }

            

            // Email those workers which are not login
            DateTime currentTime1= currentTime.AddMinutes(-15);
            DateTime endTime1 = DateTime.Now;

            var workers1 = context.Workers.Where(e => e.CheckIn > currentTime1.TimeOfDay && e.CheckIn <= endTime1.TimeOfDay).ToList();
            if (workers1.Count() > 0)
            {
                var workersName = "";
                foreach (var worker in workers1)
                {
                    if (workersName != "") workersName += ", ";
                    workersName += worker.Name;

                    var subject = "Quick Reminder: Your Log In time expired!";
                    var body = "Your Log In time expired!";
                    SendSMTPEmail(worker.Email, subject, body);
                }

                if (workersName != "")
                {
                    var subject = "Reminder: Not Logged Workers!";
                    var body = workersName + " Workers are not logged In on time.";
                    SendSMTPEmail("Admin@email.com", subject, body);
                }
            }
        }
    }

}