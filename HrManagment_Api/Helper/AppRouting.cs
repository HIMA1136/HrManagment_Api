namespace HrManagment_Api.Helper;

public static class AppRouting
{
    public static void MapEndPoints(this WebApplication app)
    {
        var endPointImplement = typeof(Program).Assembly
            .GetTypes()
            .Where(m => m.IsAssignableTo(typeof(IApiInterface))
            && !m.IsAbstract && !m.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IApiInterface>();

        foreach (var endPoint in endPointImplement)
            endPoint.RegisterEndPoint(app);
    }
}
