namespace ToDo.Data.Enums
{
    public enum Roles
    {
        Admin,
        User
    }

    public static class RoleManager
    {
        public static IEnumerable<string> GetAllRoles()
        {
            yield return Roles.Admin.ToString();
            yield return Roles.User.ToString();
        }
    }
}
