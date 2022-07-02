using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Pokedex.Api.IntegrationTests
{
    public class IntegrationTest
    {
        protected readonly HttpClient _testClient;

        public IntegrationTest()
        {
            var appFactory = new IntegrationTestFactory<Startup>();
            _testClient = appFactory.CreateClient();

        }
    }
}
