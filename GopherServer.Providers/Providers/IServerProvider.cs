using GopherServer.Core.GopherResults;

namespace GopherServer.Core.Providers
{
    public interface IServerProvider
    {
        BaseResult GetResult(string selector);
        void Init();
    }
}