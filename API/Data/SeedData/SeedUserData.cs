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

namespace API.Data.SeedData
{
    public class SeedUserData
    {
        public static async Task SeedUsers(DataContext context, ILoggerFactory logger)
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
                        using var hmac = new HMACSHA512();
                        user.UserName = user.UserName.ToLower();
                        user.PasswordSalt = hmac.Key;
                        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                    }
                    await context.Users.AddRangeAsync(users);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var log = logger.CreateLogger<SeedUserData>();
                log.LogError(ex.Message);
            }
        }
    }
}