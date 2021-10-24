using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pokedex.Api.Application.Exceptions;
using System.Diagnostics;
using System.Net;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pokedex.Api.Application.Middlewares
{
    public class PokeApiExceptionhandler : IExceptionHandler
    {
        private readonly ILogger<PokeApiExceptionhandler> _logger;
        private readonly IHostEnvironment _environment;

        public PokeApiExceptionhandler(ILogger<PokeApiExceptionhandler> logger,
                                               IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public Error HandleException(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            // Used Ben's to simplify exception messages. 
            // See: https://github.com/benaadams/Ben.Demystifier#problems-with-current-stack-traces
            var demystified = exception.Demystify();

            return CreateErrorDocument(demystified);
        }

        private Error CreateErrorDocument(Exception exception)
        {
            var error = exception switch
            {
                ValidationException validationException => HandleValidationException(validationException),
                DomainException domainException => HandleDomainException(domainException),  
                ResourceNotFoundException resourceNotFoundException => HandleResourceNotFoundException(resourceNotFoundException),
                _ => HandleOtherExceptions(exception)
            };

            if (!_environment.IsProduction())
            {
                error.Exception = exception.ToStringDemystified();
            }

            return error;
        }

        private Error HandleResourceNotFoundException(Exception resourceNotFoundException)
        {
            _logger.LogWarning(resourceNotFoundException,
                               GetLogMessage(resourceNotFoundException));


            return new Error
            {
                StatusCode = HttpStatusCode.NotFound,
                Title = resourceNotFoundException.Message
            };
        }

        private Error HandleValidationException(ValidationException validationException)
        {
            _logger.LogWarning(  validationException,
                                 GetLogMessage(validationException));
            ///TODO: Multiple validation messages to be handled. Implementation pending with Fluentvalidator against API request objects. 
            return new Error
            {
                Title = $"One or more validation errors occurred. {validationException.Message}",
                StatusCode = HttpStatusCode.BadRequest,

            };

        }

        //DomainExceptions are categorized to handle the unexpected validation errors in evaluating a business logic. 
        private Error HandleDomainException(Exception domainException)
        {
            _logger.LogWarning(domainException,
                                 GetLogMessage(domainException));

            return new Error
            {
                Title = $"One or more validation errors occurred. {domainException.Message}",
                StatusCode = HttpStatusCode.BadRequest,
            };
        }

        private Error HandleOtherExceptions(Exception exception)
        {
            _logger.LogError(exception,
                                 GetLogMessage(exception));

            return new Error
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Title = "An unhandled error occurred while processing this request."
            };
        }

        private static string GetLogMessage(Exception exception)
        {
            return exception.Message;
        }
    }
}
