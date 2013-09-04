// --------------------------------------------------------------------------
// <copyright file="PermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Core.Services.Impl
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Ares.Core.Web;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;

    /// <summary>
    /// Permission Service.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Permission Service.")]
    public class PermissionService
    {
        private static readonly IUserService UserService;

        static PermissionService()
        {
            UserService = new UserService();
        }

        /// <summary>
        /// GetRollAccess: get permission to access with each menu item of current user login.
        /// </summary>
        /// <param name="currentFunctions">The function name.</param>
        /// <returns>Return result.</returns>
        public static Dictionary<string, int> GetRoleAccess(IEnumerable<FunctionPolicy> currentFunctions)
        {
            Dictionary<string, int> roleAccess = new Dictionary<string, int>();

            foreach (FunctionPolicy functionPolicy in currentFunctions)
            {
                if (functionPolicy.Policy >= (int)Constant.Policy.Read)
                {
                    roleAccess.Add(functionPolicy.Function.Key, functionPolicy.Policy);
                }
            }

            return roleAccess;
        }

        /// <summary>
        /// HasReadPermission: check that user has permission to access one menu link.
        /// </summary>
        /// <param name="functionName">The function name.</param>
        /// <returns>Return result.</returns>
        public static bool HasReadPermission(string functionName)
        {
            return AppSession.RoleAccess != null &&
                   AppSession.RoleAccess.ContainsKey(functionName) &&
                   AppSession.RoleAccess[functionName] == (int)Constant.Policy.Read;
        }

        /// <summary>
        /// Checking has hot permission.
        /// </summary>
        /// <param name="functionName">The function name.</param>
        /// <returns>Return result.</returns>
        public static bool HasHostPermission(string functionName)
        {
            return AppSession.RoleAccess != null &&
                   AppSession.RoleAccess.ContainsKey(functionName) &&
                   AppSession.RoleAccess[functionName] == (int)Constant.Policy.Host;
        }

        /// <summary>
        /// Checking read permission.
        /// </summary>
        /// <param name="functionName">The function name.</param>
        /// <returns>Return result.</returns>
        public static bool HasPermission(string functionName)
        {
            return AppSession.RoleAccess != null &&
                   AppSession.RoleAccess.ContainsKey(functionName) &&
                   AppSession.RoleAccess[functionName] >= (int)Constant.Policy.Read;
        }
    }
}
