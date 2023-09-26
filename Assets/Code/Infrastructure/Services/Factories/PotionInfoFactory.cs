using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.Services.AssetProvider;
using Code.Logic.Potions;
using Code.StaticData;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Code.Infrastructure.Services.Factories
{
    public class PotionInfoFactory : IPotionInfoFactory
    {
        private readonly IAssetProvider _assetProvider;

        public PotionInfoFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask<PotionInfo> CreatePotionInfoAsync(IEnumerable<IngredientData> ingredients)
        {
            var characteristics = await CalculatePotionCharacteristicsAsync(ingredients);
            return new PotionInfo(characteristics);
        }
        
        private async UniTask<PotionCharacteristicAmountPair[]> CalculatePotionCharacteristicsAsync
            (IEnumerable<IngredientData> ingredients)
        {
            var groupedCharacteristics = GroupCharacteristics(ingredients);
            return await CreateCharacteristicAmountPairsAsync(groupedCharacteristics);;
        }

        private Dictionary<AssetReferenceT<PotionCharacteristic>, int> GroupCharacteristics(
            IEnumerable<IngredientData> ingredients)
        {
            var characteristicGuidAmountPairs = new Dictionary<string, int>();
            foreach (IngredientData ingredient in ingredients)
            {
                foreach (IngredientCharacteristicAmountPair pair in ingredient.CharacteristicAmountPairs)
                {
                    var characteristicGuid = pair.CharacteristicReference.AssetGUID;
                    var pointsAmount = pair.PointsAmount;

                    if (characteristicGuidAmountPairs.ContainsKey(characteristicGuid))
                    {
                        characteristicGuidAmountPairs[characteristicGuid] += pointsAmount;
                    }
                    else
                    {
                        characteristicGuidAmountPairs.Add(characteristicGuid, pointsAmount);
                    }
                }
            }

            var groupedCharacteristics = characteristicGuidAmountPairs
                .ToDictionary(pair => new AssetReferenceT<PotionCharacteristic>(pair.Key), pair => pair.Value);
                
            return groupedCharacteristics;
        }

        private async UniTask<PotionCharacteristicAmountPair[]> CreateCharacteristicAmountPairsAsync(
            Dictionary<AssetReferenceT<PotionCharacteristic>, int> groupedCharacteristics)
        {
            var characteristics = await UniTask.WhenAll(groupedCharacteristics
                .Select(pair => _assetProvider.LoadAsync<PotionCharacteristic>(pair.Key)));
            
            var pointAmountForEachPair = groupedCharacteristics.Select(pair => pair.Value).ToArray();

            var result = new PotionCharacteristicAmountPair[characteristics.Length];
            for (var i = 0; i < groupedCharacteristics.Count; i++)
            {
                PotionCharacteristic characteristic = characteristics[i];
                var pointsAmount = pointAmountForEachPair[i];
                result[i] = new PotionCharacteristicAmountPair(characteristic, pointsAmount);
            }
            return result;
        }
    }
}