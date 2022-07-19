using Microsoft.AspNetCore.Authorization;

Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();