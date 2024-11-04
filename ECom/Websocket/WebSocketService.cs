using EComBusiness.Entity;
using EComBusiness.HelperModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using static ECom.Websocket.WebSocketService;
namespace ECom.Websocket;
public class WebSocketService
{
    private readonly ConcurrentDictionary<string, ChatChannel>
        _clients = new ConcurrentDictionary<string, ChatChannel>();

    // Blocking waiting for chat
    public async Task<bool> UserRequestForChat(HttpContext context, string userid)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var channel = new ChatChannel(webSocket);
        _clients.AddOrUpdate(userid, channel, (_, _) => channel);
        await HandleClient(webSocket, userid);
        return true;
    }

    public async Task<bool> AdminAnswerUser(HttpContext context, string userid)
    {
        if (!_clients.TryGetValue(userid, out var chatChannel))
            return false;
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        if (!_clients.TryUpdate(userid, chatChannel.AdminEnter(webSocket), chatChannel))
        {
            Clean(userid);
            return false;
        }
        await HandleAdmin(webSocket, userid);
        // Leave channel
        if (_clients.TryGetValue(userid, out chatChannel))
            if (_clients.TryUpdate(userid, chatChannel.AdminLeave(), chatChannel))
                return true;
        return false;
    }

    public void Clean(string userid)
    {
        _clients.TryRemove(userid, out _);
    }

    private async Task HandleClient(WebSocket webSocket, string userid)
    {
        var buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var messageData = Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (!_clients.TryGetValue(userid, out var chatChannel))
                    continue;
                if(chatChannel.AdminSocket != null)
                    await SendMessageViaSocket(messageData, chatChannel.AdminSocket);
                _clients.TryUpdate(userid, chatChannel.AddMessage(messageData, false), chatChannel);
            }
        }
        _clients.TryRemove(userid, out _);
    }

    private async Task HandleAdmin(WebSocket webSocket, string userid)
    {
        var buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {    
                var messageData = Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (!_clients.TryGetValue(userid, out var chatChannel))
                    continue;
                if(await SendMessageViaSocket(messageData, chatChannel.Socket) == false)
                {
                    Clean(userid); // send message fail. Remove the entry.
                    return;
                }
                if (!_clients.TryUpdate(userid, chatChannel.AddMessage(messageData, true), chatChannel))
                    continue;
            }
        }
    }

    private async Task<bool> SendMessageViaSocket(string message, WebSocket socket)
    {
        var buffer = Encoding.UTF8.GetBytes($"{message}");
        var segment = new ArraySegment<byte>(buffer);
        if (socket.State == WebSocketState.Open)
        {
            await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            return true;
        }
        return false;
    }
    public async Task<IEnumerable<SimpleChannel>> AvailableChannels(EComContext context)
    {
        var userIds = _clients
            .Where(c => c.Value.AdminSocket == null) // Admin havent enter
            .Select(x => x.Key);
        var users = await context.AppUsers.AsNoTracking().NotDeleted()
            .Where(u => userIds.Contains(u.UserId))
            .ToListAsync();
        return (from id in userIds
                join user in users on id equals user.UserId
                select new SimpleChannel
                {
                    Userid = id,
                    Username = user.Name
                });
    }

    public IEnumerable<ChatItem> GetChat(string userid)
    {
        if (!_clients.TryGetValue(userid, out var chatChannel))
            return new List<ChatItem>();
        return chatChannel.Msgs.OrderBy(msg => msg.Order);
    }
    public class ChatChannel
    {
        public IEnumerable<ChatItem> Msgs { get; set; } = new List<ChatItem>();
        public WebSocket Socket { get; set; }
        public WebSocket? AdminSocket { get; set; }
        public ChatChannel(WebSocket socket)
        {
            Socket = socket;
        }

        // return a new instance
        public ChatChannel AddMessage(string content, bool isAnswer)
        {
            return new ChatChannel(Socket)
            {
                AdminSocket = AdminSocket,
                Msgs = Msgs.Append(new ChatItem
                {
                    Content = content,
                    isAnswer = isAnswer,
                    Order = Msgs.Count()
                })
            };
        }

        public ChatChannel AdminEnter(WebSocket socket)
        {
            return new ChatChannel(Socket)
            {
                AdminSocket = socket,
                Msgs = Msgs.Select(x => x) // Clone IEnumerable
            };
        }

        public ChatChannel AdminLeave()
        {
            return new ChatChannel(Socket)
            {
                AdminSocket = null,
                Msgs = Msgs.Select(x => x) // Clone IEnumerable
            };
        }
    }

    public class ChatItem
    {
        public int Order;
        public string Content { get; set; } = string.Empty;
        public bool isAnswer { get; set; } = false;
    }

    public class SimpleChannel
    {
        public string Username { get; set; }
        public string Userid { get; set; }
    }
}
