//using Microsoft.AspNetCore.Identity;
//using ToDo.Authentication_Models;

//namespace ToDo.Data
//{
//    public class ContextSeed
//    {
//        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            // Seed Roles if they do not exist
//            if (!await roleManager.RoleExistsAsync(Enums.Roles.Admin.ToString()))
//            {
//                await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
//            }
//            if (!await roleManager.RoleExistsAsync(Enums.Roles.User.ToString()))
//            {
//                await roleManager.CreateAsync(new IdentityRole(Enums.Roles.User.ToString()));
//            }
//        }
//        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            // Seed Default User
//            var defaultUser = new ApplicationUser
//            {
//                UserName = "admin",
//                Email = "admin@hotmail.com",
//                EmailConfirmed = true,
//            };
//            string password = "123Admin-";

//            // Attempt to find the user by email address
//            var existingUser = await userManager.FindByEmailAsync(defaultUser.Email);

//            // If no user with the specified email exists, create a new user
//            if (existingUser == null)
//            {
//                var result = await userManager.CreateAsync(defaultUser, password);

//                // Check if user creation was successful
//                if (result.Succeeded)
//                {
//                    // Check if roles exist before adding
//                    foreach (var roleName in Enums.RoleManager.GetAllRoles())
//                    {
//                        if (!await roleManager.RoleExistsAsync(roleName))
//                        {
//                            await roleManager.CreateAsync(new IdentityRole(roleName));
//                        }
//                    }

//                    // Add user to roles
//                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.User.ToString());
//                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
//                }
//            }
//            else
//            {
//                // User already exists with the specified email
//                // Check if the user already has the roles assigned
//                var userRoles = await userManager.GetRolesAsync(existingUser);
//                var rolesToAdd = Enums.RoleManager.GetAllRoles().Except(userRoles);

//                // Add user to roles if roles are not already assigned
//                foreach (var role in rolesToAdd)
//                {
//                    await userManager.AddToRoleAsync(existingUser, role);
//                }
//            }
//        }


//    }
//}
