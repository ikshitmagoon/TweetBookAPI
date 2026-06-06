using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using TweetBook.Contract.V1;
using TweetBook.Domain;

namespace TweetBook.IntegrationTests
{
    public class PostsControllerTests :IntegrationTest
    {
        [Fact]
        public async Task Get_ALl_WithoutANyPosts_ReturnEmptyRespose()
        {
            //Arrange
            await AuthenticateAsync();
            //act
            var response = await TestClient.GetAsync(ApiRoute.Posts.GetAll);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<List<Post>>()).Should().BeEmpty();
        }
    }
}
