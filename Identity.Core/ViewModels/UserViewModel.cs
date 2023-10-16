namespace Identity.Web.ViewModels;

public record UserViewModel
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PictureUrl { get; set; }
}