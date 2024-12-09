using GSI.WebApi.AD;
using System.Data.SqlTypes;
using System.DirectoryServices;

namespace GSI_WebApi.AD
{
    public class AdHelper
    {
        public static List<AdUser> GetUsers()
        {
            using (var root = new DirectoryEntry($"LDAP://192.168.1.229"))
            {
                root.Username = "Administrator";
                root.Password = "teste123!\"#";

                using (var searcher = new DirectorySearcher(root))
                {
                    searcher.Filter = $"(&(objectCategory=person)(objectClass=user))";
                    SearchResultCollection results = searcher.FindAll();

                    List<AdUser> users = new List<AdUser>();

                    foreach (SearchResult result in results)
                    {
                        AdUser user = new AdUser();
                        user.Id = new Guid((byte[])result.Properties["objectGUID"][0]).ToString();
                        user.Name = result.Properties["name"][0].ToString();
                        user.AccountExpires = GetDate((long)result.Properties["accountExpires"][0]);
                        user.IsEnabled = IsAccountEnabled((int)result.Properties["userAccountControl"][0]);
                        user.CreationDate = (DateTime)result.Properties["whenCreated"][0];
                        user.LastPasswordChange = GetDate((long)result.Properties["pwdLastSet"][0]);
                        user.NLogins = (int)result.Properties["logonCount"][0];
                        user.Department = GetOU(result.Properties["distinguishedName"][0].ToString());

                        users.Add(user);
                    }

                    return users;
                }
            }
        }

        private static bool IsAccountEnabled(int value)
        {
            return !Convert.ToBoolean(value & 0x0002);
        }

        private static DateTime? GetDate(long value)
        {
            try
            {
                DateTime dt = DateTime.FromFileTime(value);
                return dt < SqlDateTime.MinValue ? null : dt;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string GetOU(string value)
        {
            string ou = value.Split(",").ToList().FirstOrDefault(x => x.StartsWith("OU="));
            return string.IsNullOrEmpty(ou) ? null : ou.Split("=")[1];
        }
    }
}
