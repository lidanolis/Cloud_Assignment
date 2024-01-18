using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cloud_Assignment.Data;
using Cloud_Assignment.Areas.Identity.Data;
using Amazon.XRay.Recorder.Handlers.AwsSdk;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Cloud_AssignmentContextConnection") ?? throw new InvalidOperationException("Connection string 'Cloud_AssignmentContextConnection' not found.");

builder.Services.AddDbContext<Cloud_AssignmentContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<Cloud_AssignmentUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Cloud_AssignmentContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
AWSSDKHandler.RegisterXRayForAllServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseXRay("FoodBankManagementSystem");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
