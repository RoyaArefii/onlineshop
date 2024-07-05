namespace OnlineShop.BackOffice.WebApiEndpoint.ControllerDtos.UserManagementDtos.RoleControllerDtos
{
    public class PutRoleControllerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string? EntityDescription { get; set; }

    }

}
