using System.Collections.Generic;
using System.Net;

namespace Pokedex.Api.Application.Exceptions
{
    public  class Error
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Title { get; set; }

        public string Exception { get; set; }

    }
}