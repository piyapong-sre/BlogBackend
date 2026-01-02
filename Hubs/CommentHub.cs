using Microsoft.AspNetCore.SignalR;

namespace BlogBackend.Hubs;

public class CommentHub : Hub
{

    public async Task JoinBlogPostGroup(int blogPostId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"BlogPost_{blogPostId}");
        await Clients.Caller.SendAsync("JoinedGroup", blogPostId);
    }
    
    public async Task LeaveBlogPostGroup(int blogPostId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"BlogPost_{blogPostId}");
        await Clients.Caller.SendAsync("LeftGroup", blogPostId);
    }

    public async Task UserStoppedTyping(int blogPostId, string userName)
    {
        await Clients.OthersInGroup($"BlogPost_{blogPostId}")
            .SendAsync("UserStoppedTyping", userName);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}

