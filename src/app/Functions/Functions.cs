using System;
using GTDPad.DTO;
using GTDPad.Support;
using Microsoft.AspNetCore.Mvc;
using static GTDPad.Constants;

namespace GTDPad
{
    public static class Functions
    {
        public static T WithID<T>(this T dto, Guid id)
            where T : DTOBase<T>
        {
            Guard.AgainstNull(dto, nameof(dto));

            dto.ID = id;

            return dto;
        }

        public static bool IsNonEmptyAndLocal(this IUrlHelper urlHelper, Uri uri) =>
            uri is object && urlHelper is object && urlHelper.IsLocalUrl(uri.ToString());

#pragma warning disable CA1055 // Uri return values should not be strings
        public static string GetUrl(string controllerName, string actionName)
        {
            Guard.AgainstNull(controllerName, nameof(controllerName));
            Guard.AgainstNull(actionName, nameof(actionName));

            var controllerRoute = controllerName.Replace(ControllerSuffix, string.Empty, StringComparison.OrdinalIgnoreCase);

            return $"/{controllerRoute}/{actionName}";
        }
#pragma warning restore CA1055 // Uri return values should not be strings
    }
}
