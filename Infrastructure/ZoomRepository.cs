using Core.Zoom;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure
{
    public class ZoomRepository : IZoomRepository
    {
        private readonly string _sdkKey;
        private readonly string _sdkSecret;
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public ZoomRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _sdkKey = _configuration["ZoomConfig:SdkKey"]!;
            _sdkSecret = _configuration["ZoomConfig:SdkSecret"]!;
            _connectionString = _configuration.GetConnectionString("DataAccessConnection")!;
        }

        public IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<Response<SessionAuthorizeResponse>> StartZoomCall()
        {
            SessionAuthorizeResponse sessionAuthorizeResponse = new SessionAuthorizeResponse();
            Response<SessionAuthorizeResponse> response = new Response<SessionAuthorizeResponse>();
            ZoomConfig zoomConfig = new ZoomConfig();
            Guid userId = Guid.NewGuid();

            var sessionResponse = await AuthorizeZoomSessionAsync(userId, role: 1);


            if (sessionResponse != null)
            {
                sessionAuthorizeResponse.SessionPasscode = RandomString(10);
                sessionAuthorizeResponse.Signature = sessionResponse.Signature;
                sessionAuthorizeResponse.SessionName = sessionResponse.SessionName;
                sessionAuthorizeResponse.SessionPasscode = sessionAuthorizeResponse.SessionPasscode;
            }
            response.Data = sessionAuthorizeResponse;
            response.Status = true;
            response.Message = "Success";


            return response;

        }
        private static string GenerateSignature(string sdkKey, string sdkSecret, string sessionName, int role)
        {
            var iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 30;
            var exp = iat + 60 * 60 * 2;

            var oHeader = new Dictionary<string, object>
            {
                { "alg", "HS256" },
                { "typ", "JWT" }
            };

            var oPayload = new Dictionary<string, object>
            {
                { "app_key", sdkKey },
                { "tpc", sessionName },
                { "role_type", role },
                { "version", 1 },
                { "iat", iat },
                { "exp", exp }
            };

            var sHeader = JsonConvert.SerializeObject(oHeader);
            var sPayload = JsonConvert.SerializeObject(oPayload);

            var encodedHeader = Base64UrlEncode(Encoding.UTF8.GetBytes(sHeader));
            var encodedPayload = Base64UrlEncode(Encoding.UTF8.GetBytes(sPayload));

            var signatureInput = encodedHeader + "." + encodedPayload;

            var secretBytes = Encoding.UTF8.GetBytes(sdkSecret);
            using (var hmac = new HMACSHA256(secretBytes))
            {
                var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signatureInput));
                var encodedSignature = Base64UrlEncode(signatureBytes);
                return signatureInput + "." + encodedSignature;
            }
        }

        private static string Base64UrlEncode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }

        private static long ToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000;
            return epoch;
        }
        private async Task<SessionAuthorizeResponse> AuthorizeZoomSessionAsync(Guid userId, int role)
        {
            string sdkKey = _sdkKey;
            string sdkSecret = _sdkSecret;

            if (string.IsNullOrWhiteSpace(sdkKey))
                throw new ArgumentException("Zoom SDK Key is null or empty.");

            if (string.IsNullOrWhiteSpace(sdkSecret))
                throw new ArgumentException("Zoom SDK Secret is null or empty.");

            string timestamp = (ToTimestamp(DateTime.UtcNow.ToUniversalTime()) - 30000).ToString();

            string sessionName = $"session_{userId}_{timestamp}";
            if (string.IsNullOrWhiteSpace(sessionName))
                throw new ArgumentException("Generated sessionName is null or empty.");

            string signature = GenerateSignature(sdkKey, sdkSecret, sessionName, role);

            return new SessionAuthorizeResponse
            {
                SessionName = sessionName,
                Signature = signature
            };
        }


        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new StringBuilder(length);
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }


    }
}
