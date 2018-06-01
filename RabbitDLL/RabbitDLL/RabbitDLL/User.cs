using System;

namespace RabbitDLL
{
    public class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string LastToken { get; set; }
    }
}
