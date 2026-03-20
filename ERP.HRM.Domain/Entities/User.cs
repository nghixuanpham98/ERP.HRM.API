using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ERP.HRM.API;

public partial class User : IdentityUser<Guid>
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
