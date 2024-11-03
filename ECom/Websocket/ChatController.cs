// Controllers/ChatController.cs
using ECom;
using ECom.Websocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

[ApiController]
public class ChatController : Controller
{
    private readonly WebSocketService _webSocketService;
    private readonly EComContext _eComContext;

    public ChatController(WebSocketService webSocketService, EComContext context)
    {
        _webSocketService = webSocketService;
        _eComContext = context;
    }

    [HttpGet("/chat/{userid}")]
    public async Task AcceptWebSocket(string userid)
    {
        var user = await _eComContext.AppUsers.FirstOrDefaultAsync(u => u.UserId.Equals(userid));
        if(user == null)
        {
            HttpContext.Response.StatusCode = 400; // Bad Request
            return;
        }    
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            await _webSocketService.UserRequestForChat(HttpContext, userid);
        }
        else
        {
            HttpContext.Response.StatusCode = 400; // Bad Request
        }
    }

    [HttpGet("/chat/admin/{userid}")]
    public async Task AcceptAdminWebSocket(string userid)
    {
        var user = await _eComContext.AppUsers.FirstOrDefaultAsync(u => u.UserId.Equals(userid));
        if (user == null)
        {
            HttpContext.Response.StatusCode = 400; // Bad Request
            return;
        }
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            await _webSocketService.AdminAnswerUser(HttpContext, userid);
        }
        else
        {
            HttpContext.Response.StatusCode = 400; // Bad Request
        }
    }

    [HttpGet("/chat")]
    public async Task<ActionResult>
        GetOnlineClients([FromQuery]string? userid)
    {
        if (string.IsNullOrEmpty(userid))
        {
            return Ok(await _webSocketService.AvailableChannels(_eComContext));
        }

        return Ok(_webSocketService.GetChat(userid));
    }
}
