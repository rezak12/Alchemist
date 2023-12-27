using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.Ambient
{
    public interface IAmbientPlayer
    {
        UniTask InitializeAsync();
        UniTaskVoid StartPlayingLoop();
    }
}