﻿using Code.Logic.PotionMaking;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.Factories
{
    public interface IAlchemyTableFactory
    {
        public UniTask<AlchemyTableComponent> CreateTableAsync(Vector3 position);
    }
}