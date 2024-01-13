using Azure.Identity;
using TutorialBlazorServerConfigurationSources;
using TutorialBlazorServerConfigurationSources.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// ==============================
// Start code to add for tutorial
// ==============================
// Initialize ConfigurationManager with the standard providers (appsettings.json, environment variables, etc)
var configuration = builder.Configuration;
// Reference: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-8.0
/* The initialized WebApplicationBuilder (builder) provides DEFAULT CONFIGURATION for the app
 * in the following order, from highest to lowest priority:
 * 1. Command-line arguments.
 * 2. Non-prefixed environment variables (Azure App Services Configuration is exposed as environment variables)
 * 3. User secrets when the app runs in the Development environment.
 * 4. appsettings.{Environment}.json.
 *       For example, appsettings.Production.json and appsettings.Development.json.
 * 5. appsettings.json.
 */
// Note: Below we add Azure App Configuration and Azure Key Vault.
//      The "if" statement is set to include those configuration sources for Production environment.
//      ===================================================================================================
//      You can remove the "if" statement because the code will also work for Development in Visual Studio!
//          - it takes several seconds load the web page if you include those sources in Development.
//      ===================================================================================================
// Reference: https://azuresdkdocs.blob.core.windows.net/$web/dotnet/Azure.Identity/1.4.1/index.html
//      To authenticate in Visual Studio select the Tools > Options menu to launch the Options dialog.
//      Then navigate to the Azure Service Authentication options to sign in with your Azure Active Directory account.
// LIMITING TO PRODUCTION IS NOT MANDATORY; THE CODE WILL ALSO WORK IN DEVELOPMENT (Visual Studio).
if (builder.Environment.IsProduction())  
{
    // 1/10/2024 Configuration providers that are added later have higher priority and override the earlier configurations.
    //           So, in this order, the Key Vault configuration will override the Azure App Configuration
    configuration.AddAzureAppConfiguration(options =>
        options.Connect(
            new Uri("https://kentestappcfg.azconfig.io"),  // <--- this literal should be in appsettings.json!
            new DefaultAzureCredential()));                // Allows this app to run on VisualStudio (Development) and Production!
    configuration.AddAzureKeyVault(
            new Uri("https://kentestkv.vault.azure.net/"), // <--- this literal should be in appsettings.json!
            new DefaultAzureCredential());                 // Allows this app to run on VisualStudio (Development) and Production!
}
// ==============================
// End code to add for tutorial
// ==============================

var app = builder.Build();

/* 1/12/2024 This was used for debugging; the code is now in Home.razor.
IConfigurationRoot ConfigRoot = (IConfigurationRoot)app.Services.GetService<Microsoft.Extensions.Configuration.IConfiguration>();
string str = "";
foreach (var provider in ConfigRoot.Providers.ToList())
{
    str += provider.ToString() + "\n";
}
*/

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
