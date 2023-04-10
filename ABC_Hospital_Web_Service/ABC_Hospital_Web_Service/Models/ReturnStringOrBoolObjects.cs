namespace ABC_Hospital_Web_Service.Models
{
    public class ReturnStringObject
    {
        public string str { get; set; }

        public ReturnStringObject(string _str = "")
        {
            str = _str;
        }
    }

    public class ReturnBoolObject
    {
        public bool success { get; set; }

        public ReturnBoolObject(bool _success = false)
        {
            success = _success;
        }
    }
}
