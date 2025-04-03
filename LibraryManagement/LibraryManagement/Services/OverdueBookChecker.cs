
using LibraryManagement.Repositories.Interface;

namespace LibraryManagement.Services
{
    public class OverdueBookChecker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OverdueBookChecker> _logger;
        public OverdueBookChecker(IServiceProvider serviceProvider, ILogger<OverdueBookChecker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Overdue book checker service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var borrowRepository = scope.ServiceProvider.GetRequiredService<IBorrowRepository>();
                        await borrowRepository.UpdateOverdueBooksAsync();
                    }
                    _logger.LogInformation("Checked and updated overdue books at: {Time}", DateTime.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error updating overdue books: {ex.Message}");
                }

                // Chạy lại sau 24h
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
