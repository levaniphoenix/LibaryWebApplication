using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.ViewModels
{
    public class EditUserRoleModel
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public List<string> roles { get; set; }
        public List<string> userRoles { get; set; }
    }
}
