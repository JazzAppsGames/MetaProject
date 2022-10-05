using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzApps
{
    [CreateAssetMenu(fileName = "Map Configuration", menuName = "SOs/Map Configuration")]
    public class SOMapConfiguration : ScriptableObject
    {
        public short seed = 0;
        public int mapSizeInChunks;
        public int chunkSize = 16;
        public int chunkHeight = 100;
        public int waterLevel = 50;
        public float noiseScale = 0.03f;
        public NoiseConfiguration noiseConfiguration;
        public GameObject chunkPrefab;
    }

    [System.Serializable]
    public class NoiseConfiguration
    {
        public float noiseZoom = 0.01f;
        public int octaves = 5;
        public Vector2Int offset = new Vector2Int { x = -100, y = 3400 };
        public Vector2Int worldOffset = Vector2Int.zero;
        public float persistence = 0.5f;
        public float redistributionModifier = 1.2f;
        public float exponent = 5f;
    }
}
