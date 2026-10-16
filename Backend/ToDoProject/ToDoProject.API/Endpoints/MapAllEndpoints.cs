namespace ToDoProject.API.Endpoints
{
    public static class MapAllEndpoints
    {
        public static IEndpointRouteBuilder MapAllApplicationEndpoints(
            this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapProductEndpoints();

            return endpoints;
        }
    }
}