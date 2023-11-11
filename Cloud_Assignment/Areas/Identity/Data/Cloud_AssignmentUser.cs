using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Cloud_Assignment.Areas.Identity.Data;

// Add profile data for application users by adding properties to the Cloud_AssignmentUser class
public class Cloud_AssignmentUser : IdentityUser
{
    [PersonalData]
    public string? UserFullName { get; set; }
    [PersonalData]
    public int UserAge { get; set; }
    [PersonalData]
    public string? UserAddress { get; set; }
    [PersonalData]
    public DateTime UserDOB { get; set; }

    [PersonalData]
    public string? UserRole { get; set; }

}

