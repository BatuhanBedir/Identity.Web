namespace Identity.Web.Services;

public interface IEmailService
{
    Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);
}