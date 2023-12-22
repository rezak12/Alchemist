using System;
using System.Collections.Generic;
using System.Threading;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.RandomServices;
using Code.Infrastructure.Services.StaticData;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Services.Ambient
{
    public class AmbientPlayer : MonoBehaviour, IAmbientPlayer
    {
        [SerializeField] private AudioSource _audioSource;
        private AssetReferenceT<AudioClip> _currentClipReference;
        private AssetReferenceT<AudioClip> _nextClipReference;
        private AmbientReferencesCatalog _ambientReferencesCatalog;
        private CancellationToken _cancellationToken;

        private IStaticDataService _staticDataService;
        private IAssetProvider _assetProvider;
        private IRandomService _randomService;

        [Inject]
        private void Construct(
            IStaticDataService staticDataService, 
            IAssetProvider assetProvider, 
            IRandomService randomService)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _randomService = randomService;
        }
        
        public async UniTask InitializeAsync()
        {
            _cancellationToken = this.GetCancellationTokenOnDestroy();
            LoadCatalog();
            
            await PrepareNextClip(_cancellationToken);
        }

        public async UniTaskVoid StartPlayingLoop()
        {
            Func<bool> waitUntil = () => !_audioSource.isPlaying;
            
            while(true)
            {
                await PlayNextClip(_cancellationToken);
                await PrepareNextClip(_cancellationToken);
                await UniTask.WaitUntil(waitUntil, cancellationToken: _cancellationToken);
            }
        }

        private async UniTask PlayNextClip(CancellationToken token)
        {
            AudioClip clip = await TakeNextClip(token);
            _audioSource.PlayOneShot(clip);
        }

        private async UniTask<AudioClip> TakeNextClip(CancellationToken token)
        {
            AssetReferenceT<AudioClip> previousClipReference = _currentClipReference;
            _currentClipReference = _nextClipReference;

            if (previousClipReference != null)
            {
                _assetProvider.Release(previousClipReference);
            }
            return await _assetProvider.LoadAsync<AudioClip>(_currentClipReference);
        }

        private async UniTask PrepareNextClip(CancellationToken token)
        {
            _nextClipReference = TakeRandomReference();
            await _assetProvider.LoadAsync<AudioClip>(_nextClipReference);
        }

        private void LoadCatalog() => 
            _ambientReferencesCatalog = _staticDataService.GetAmbientReferencesCatalog();

        private AssetReferenceT<AudioClip> TakeRandomReference()
        {
            List<AssetReferenceT<AudioClip>> ambientReferences = _ambientReferencesCatalog.AmbientReferences;
            return ambientReferences[_randomService.Next(0, ambientReferences.Count)];
        }
    }
}