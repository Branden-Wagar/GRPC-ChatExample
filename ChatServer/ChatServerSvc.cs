using ChatServerProto;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ChatServerSvc : ChatServerProto.ChatServerService.ChatServerServiceBase
    {
        public ChatServerSvc()
        {
        }
        
        public override Task<ResponseCode> PostMessage(ChatMessage msg, ServerCallContext context)
        {
            Console.WriteLine(msg.Msg);
            DataStore.MessageBoardDBContext db = new DataStore.MessageBoardDBContext();
            db.MessageBoard.Add(DBMessageConversion(msg));
            db.SaveChanges();
            db.Dispose();
            return Task.FromResult(new ResponseCode() { Code = 200 });
        }

        public override async Task GetMessages(GetMessageRequest request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            var db = new DataStore.MessageBoardDBContext();
            foreach(var msg in db.MessageBoard)
            {
                await responseStream.WriteAsync(DBMessageConversion(msg));
            }
            db.Dispose();
        }

        private DataStore.Message DBMessageConversion(ChatMessage msg)
        {
            return new DataStore.Message()
            {
                message = msg.Msg,
                userID = msg.UserID,
                MessageTimeStamp = msg.PostTime.ToDateTime()
            };
        }

        private ChatMessage DBMessageConversion(DataStore.Message m)
        {
            return new ChatMessage()
            {
                Msg = m.message,
                UserID = m.userID,
                PostTime = m.MessageTimeStamp.ToUniversalTime().ToTimestamp()
            };
        }

    }
}
