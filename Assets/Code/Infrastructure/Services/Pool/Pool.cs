using System.Collections.Generic;
using Code.Infrastructure.Services.Factories;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Code.Infrastructure.Services.Pool
{
    public class Pool<T> where T : MonoBehaviour
    {
        private readonly IPrefabFactory _factory;
        
        public PoolObjectType Type { get; private set; }
        private AssetReferenceGameObject _objectReference;
        private Transform _parent;
        
        private Stack<T> _entries;

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
            _entries = new Stack<T>(startCapacity);

            var tasks = new List<UniTask>(startCapacity);
            for (int i = 0; i < startCapacity; i++)
            {
                tasks.Add(AddObject());
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask<T> Get(Vector3 position)
        {
            if (_entries.Count == 0)
            {
                await AddObject();
            }

            T poolObject= _entries.Pop();
            
            poolObject.transform.position = position;
            poolObject.gameObject.SetActive(true);
            
            return poolObject;
        }

        public void Return(T poolObject)
        {
            poolObject.gameObject.SetActive(false);
            poolObject.transform.position = _parent.transform.position;
            
            _entries.Push(poolObject);
        }

        private async UniTask AddObject()
        {
            T newObject = await _factory.Create<T>(_objectReference);
            
            newObject.gameObject.SetActive(false);
            newObject.transform.SetParent(_parent);
            newObject.transform.position = _parent.transform.position;
            
            _entries.Push(newObject);
        }
    }
}