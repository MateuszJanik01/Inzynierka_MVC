﻿using Microsoft.AspNetCore.Identity;

namespace Fences.Model.DataModels
{
    public class Role : IdentityRole<int>
    {
        public RoleValue RoleValue { get; set; }
        public Role(string name, RoleValue roleValue) { }
        public Role() { }
    }
}
