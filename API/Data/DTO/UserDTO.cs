using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.DTO
{
    public static class UserDTO
    {
        private static readonly List<string> attributesToGet = [
                    "Id",
                    "firstName",
                    "middleName",
                    "lastName",
                    "emailAddress",
                    "roles",
                    "scopes",
                    "phoneNumber"
                    ];
        public static List<string> AttributesToGet { get => attributesToGet; }
    }
}