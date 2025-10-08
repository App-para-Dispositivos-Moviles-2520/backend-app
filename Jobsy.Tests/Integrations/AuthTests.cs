namespace Jobsy.Tests.Integrations
{
    using System.Net;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class AuthTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RegistrarYLogin()
        {
            var registerData = new
            {
                name = "Celipe Paredes",
                email = "aser@example.com",
                password = "Secret1234!",
                role = 1,
                description = "Usuarios consumidor"
            };

            var responseRegister = await _client.PostAsJsonAsync("/api/User", registerData);
            responseRegister.StatusCode.Should().Be(HttpStatusCode.Created);

            var registerContent = await responseRegister.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            registerContent.Should().ContainKey("id");

            var loginData = new
            {
                email = "aser@example.com",
                password = "Secret1234!"
            };

            var responseLogin = await _client.PostAsJsonAsync("/api/User/login", loginData);
            responseLogin.StatusCode.Should().Be(HttpStatusCode.OK);

            var loginContent = await responseLogin.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            loginContent.Should().ContainKey("token");
            loginContent["token"].Should().NotBeNull();
        }
    }
}