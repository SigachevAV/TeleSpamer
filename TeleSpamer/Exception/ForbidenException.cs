namespace TeleSpamer
{
    internal class ForbidenException : Exception
    {
        public ForbidenException() { }  
        
        public ForbidenException(string message) : base(message) { }
    }
}
