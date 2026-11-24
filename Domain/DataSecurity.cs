namespace Domain;

public class DataSecurity
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}