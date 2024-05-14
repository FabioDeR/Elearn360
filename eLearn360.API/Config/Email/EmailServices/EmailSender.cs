using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elearn360.API.Config.Email.EmailServices
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }


        #region Confirmation MailRegister
        public async Task SendMailConfirmation(Message message,string password)
        {
            try
            {
                var mailMessage = CreateEmailConfirmationMessage(message,password);

                await SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        private MimeMessage CreateEmailConfirmationMessage(Message message,string password)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(Encoding.UTF8, "EducAssist", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format($"<p>Bonjour," +
                $"                                                         </p><p>Votre compte vient d'être créé sur la plateforme eLearn360." +
                $"                                                         </p><p>Veuillez confirmer votre email en cliquant sur le lien suivant:" +
                                                                           $"<p>Voici vos identifiant :"+
                                                                           $"<p>Email :{message.To.First().Address}</p>"+
                                                                           $"<p>Mot de passe : <strong> {password} </strong></p>"+
                $"                                                         </p><p><a href={message.Content}>Cliquez sur le lien</a></p><p>Merci !</p>") };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
        #endregion

        #region Email Organization Associate


        public async Task SendMailOrganizationAssociation(Message message)
        {
            try
            {
                var mailMessage = CreatEmailOrganizationAssociation(message);
                await SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        private MimeMessage CreatEmailOrganizationAssociation(Message message)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(Encoding.UTF8, "EducAssist", _emailConfig.From));
                emailMessage.To.AddRange(message.To);
                emailMessage.Subject = message.Subject;
                var bodyBuilder = new BodyBuilder { HtmlBody = string.Format($"<p>Bonjour," +
                    $"                                                         </p>" +
                    $"                                                         <p>Votre compte vient d'être associé sur la plateforme eLearn360.</p>" +
                    $"                                                         <p>Bonne étude.</p>" +
                    $"                                                         <p>L'équipe de {message.Content}</p>"
                                                                            ) };
                if (message.Attachments != null && message.Attachments.Any())
                {
                    byte[] fileBytes;
                    foreach (var attachment in message.Attachments)
                    {
                        using (var ms = new MemoryStream())
                        {
                            attachment.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                    }
                }

                emailMessage.Body = bodyBuilder.ToMessageBody();
                return emailMessage;               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion

        #region Organization has Been Added

        #endregion

        #region Sennd Email
        private async Task SendAsync(MimeMessage mailMessage)
        {
           
            using (var client = new SmtpClient())
            {
                try
                {                    
                    client.CheckCertificateRevocation = false;
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");                    
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password); 
                    


                 
                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
        #endregion

        #region Confirmation Mail PasswordChange
        public async Task SendMailPasswordChange(Message message)
        {
            try
            {
                var mailMessage = CreateChangePasswordConfirmationMessage(message);

                await SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        private MimeMessage CreateChangePasswordConfirmationMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(Encoding.UTF8, "EducAssist", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder 
            { HtmlBody = string.Format($"<p>Bonjour,</p><p>Nous vous notifions que votre passe passe vient d'être changé.</p>") };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
        #endregion

        #region Create Mail PasswordReset
        public async Task SendMailPasswordReset(Message message)
        {
            try
            {
                var mailMessage = CreateResetPasswordConfirmationMessage(message);

                await SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        private MimeMessage CreateResetPasswordConfirmationMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(Encoding.UTF8, "EducAssist", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder
            { HtmlBody = string.Format($"<p> Bonjour,</p><p>Vous avez fait une demande de réinitialisation de mot de passe.</p> " +
                                       $"<p> Votre nouveau mot de passe est : <b>{message.Content.ToString()} </b> </p>" +
                                       $"<p> Une fois connecté(e), nous vous conseillons de changer de mot de passe </p>") };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }
        #endregion




    }
}
