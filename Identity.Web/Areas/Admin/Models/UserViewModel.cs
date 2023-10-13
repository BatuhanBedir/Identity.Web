namespace Identity.Web.Areas.Admin.Models;

public record UserViewModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}