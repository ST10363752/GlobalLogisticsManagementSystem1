using System.Text;
using System.Text.Json;
using GlobalLogisticsManagementSystem.Models;

namespace GlobalLogisticsManagementSystem.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            // Check if running in Docker (environment variable can be set)
            var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

            if (isDocker)
            {
                // Use Docker service name when running in containers
                _apiBaseUrl = "http://glms-api/api/";
            }
            else
            {
                // Use localhost when running normally
                _apiBaseUrl = "https://localhost:7095/api/";
            }

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
        }

        // ========== CLIENTS ==========
        public async Task<List<Client>> GetClientsAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}clients");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Client>>(json, _jsonOptions) ?? new List<Client>();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}clients/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Client>(json, _jsonOptions);
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            var content = new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}clients", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Client>(json, _jsonOptions) ?? client;
        }

        public async Task<bool> UpdateClientAsync(int id, Client client)
        {
            var content = new StringContent(JsonSerializer.Serialize(client), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_apiBaseUrl}clients/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}clients/{id}");
            return response.IsSuccessStatusCode;
        }

        // ========== CONTRACTS ==========
        public async Task<List<Contract>> GetContractsAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}contracts");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Contract>>(json, _jsonOptions) ?? new List<Contract>();
        }

        public async Task<Contract?> GetContractByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}contracts/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Contract>(json, _jsonOptions);
        }

        public async Task<List<Contract>> FilterContractsAsync(DateTime? startDate, DateTime? endDate, string? status)
        {
            var url = $"{_apiBaseUrl}contracts/filter?";
            if (startDate.HasValue) url += $"startDate={startDate.Value:yyyy-MM-dd}&";
            if (endDate.HasValue) url += $"endDate={endDate.Value:yyyy-MM-dd}&";
            if (!string.IsNullOrEmpty(status)) url += $"status={status}&";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Contract>>(json, _jsonOptions) ?? new List<Contract>();
        }

        public async Task<Contract> CreateContractAsync(Contract contract)
        {
            var content = new StringContent(JsonSerializer.Serialize(contract), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}contracts", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Contract>(json, _jsonOptions) ?? contract;
        }

        public async Task<bool> UpdateContractStatusAsync(int id, string status)
        {
            var content = new StringContent($"\"{status}\"", Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{_apiBaseUrl}contracts/{id}/status", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteContractAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}contracts/{id}");
            return response.IsSuccessStatusCode;
        }

        // ========== SERVICE REQUESTS ==========
        public async Task<List<ServiceRequest>> GetServiceRequestsAsync()
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}servicerequests");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ServiceRequest>>(json, _jsonOptions) ?? new List<ServiceRequest>();
        }

        public async Task<ServiceRequest?> GetServiceRequestByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}servicerequests/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ServiceRequest>(json, _jsonOptions);
        }

        public async Task<bool> CanCreateServiceRequestAsync(int contractId)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}servicerequests/can-create/{contractId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            if (root.TryGetProperty("canCreate", out var canCreateElement))
            {
                return canCreateElement.GetBoolean();
            }

            return false;
        }

        public async Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request)
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}servicerequests", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ServiceRequest>(json, _jsonOptions) ?? request;
        }

        public async Task<bool> UpdateServiceRequestStatusAsync(int id, string status)
        {
            var content = new StringContent($"\"{status}\"", Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{_apiBaseUrl}servicerequests/{id}/status", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteServiceRequestAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}servicerequests/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}