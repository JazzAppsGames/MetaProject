using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzApps
{
    public class BiomeGenerator : MonoBehaviour
    {
        public ChunkData ProcessChunkColumn(SOMapConfiguration mapConfiguration, ChunkData data, int x, int z)
        {
            float noiseValue = Mathf.PerlinNoise(
                ((mapConfiguration.seed*mapConfiguration.noiseScale) + data.mapPosition.x + x) * mapConfiguration.noiseScale,
                ((mapConfiguration.seed*mapConfiguration.noiseScale) + data.mapPosition.z + z) * mapConfiguration.noiseScale);
            int groundPosition = Mathf.RoundToInt(noiseValue * mapConfiguration.chunkHeight);
            for (int y = 0; y < mapConfiguration.chunkHeight; y++)
            {
                BlockType voxelType = BlockType.DIRT;
                if (y > groundPosition)
                {
                    if (y < mapConfiguration.waterLevel)
                        voxelType = BlockType.WATER;
                    else
                        voxelType = BlockType.AIR;
                }
                else if (y < mapConfiguration.waterLevel)
                    voxelType = BlockType.SAND;
                if (y == groundPosition)
                {
                    if (y < mapConfiguration.waterLevel)
                        voxelType = BlockType.SAND;
                    else if (y >= mapConfiguration.waterLevel)
                        voxelType = BlockType.GRASS_DIRT;
                }

                Chunk.SetBlock(data, new Vector3Int(x, y, z), voxelType);
            }
            return data;
        }
    }
}

