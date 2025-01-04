namespace Doco.Server.Gateway.Services;

internal interface IFileServiceUrlProvider
{
    string GetUrl();
    
    void SetUrls(string[] urls);
}