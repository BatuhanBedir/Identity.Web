namespace Identity.Web.Areas.Admin.Models;

public record AssignRoleToUserViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool Exist { get; set; }
}