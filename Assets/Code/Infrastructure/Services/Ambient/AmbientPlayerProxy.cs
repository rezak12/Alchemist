using Code.Data;
using Code.Infrastructure.Services.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.Ambient
{
    public class AmbientPlayerProxy : IAmbientPlayer
    {
        private readonly IPrefabFactory _prefabFactory;
        private IAmbientPlayer _ambientPlayer;

        public AmbientPlayerProxy(NonCachePrefabFactory prefabFactory)
        {
            _prefabFactory = prefabFactory;
        }

        public async UniTask InitializeAsync()
        {
            _ambientPlayer = await _prefabFactory.CreateAsync<AmbientPlayer>(
                ResourcesAddresses.AmbientPlayerAddress, Vector3.zero);

            await _ambientPlayer.InitializeAsync();
        }

        public UniTaskVoid StartPlayingLoop() => _ambientPlayer.StartPlayingLoop();
    }
}