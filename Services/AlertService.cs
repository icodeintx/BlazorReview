using System;

namespace WebsiteBlazor.Services
{
    public class AlertService
    {
        public event Action RefreshRequested;

        public string Message { get; private set; } = "";

        public void AddMessage(string alert)
        {
            this.Message = alert;

            RefreshRequested?.Invoke();

            // pop message off after a delay
            new System.Threading.Timer((_) =>
            {
                this.Message = "";
                RefreshRequested?.Invoke();
            }, null, 4000, System.Threading.Timeout.Infinite);
        }
    }
}