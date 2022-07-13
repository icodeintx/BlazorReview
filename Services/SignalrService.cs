using AmaraCode.RainMaker.SignalrClient;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;

namespace WebsiteBlazor.Services;

/// <summary>
/// Service that provides SigalR Hub connections DataService SignalR server.
/// </summary>
public class SignalrService : BaseService
{
    private UserService _userService;

    /// <summary>
    ///
    /// </summary>
    /// <param name="auth"></param>
    /// <param name="redis"></param>
    /// <param name="appConfig"></param>
    /// <param name="userService"></param>
    public SignalrService(AuthenticationStateProvider auth, ICacheService redis,
            AppConfig appConfig, UserService userService) : base(auth, redis, appConfig)
    {
        _userService = userService;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="traderName"></param>
    /// <returns></returns>
    public TraderMonitorClient GetTradeMonitorClient(string traderName)
    {
        var client = TraderMonitorClient.CreateForReceiver(base._appConfig.DataSericeURL, traderName, base._appConfig.RmApiKey);

        return client;
    }

    /// <summary>
    /// Creates a TraderHubClient but not for a Trader.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public TraderHubClient GetTraderHubClient(string userName)
    {
        var client = TraderHubClient.CreateForNonTrader(base._appConfig.DataSericeURL, userName, base._appConfig.RmApiKey);

        return client;
    }

    /*    /// <summary>
        /// Creates a TraderHubServer connection to the DataService API
        /// </summary>
        /// <returns></returns>
        public HubConnection TraderHubServerConnection()
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"{base._appConfig.DataSericeURL}/traderhub", options =>
                {
                    options.Headers.Add("userId", $"WebsiteUser {_userService.GetCurrentUserName}");
                    options.Headers.Add("Authorization", $"Bearer {base._appConfig.RmApiKey}");
                })
                .Build();

            hubConnection.StartAsync();
            return hubConnection;
        }*/

    /* /// <summary>
     /// Creates a TraderMonitorServer connection to the DataService API
     /// </summary>
     /// <returns></returns>
     public HubConnection TraderMonitorServerConnection()
     {
         var hubConnection = new HubConnectionBuilder()
             .WithUrl($"{base._appConfig.DataSericeURL}/tradermonitor", options =>
             {
                 options.Headers.Add("userId", $"WebsiteUser {_userService.GetCurrentUserName}");
                 options.Headers.Add("Authorization", $"Bearer {base._appConfig.RmApiKey}");
             })
             .Build();

         hubConnection.StartAsync();
         return hubConnection;
     }*/
}