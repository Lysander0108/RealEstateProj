namespace RealEstateProj.Data
{

    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PasswordHashed { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        AGENT,
        USER
    }
}