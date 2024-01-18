using System;
using System.Collections.Generic;
using System.Threading;
using Code.Infrastructure.Services.AssetProvider;
using Code.Infrastructure.Services.RandomServices;
using Code.Infrastructure.Services.StaticData;
using Code.StaticData;
using Code.StaticData.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Code.Infrastructure.Services.Ambient
{
    public class AmbientPlayer : MonoBehaviour, IAmbientPlayer
    {
        [SerializeField] private AudioSource _audioSource;
        
        private AsyncOperationHandle<AudioClip> _currentClipHandle;
        private AsyncOperationHandle<AudioClip> _nextClipHandle;
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
            
            await PrepareNextClip();
        }

        public async UniTaskVoid StartPlayingLoop()
        {
            Func<bool> waitUntil = () => !_audioSource.isPlaying;
            
            while(true)
            {
                await PlayNextClip();
                await PrepareNextClip();
                await UniTask.WaitUntil(waitUntil, cancellationToken: _cancellationToken);
            }
        }

        private async UniTask PlayNextClip()
        {
            AudioClip clip = await TakeNextClip();
            _audioSource.PlayOneShot(clip);
        }

        private async UniTask<AudioClip> TakeNextClip()
        {
            AsyncOperationHandle<AudioClip> previousClipHandle = _currentClipHandle;
            _currentClipHandle = _nextClipHandle;

            if (previousClipHandle.IsValid())
            {
                _assetProvider.Release(previousClipHandle);
            }

            await _currentClipHandle.ToUniTask();
            return _currentClipHandle.Result;
        }

        private async UniTask PrepareNextClip()
        {
            AssetReferenceT<AudioClip> nextClipReference = TakeRandomReference();
            _nextClipHandle = _assetProvider.GetLoadingHandle<AudioClip>(nextClipReference);
            await _nextClipHandle.ToUniTask();
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