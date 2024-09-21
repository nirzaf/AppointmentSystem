using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AppointmentSystem.Services
{
    public interface ISmsService
    {
        Task SendSmsAsync(string phoneNumber, string message);
    }

    public class SmsService : ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmsService> _logger;

        public SmsService(HttpClient httpClient, IConfiguration configuration, ILogger<SmsService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            var smsProviderUrl = _configuration["SmsProvider:Url"];
            var smsProviderApiKey = _configuration["SmsProvider:ApiKey"];

            var payload = new
            {
                to = phoneNumber,
                body = message
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, smsProviderUrl)
                {
                    Headers = { { "Authorization", $"Bearer {smsProviderApiKey}" } },
                    Content = content
                };

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("SMS sent successfully to {PhoneNumber}", phoneNumber);
                }
                else
                {
                    _logger.LogError("Failed to send SMS to {PhoneNumber}. Status Code: {StatusCode}", phoneNumber, response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending SMS to {PhoneNumber}", phoneNumber);
            }
        }
    }
}
