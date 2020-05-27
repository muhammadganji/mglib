using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library1.Models
{
    public class SeedData
    {
        public static void Seeding(ApplicationDbContext context)
        {
            // 0. create database
            // 1. role
            // 2. user
            // step: 0


            // step: 1
            if (!context.Roles.Any()) // اگر در جدول رول هیچ سطری نبود
            {
                #region command
                //context.Roles.AddRange(
                //    new ApplicationRole
                //    {
                //        Id = "b76cdd25-c852-4afa-995d-5b73bc7d6109",
                //        ConcurrencyStamp = "e63943b6-5eeb-4d32-8320-22b40200527f",
                //        Description = "sysadmin",
                //        Name = "admin",
                //        NormalizedName = "ADMIN"
                //    });
                //context.SaveChanges();

                //context.Roles.AddRange(
                //    new ApplicationRole
                //    {
                //        Id = "5e8e57de-2b3c-48ab-9d4f-f33facdb8eed",
                //        ConcurrencyStamp = "d404344e-4855-4e72-9743-704f7c618e5e",
                //        Description = "regular user",
                //        Name = "user",
                //        NormalizedName = "USER"
                //    });
                //context.SaveChanges();
                #endregion

                ApplicationRole rolAdmin = new ApplicationRole
                {
                    Id = "b76cdd25-c852-4afa-995d-5b73bc7d6109",
                    ConcurrencyStamp = "e63943b6-5eeb-4d32-8320-22b40200527f",
                    Description = "sysadmin",
                    Name = "admin",
                    NormalizedName = "ADMIN"
                };
                context.Roles.Add(rolAdmin);
                context.SaveChanges();

                ApplicationRole rolUser = new ApplicationRole
                {
                    Id = "5e8e57de-2b3c-48ab-9d4f-f33facdb8eed",
                    ConcurrencyStamp = "d404344e-4855-4e72-9743-704f7c618e5e",
                    Description = "regular user",
                    Name = "user",
                    NormalizedName = "USER"
                };
                context.Roles.Add(rolUser);
                context.SaveChanges();

                // step: 2
                if (!context.Users.Any())
                {
                    ApplicationUser admin = new ApplicationUser
                    {
                        //Id = "16dbb2ed-0a86-499a-9296-2d0efa1def54",
                        FirstName = "adminSys",
                        LastName = "system",
                        Email = "mail@mail.com",
                        NormalizedEmail = "MAIL@MAIL.COM",
                        PhoneNumber = "0000000000",
                        LockoutEnabled = true,
                        UserName = "admin",
                        NormalizedUserName = "ADMIN",
                        // password : **************
                        PasswordHash = "AQAAAAEAACcQAAAAENgFrB71A/cyVJ+9bbacZbl/B5m8n7ZUz58PLrwq2t/IuZ6UAfjDMbyfAhQWhvQN9Q==",
                        ConcurrencyStamp = "e81029bb-1f0e-4e7a-af39-3e8addb66122",
                        SecurityStamp = "VBGSDPHC64HP7IFCRGGBYPMDSCCTIY7Y"
                    };
                    context.Users.Add(admin);
                    context.SaveChanges();

                    if (!context.UserRoles.Any())
                    {
                        IdentityUserRole<string> ur = new IdentityUserRole<string>();
                        ur.RoleId = "b76cdd25-c852-4afa-995d-5b73bc7d6109";
                        ur.UserId = admin.Id;

                        context.UserRoles.Add(ur);
                        context.SaveChanges();
                    }
                }
            }

        }
    }
}
