# Tutorial: BlazorServer Configuration Sources
YouTube link: https://youtu.be/LwF7ml04P_o

Below is the code to add in Program.cs for two special configuration sources: Azure Key Vault and Azure App Configuration.
You need to set the correct literals in the new Uri statements based on the actual sources you create on the Azure portal!
```C#
var configuration = builder.Configuration;
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
```
