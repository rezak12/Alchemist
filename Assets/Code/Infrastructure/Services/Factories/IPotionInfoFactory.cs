﻿using System.Collections.Generic;
using Code.Logic.Potions;
using Code.StaticData;
using Code.StaticData.Ingredients;
using Cysharp.Threading.Tasks;

namespace Code.Infrastructure.Services.Factories
{
    public interface IPotionInfoFactory
    {
        UniTask<PotionInfo> CreatePotionInfoAsync(IEnumerable<IngredientData> ingredients);
    }
}