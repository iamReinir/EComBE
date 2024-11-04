using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EComAdmin.Pages.Chat
{
    public class RoomModel : PageModel
    {
        IConfiguration config;
        private readonly HttpClient _channel;
        public RoomModel(IConfiguration config, HttpClient channel) {
            this.config = config;
            _channel = channel;
        }
        [BindProperty]
        public string UserId { get; set; } = string.Empty;

        [BindProperty]
        public string WSendpoint { get; set; }

        [BindProperty]
        public IEnumerable<ChatItem> Items { get; set; }
        public async Task<ActionResult> OnGetAsync(string id)
        {
            UserId = id;
            WSendpoint = config["ws_endpoint"] ?? "ws://localhost:5000";
            var responseMessage = await _channel.GetAsync("chat");
            if (responseMessage.IsSuccessStatusCode)
            {
                Items = await responseMessage.Content
                    .ReadAsAsync<IEnumerable<ChatItem>>();
            }
            return Page();
        }

        public class ChatItem
        {
            [BindProperty]
            public string Content { get; set; }
            [BindProperty]
            public bool IsAnswer { get; set; }
        }
    }
}
