using System;
using System.Collections.Generic;

using Assets.Classes.Common.Enums;

namespace Assets.Classes.System.Common
{
    [Serializable]
    public class CropDescriptor
    {
        public CropSpecies Species;
        public CropFarmingMethod FarmingMethod;
        public CropSize Size;
        public PrefabCommonDescriptor ReadyPrefab;

        public CropDescriptor
        (
            CropSpecies species,
            CropFarmingMethod farmingMethod,
            CropSize size,
            PrefabCommonDescriptor readyPrefab
        )
        {
            Species = species;
            FarmingMethod = farmingMethod;
            Size = size;
            ReadyPrefab = readyPrefab;
        }

        public List<string> GetMaterials()
        {
            return ReadyPrefab.Materials;
        }

        public override string ToString()
        {
            return $"Crop descriptor for {Species}\n\nFarming Method: {FarmingMethod}\nSize: {Size}\n{ReadyPrefab}";
        }
    }
}
