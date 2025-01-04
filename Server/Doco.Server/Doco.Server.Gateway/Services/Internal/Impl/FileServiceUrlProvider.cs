using System.Diagnostics;
using Doco.Server.Gateway.Exceptions;

namespace Doco.Server.Gateway.Services.Internal.Impl;

internal sealed class FileServiceUrlProvider : IFileServiceUrlProvider
{
    private int _urlIdx;
    private string[]? _urls;

    private readonly Lock _lock = new();

    public string GetUrl()
    {
        lock (_lock)
        {
            if (_urls is null or { Length: 0 })
                throw new NotDiscoveredException("No file service urls discovered.");

            Debug.Assert(_urlIdx < _urls.Length);

            var url = _urls[_urlIdx];

            RoundRobinUrlIdx();

            return url;
        }
    }

    private void RoundRobinUrlIdx()
    {
        if (_urlIdx == _urls!.Length - 1)
        {
            _urlIdx = 0;
        }
        else
        {
            _urlIdx++;
        }
    }

    public void SetUrls(string[] urls)
    {
        lock (_lock)
        {
            //no urls discovered yet, just set
            if (_urls is null or { Length: 0 })
            {
                _urls = urls;
                _urlIdx = 0;
                return;
            }

            //if current urlIdx is applicable to new array
            //(new urls extend current or at least not decrease them)
            //just set
            if (_urlIdx < urls.Length)
            {
                _urls = urls;
                return;
            }

            //urls are decreased and currentUrlIdx not applicable to new urls
            _urls = urls;
            _urlIdx = 0;
        }
    }
}