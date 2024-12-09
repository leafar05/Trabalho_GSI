namespace GSI.WebApi.AD
{
    public class AdUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? AccountExpires { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastPasswordChange { get; set; }
        public int NLogins { get; set; }
        public string Department { get; set; }
    }
}
