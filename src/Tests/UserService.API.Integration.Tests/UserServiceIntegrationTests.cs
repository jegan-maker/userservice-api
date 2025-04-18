using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using UserService.Domain.Users;
using Microsoft.AspNetCore.Http;

namespace UserService.API.Integration.Tests
{
    public class UserServiceIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UserServiceIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Create_And_Get_Update_User_Should_Return_Ok()
        {
            var createRequest = new { fullName = "API Test", email = "api@test.com" };

            // CREATE
            var createResponse = await _client.PostAsJsonAsync("/api/users", createRequest);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var userId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            userId.Should().NotBe(Guid.Empty);

            // GET
            var getResponse = await _client.GetAsync($"/api/users/{userId}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var user = await getResponse.Content.ReadFromJsonAsync<User>();
            user.Should().NotBeNull();
            
            // UPDATE
            var updateRequest = new
            {
                id = userId,
                fullName = "Updated API Test",
                email = "updated@test.com",
                RowVersion = user!.RowVersion
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/users", updateRequest);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // GET after update
            var getUpdated = await _client.GetAsync($"/api/users/{userId}");
            getUpdated.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedUser = await getUpdated.Content.ReadFromJsonAsync<User>();
            updatedUser.Should().NotBeNull();
            updatedUser!.FullName.Should().Be("Updated API Test");
            updatedUser.Email.Should().Be("updated@test.com");
        }
    }
}