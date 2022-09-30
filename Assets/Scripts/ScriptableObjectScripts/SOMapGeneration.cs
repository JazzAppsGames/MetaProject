using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzApps
{
    [System.Serializable]
    public struct MapGenerationData
    {
        public short seed;
        public uint mapRadius;
        // "sea level" is 0.
        public uint maxTerrainHeight;
        public uint maxTerrainBottom;
    }
    
    /// <summary>
    /// SO data structure for procedural map generations.
    /// </summary>
    [CreateAssetMenu]
    public class SOMapGeneration : ScriptableObject
    {
        public MapGenerationData mapGenData;
        public List<GameObject> mapGenPrefabs; // TODO: Use addressables or something better? Prefabs is probably not optimal.
        // TODO: Create data presets
    }
}