//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection.Extensions;
//using System;
//using System.Collections.Generic;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;
//using System.Text;
//using TweetBook.Contract.V1;
//using TweetBook.Contract.V1.Requests;
//using TweetBook.Contract.V1.Responses;
//using TweetBook.Data;

//namespace TweetBook.IntegrationTests
//{
//    public class IntegrationTest
//    {
//        protected readonly HttpClient TestClient;
//        //public IntegrationTest()
//        //{
//        //    var AppFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
//        //    {
//        //        builder.ConfigureServices(services =>
//        //        {
//        //            // Remove both the generic options and the specific context mapping 
//        //            services.RemoveAll(typeof(DbContextOptions<DataContext>));
//        //            services.RemoveAll(typeof(DbContextOptions));

//        //            services.AddDbContext<DataContext>(options =>
//        //            {
//        //                options.UseInMemoryDatabase("testDb");
//        //            });
//        //        });
//        //    });
//        //    TestClient = AppFactory.CreateClient();
//        //}
//        public IntegrationTest()
//        {
//            var AppFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
//            {
//                builder.ConfigureServices(services =>
//                {
//                    // 1. Remove the specific DataContext registration
//                    services.RemoveAll(typeof(DbContextOptions<DataContext>));

//                    // 2. CRITICAL: Remove the generic options that cache the SQL Server provider internals
//                    services.RemoveAll(typeof(DbContextOptions));

//                    // 3. Register a completely fresh In-Memory database
//                    services.AddDbContext<DataContext>(options =>
//                    {
//                        options.UseInMemoryDatabase("testDb");
//                    });
//                });
//            });

//            TestClient = AppFactory.CreateClient();
//        }
//        protected async Task AuthenticateAsync()
//        {
//            TestClient.DefaultRequestHeaders.Authorization =new  AuthenticationHeaderValue("bearer",await GetJwtAsync());
//        }

//        public async Task<string> GetJwtAsync()
//        {
//            var response = await TestClient.PostAsJsonAsync(ApiRoute.Identity.Register,new UserRegistrationRequest
//            {
//                Email="test@integration.com",
//                Password="ik@123ASA"
//            });
//            response.EnsureSuccessStatusCode();
//            var registrationResponse = await response.Content.ReadFromJsonAsync<AuthSuccessResponse>();
//            return registrationResponse.token;

//        }
//    }
//}
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TweetBook.Contract.V1;
using TweetBook.Contract.V1.Requests;
using TweetBook.Contract.V1.Responses;
using TweetBook.Data;

namespace TweetBook.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;

        public IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                //    builder.UseEnvironment("Testing");

                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<DataContext>();
                        services.RemoveAll<DbContextOptions<DataContext>>();

                        var serviceProvider = new ServiceCollection()
                            .AddEntityFrameworkInMemoryDatabase()
                            .BuildServiceProvider();

                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                            options.UseInternalServiceProvider(serviceProvider);
                        });
                    });
                });

            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
        }

        public async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync(
                ApiRoute.Identity.Register,
                new UserRegistrationRequest
                {
                    Email = "test@integration.com",
                    Password = "ik@123ASA"
                });

            response.EnsureSuccessStatusCode();

            var registrationResponse =
                await response.Content.ReadFromJsonAsync<AuthSuccessResponse>();

            return registrationResponse.token;
        }
    }
}
