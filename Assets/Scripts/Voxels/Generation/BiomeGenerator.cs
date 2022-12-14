using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzApps
{
    public class BiomeGenerator : MonoBehaviour
    {
        // Externals
        [SerializeField] private BlockLayers blockLayers;
        
        // Internals
        private BlockLayerHandler startLayer;
        
        public ChunkData ProcessChunkColumn(SOMapConfiguration mapConfiguration, ChunkData data, int x, int z)
        {
            mapConfiguration.noiseConfiguration.worldOffset = new Vector2Int { x = mapConfiguration.seed, y = mapConfiguration.seed };
            int groundPosition = GetSurfaceHeightNoise(mapConfiguration.noiseConfiguration, data.mapPosition.x + x, data.mapPosition.z + z, data.chunkHeight);
            startLayer = blockLayers.GetStartLayer(mapConfiguration);
            
            for (int y = 0; y < mapConfiguration.chunkHeight; y++)
            {
                startLayer.Handle(data, new Vector3Int(x, y, z), groundPosition,
                    mapConfiguration.noiseConfiguration.worldOffset);
                /*
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
                */
            }
            return data;
        }

        private int GetSurfaceHeightNoise(NoiseConfiguration config, int x, int z, int chunkHeight)
        {
            float terrainHeight = NoiseGenerator.OctavePerlin(x, z, config);
            terrainHeight = NoiseGenerator.Redistribution(terrainHeight, config);
            int surfaceHeight = NoiseGenerator.RemapValue01ToInt(terrainHeight, 0, chunkHeight);
            return surfaceHeight;
        }
    }
}

