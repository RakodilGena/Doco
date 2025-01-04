namespace Doco.Server.Gateway.Services.Internal;

internal interface IFileServiceUrlProvider
{
    string GetUrl();
    
    void SetUrls(string[] urls);
}