using System;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.SceneLoader
{
    public interface ISceneLoader
    {
        public UniTask LoadAsync(string sceneName);
    }
}