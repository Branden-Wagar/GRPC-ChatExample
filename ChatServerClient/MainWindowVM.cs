using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace ChatServerClient
{
    public class MainWindowVM : ObservableObject, IDisposable
    {
        private static string _host = "localhost";
        private static int _port = 8080;
        private Channel _channel = new Channel(_host + ":" + _port, ChannelCredentials.Insecure);
        private ChatServerProto.ChatServerService.ChatServerServiceClient _client;
        private DelegateCommand _sendMessageCommand;
        private DelegateCommand _getMessagesCommand;
        private string _boardContent;

        public MainWindowVM()
        {
            _client = new ChatServerProto.ChatServerService.ChatServerServiceClient(_channel);
        }

        public string Message { get; set; }
        public int UserId { get; set; }

        public string BoardContent
        {
            get => _boardContent;
            set
            {
                _boardContent = value;
                OnPropertyChanged("BoardContent");
            }
        }

        public DelegateCommand SendMessageCommand
        {
            get
            {
                if (_sendMessageCommand == null)
                {
                    _sendMessageCommand = new DelegateCommand(SendMessage);
                }
                return _sendMessageCommand;
            }
        }

        public DelegateCommand GetMessagesCommand
        {
            get
            {
                if (_getMessagesCommand == null)
                {
                    _getMessagesCommand = new DelegateCommand(GetMessagesAsync);
                }
                return _getMessagesCommand;
            }
        }

        public void Dispose()
        {
            _channel.ShutdownAsync();
        }

        private async void GetMessagesAsync(object obj)
        {
            var messageStream = _client.GetMessages(new ChatServerProto.GetMessageRequest());
            BoardContent = "";
            
            while (await messageStream.ResponseStream.MoveNext())
            {
                var current = messageStream.ResponseStream.Current;
                BoardContent += current.UserID + " : " + current.Msg;
                BoardContent += "\n";
            }
        }

        private void SendMessage(object obj)
        {
            
            var result = _client.PostMessage(new ChatServerProto.ChatMessage()
            {
                Msg = Message,
                UserID = UserId
            });
            if (result.Code != 200)
            {
                throw new Exception("Received error code " + result.Code + " from server");
            }
        }
    }
}
