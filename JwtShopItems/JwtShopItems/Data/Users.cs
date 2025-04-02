using JwtShopItems.Models;

namespace JwtShopItems.Data
{
    public static class Users
    {
        public static List<User> GetUsers()
        {
            return new List<User>()
            {
                new User() { UserName = "Someone", Password = "passw", Email = "took@gmail.com" },
                new User() { UserName = "Honcharova", Password = "adminPassw", Email = "huy@gmail.com" }
            };
        }
    }
}
