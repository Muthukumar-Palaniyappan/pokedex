using Pokedex.Api.Application.Exceptions;
using System;

namespace Pokedex.Api.Application.Middlewares
{
    public interface IExceptionHandler
    {
        public Error HandleException(Exception exception);
    }
}
