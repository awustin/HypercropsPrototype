using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using Assets.Hypercrops.Common.Enums;

namespace Assets.Hypercrops.System.CommonSerializable
{
    [Serializable]
    public class CropDescriptor
    {
        [JsonProperty("Species")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CropSpecies Species;

        [JsonProperty("FarmingMethod")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public CropFarmingMethod FarmingMethod;

        [JsonProperty("Size")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
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
