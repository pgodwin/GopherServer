using GopherServer.Core.Results;

namespace GopherServer.Core.Providers
{
    public interface IServerProvider
    {
        BaseResult GetResult(string selector);
        void Init();
    }
}