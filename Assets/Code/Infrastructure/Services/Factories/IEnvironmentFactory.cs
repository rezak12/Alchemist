using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.Factories
{
    public interface IEnvironmentFactory
    {
        UniTask<GameObject> CreateEnvironmentAsync(Vector3 position);
    }
}