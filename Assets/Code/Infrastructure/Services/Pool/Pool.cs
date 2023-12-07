using System.Collections.Generic;
using Code.Infrastructure.Services.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Pool
{
    public class Pool<TComponent> where TComponent : MonoBehaviour
    {
        private readonly IPrefabFactory _factory;
        
        public PoolObjectType Type { get; private set; }
        private AssetReferenceGameObject _objectReference;
        private Transform _parent;
        
        private Stack<TComponent> _entries;

        public Pool(IPrefabFactory factory)
        {
            _factory = factory;
        }

        public async UniTask InitializeAsync(
            AssetReferenceGameObject objectReference, 
            int startCapacity, 
            PoolObjectType type, 
            Transform parent)
        {
            _objectReference = objectReference;
            Type = type;
            _parent = parent;
            _entries = new Stack<TComponent>(startCapacity);

            var tasks = new List<UniTask>(startCapacity);
            for (int i = 0; i < startCapacity; i++)
            {
                tasks.Add(AddObject());
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask<TComponent> Get(Vector3 position)
        {
            if (_entries.Count == 0)
            {
                await AddObject();
            }

            TComponent poolObject= _entries.Pop();
            
            poolObject.transform.position = position;
            poolObject.gameObject.SetActive(true);
            
            return poolObject;
        }

        public void Return(TComponent poolObject)
        {
            poolObject.gameObject.SetActive(false);
            poolObject.transform.position = _parent.transform.position;
            
            _entries.Push(poolObject);
        }

        private async UniTask AddObject()
        {
            var newObject = await _factory.CreateAsync<TComponent>(_objectReference, _parent.transform.position, _parent);
            newObject.gameObject.SetActive(false);
            _entries.Push(newObject);
        }
    }
}