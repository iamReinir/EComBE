using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ECom;
using EComBusiness.Entity;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EComAdmin.Pages.Chat
{
    public class IndexModel : PageModel
    {
        private readonly ECom.EComContext _context;
        private readonly HttpClient _channel;

        [BindProperty]
        public IEnumerable<ChatChannel> Channels { get; set; } = default!;

        public IndexModel(ECom.EComContext context, HttpClient channel)
        {
            _context = context;
            _channel = channel;
        }

        public async Task OnGetAsync()
        {
            var responseMessage = (await _channel.GetAsync("chat"));
            if(responseMessage.IsSuccessStatusCode)
            {
                Channels = await responseMessage.Content
                    .ReadAsAsync<IEnumerable<ChatChannel>>();
            }
           
        }

        public class ChatChannel
        {
            public string Userid { get; set; }
            public string Username { get; set; }
        }
    }
}
