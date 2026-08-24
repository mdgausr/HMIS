using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
app.MapPost("/abdm/verify", ([FromBody] dynamic req) =>
{
    string id = req?.healthId ?? "";
    var last = id.Length > 0 ? id[^1] : '0';
    var success = char.IsDigit(last) && ((last - '0') % 2 == 0);
    if (success) return Results.Ok(new { success = true, message = "Verified (mock)", name = "John Doe" });
    return Results.BadRequest(new { success = false, message = "Not found (mock)" });
});
app.Run();
