using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzApps
{
    [CreateAssetMenu(fileName = "Block Data", menuName = "SOs/Map Configuration")]
    public class SOMapConfiguration : ScriptableObject
    {
        public short seed = 0;
        public int mapSizeInChunks;
        public int chunkSize = 16;
        public int chunkHeight = 100;
        public int waterLevel = 50;
        public float noiseScale = 0.03f;
        public GameObject chunkPrefab;
    }
}
