using System.Security.Cryptography.X509Certificates;

namespace API.Data.DTO
{
    public static class CoachDTO
    {
        private static readonly List<string> attributesToGet = [
                    "Id",
                    "firstName",
                    "middleName",
                    "lastName",
                    "emailAddress",
                    "roles",
                    "scopes",
                    "phoneNumber",
                    "profileImageURL",
                    "aboutMe",
                    "closestWorkAddress"
                        ];
        public static List<string> AttributesToGet { get => attributesToGet; }
    }
}