using System;

namespace DataStore
{
    public class Message
    {
        public int messageID { get; set; }
        public int userID { get; set; }
        public string message { get; set; }

        public DateTime MessageTimeStamp { get; set; }
    }
}