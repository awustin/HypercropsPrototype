using System;
using System.Collections.Generic;

namespace Assets.Hypercrops.Common.Serializables
{
    [Serializable]
    public class PrefabCommonDescriptor
    {
        public string MeshCollider;
        public List<string> Materials;

        public PrefabCommonDescriptor()
        {
            MeshCollider = "";
            Materials = new();
        }

        public override string ToString()
        {
            return $"Materials loaded: {Materials.Count}\nMesh Collider: {MeshCollider} ";
        }
    }
}
