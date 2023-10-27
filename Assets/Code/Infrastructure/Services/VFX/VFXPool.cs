using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.FX
{
    public class VFXPool
    {
        private readonly VFX.Factory _factory;
        
        public VFXType Type { get; private set; }
        private AssetReferenceGameObject _objectReference;
        private Transform _parent;
        
        private Stack<VFX> _entries;

        public VFXPool(VFX.Factory factory)
        {
            _factory = factory;
        }

        public async UniTask InitializeAsync(
            AssetReferenceGameObject objectReference, 
            int startCapacity, 
            VFXType type, 
            Transform parent)
        {
            _objectReference = objectReference;
            Type = type;
            _parent = parent;
            _entries = new Stack<VFX>(startCapacity);

            var tasks = new List<UniTask>(startCapacity);
            for (int i = 0; i < startCapacity; i++)
            {
                tasks.Add(AddObject());
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask<VFX> Get(Vector3 position)
        {
            if (_entries.Count == 0)
            {
                await AddObject();
            }

            VFX poolObject= _entries.Pop();
            
            poolObject.transform.position = position;
            poolObject.gameObject.SetActive(true);
            
            return poolObject;
        }

        public void Return(VFX poolObject)
        {
            poolObject.gameObject.SetActive(false);
            poolObject.transform.position = _parent.transform.position;
            
            _entries.Push(poolObject);
        }

        private async UniTask AddObject()
        {
            VFX newObject = await _factory.Create(_objectReference);
            
            newObject.gameObject.SetActive(false);
            newObject.transform.SetParent(_parent);
            newObject.transform.position = _parent.transform.position;
            
            _entries.Push(newObject);
        }
    }
}