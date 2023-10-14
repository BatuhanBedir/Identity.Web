namespace Identity.Web.Areas.Admin.Models;

public record RoleViewModel
{
    public string Id { get; set; }
    public string Name { get; set; } = null!;
}