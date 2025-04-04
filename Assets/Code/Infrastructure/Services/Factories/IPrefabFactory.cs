﻿using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPrefabFactory
    { 
        UniTask<TComponent> CreateAsync<TComponent>(string key, Vector3 position, Transform parent = null) 
            where TComponent : MonoBehaviour;
        UniTask<TComponent> CreateAsync<TComponent>(AssetReference reference, Vector3 position, Transform parent = null) 
            where TComponent : MonoBehaviour;
    }
}