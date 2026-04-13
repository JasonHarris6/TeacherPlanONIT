using Microsoft.AspNetCore.Mvc.Testing;

namespace TeacherPlanApp.Tests
{
    public class HomePageTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HomePageTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_HomePage_ReturnsSuccessAndCorrectContentType()
        {
            // Arrange: создаем виртуальный браузер (клиент)
            var client = _factory.CreateClient();

            // Act: делаем GET-запрос на главную страницу (/)
            var response = await client.GetAsync("/");

            // Assert: проверяем, что сервер ответил кодом 200-299 (Успех)
            response.EnsureSuccessStatusCode(); 
            
            // И проверяем, что вернулся HTML-документ
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }
    }
}