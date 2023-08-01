using ZenithBankATM.API;

var builder = WebApplication.CreateBuilder(args);

var app = builder.InjectServicesAndReturnWebApp();
app.Run();
