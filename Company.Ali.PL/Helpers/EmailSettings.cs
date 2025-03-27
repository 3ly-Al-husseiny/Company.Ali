using System.Net;
using System.Net.Mail;

namespace Company.Ali.PL.Helpers
{
    public class EmailSettings
    {
        public static bool SendEmail(Email email) 
        {
            // Mail Sending 
            // Mail Server : Gmail 
            // SMTP : Simple Mail Protocol 

            
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587); // Host , Port 
                client.EnableSsl = true;
                //eahlqxgratzyzlcb
                client.Credentials = new NetworkCredential("ali.ahmed.software.engineer@gmail.com", "eahlqxgratzyzlcb"); // Sender
                client.Send("ali.ahmed.software.engineer@gmail.com", email.To, email.Subject, email.Body);
            }

            catch (Exception e)
            {
                return false;
            }
            
            return true;
        }
    }
}
