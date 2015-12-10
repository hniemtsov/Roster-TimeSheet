﻿using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace RestHomes.Infrastructure
{
    public class AppRoleManager : RoleManager<IdentityRole>, IDisposable
    {
        public AppRoleManager(RoleStore<IdentityRole> store)
            : base(store)
        {
        }
        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options,
            IOwinContext context)
        {
            return new AppRoleManager(new RoleStore<IdentityRole>(context.Get<AppIdentityDbContext>()));
        }
    }
}