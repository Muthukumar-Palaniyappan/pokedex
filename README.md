# Steps to Run Pokedex API
1. Navigate to base folder having dockerfile
2. Run docker build command **docker build  -t pokedex:v1 .**
3. Run docker image **docker run -p 5000:80 pokedex:v1** 
4. Hit the swagger url http://localhost:5000/swagger


# Features Implemented to highlight
1. Healthcheck endpoints using existing .net core middleware, could be helpful in production pod healthchecks by kubernetes/other orchestrators
2. Structured logging using Serilog, which will be helpful in debugging and building monitoring dashboards and alerts
3. Swagger

# Improvements
1. Unit Tests for negative cases & code coverage have to be improved
2. Mediator pattern and RequestValidation via Fluentvalidation
3. Docker file could be improved to expose 2 port mappings to do HTTPS. 
4. Caching 


