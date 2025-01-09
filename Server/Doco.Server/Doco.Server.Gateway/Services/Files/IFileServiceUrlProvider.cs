namespace Doco.Server.Gateway.Services.Files;

internal interface IFileServiceUrlProvider
{
    string GetUrl();
    
    void SetUrls(string[] urls);
}