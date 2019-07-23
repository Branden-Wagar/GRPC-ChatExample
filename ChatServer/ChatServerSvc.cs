using ChatServerProto;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ChatServerSvc : ChatServerProto.ChatServerService.ChatServerServiceBase, IDisposable
    {
        private DataStore.MessageBoardDBContext _db;
        public ChatServerSvc()
        {
            _db = new DataStore.MessageBoardDBContext();
        }
        
        public override Task<ResponseCode> PostMessage(ChatMessage msg, ServerCallContext context)
        {
            Console.WriteLine(msg.Msg);
            _db.MessageBoard.Add(DBMessageConversion(msg));
            _db.SaveChanges();
            return Task.FromResult(new ResponseCode() { Code = 200 });
        }

        public override async Task GetMessages(GetMessageRequest request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            foreach(var msg in _db.MessageBoard)
            {
                await responseStream.WriteAsync(DBMessageConversion(msg));
            }
        }

        private DataStore.Message DBMessageConversion(ChatMessage msg)
        {
            return new DataStore.Message()
            {
                message = msg.Msg,
                userID = msg.UserID
            };
        }

        private ChatMessage DBMessageConversion(DataStore.Message m)
        {
            return new ChatMessage()
            {
                Msg = m.message,
                UserID = m.userID
            };
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
