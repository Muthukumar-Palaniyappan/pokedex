using System;
using System.Collections.Generic;
using System.Text;
using WireMock.Server;

namespace Pokedex.Api.IntegrationTests
{
    public static class WireMockServers
    {
        public static WireMockServer mockPokedexServer;
        public static WireMockServer mockTranslatorServer;
    }
}
