namespace ABC_Hospital_Web_Service.Models
{
    public class KeyTokenPair
    {
        public string Key { get; set; }
        public string Token { get; set; }

        public KeyTokenPair(string key = "", string token = "")
        {
            Key = key;
            Token = token;
        }
    }
}
