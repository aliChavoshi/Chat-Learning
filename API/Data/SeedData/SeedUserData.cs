using System.Security.Cryptography;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using API.Entities;
using System.Text;
using Microsoft.Extensions.Logging;
using API.interfaces;
using Microsoft.AspNetCore.Identity;

namespace API.Data.SeedData
{
    public class SeedUserData
    {
        public static async Task SeedUsers(DataContext context, UserManager<Users> userManager)
        {
            try
            {
                if (!await context.Users.AnyAsync())
                {
                    var userData = await File.ReadAllTextAsync("Data/SeedData/UserSeedData.json");
                    var users = JsonSerializer.Deserialize<List<Users>>(userData);
                    if (users == null) return;
                    foreach (var user in users)
                    {
                        await userManager.CreateAsync(user, "P@$$w0rd");
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}