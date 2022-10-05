using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzApps
{
    public class TerrainGenerator : MonoBehaviour
    {
        public BiomeGenerator biomeGenerator;
        
        public ChunkData ProcessChunkData(SOMapConfiguration mapConfiguration, ChunkData data, short seed)
        {
            for (int x = 0; x < data.chunkSize; x++)
            {
                for (int z = 0; z < data.chunkSize; z++)
                {
                    data = biomeGenerator.ProcessChunkColumn(mapConfiguration, data, x, z);
                }
            }
            return data;
        }
    }
}