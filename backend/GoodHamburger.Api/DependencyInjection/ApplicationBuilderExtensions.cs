namespace GoodHamburger.Api.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseApi(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
           
            app.UseCors(policy =>
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}