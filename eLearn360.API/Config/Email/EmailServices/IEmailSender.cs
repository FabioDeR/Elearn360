using System.Threading.Tasks;

namespace elearn360.API.Config.Email.EmailServices
{
    public interface IEmailSender
    {
       
        Task SendMailConfirmation(Message message, string password);
        Task SendMailOrganizationAssociation(Message message);
        Task SendMailPasswordChange(Message message);
        Task SendMailPasswordReset(Message message);
    }
}
