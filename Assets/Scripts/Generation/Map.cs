using System.Collections;
using System.Collections.Generic;
using JazzApps;
using UnityEngine;

namespace JazzApps
{
    public class Map : MonoBehaviour
    {
        public short seed = 0;
        public int mapSizeInChunks;
        public int chunkSize = 16, chunkHeight = 100;
        public int waterLevel = 50;
        public float noiseScale = 0.03f;
        public GameObject chunkPrefab;

        private Dictionary<Vector3Int, ChunkData> chunkDatas = new Dictionary<Vector3Int, ChunkData>();
        private Dictionary<Vector3Int, ChunkRenderer> chunkRenderers = new Dictionary<Vector3Int, ChunkRenderer>();

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                seed = (short)Random.Range(short.MinValue, short.MaxValue);
                GenerateMap();
            }
        }
        
        public void GenerateMap()
        {
            chunkDatas.Clear();
            foreach (ChunkRenderer chunk in chunkRenderers.Values)
            {
                Destroy(chunk.gameObject);
            }
            chunkRenderers.Clear();

            for (int x = 0; x < mapSizeInChunks; x++)
            {
                for (int z = 0; z < mapSizeInChunks; z++)
                {
                    ChunkData data = new ChunkData(chunkSize, chunkHeight, new Vector3Int(x * chunkSize, 0, z * chunkSize), this);
                    GenerateBlocks(data);
                    chunkDatas.Add(data.mapPosition, data);
                }
            }

            foreach (ChunkData data in chunkDatas.Values)
            {
                MeshData meshData = Chunk.GetChunkMeshData(data);
                GameObject chunkObject = Instantiate(chunkPrefab, data.mapPosition, Quaternion.identity);
                ChunkRenderer chunkRenderer = chunkObject.GetComponent<ChunkRenderer>();
                chunkRenderers.Add(data.mapPosition, chunkRenderer);
                chunkRenderer.InitializeChunk(data);
                chunkRenderer.UpdateChunk(meshData);
            }
        }
        
        private void GenerateBlocks(ChunkData data)
        {
            for (int x = 0; x < data.chunkSize; x++)
            {
                for (int z = 0; z < data.chunkSize; z++)
                {
                    float noiseValue = Mathf.PerlinNoise(
                        ((seed*noiseScale) + data.mapPosition.x + x) * noiseScale,
                        ((seed*noiseScale) + data.mapPosition.z + z) * noiseScale);
                    int groundPosition = Mathf.RoundToInt(noiseValue * chunkHeight);
                    for (int y = 0; y < chunkHeight; y++)
                    {
                        BlockType voxelType = BlockType.DIRT;
                        if (y > groundPosition)
                        {
                            if (y < waterLevel)
                            {
                                voxelType = BlockType.WATER;
                            }
                            else
                            {
                                voxelType = BlockType.AIR;
                            }
                        }
                        else if (y == groundPosition)
                        {
                            voxelType = BlockType.GRASS_DIRT;
                        }

                        Chunk.SetBlock(data, new Vector3Int(x, y, z), voxelType);
                    }
                }
            }
        }
        
        internal BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, Vector3Int pos)
        {
            Vector3Int blockChunkPos = Chunk.ChunkPositionFromBlockCoords(this, pos);
            ChunkData containerChunk = null;

            chunkDatas.TryGetValue(blockChunkPos, out containerChunk);

            if (containerChunk == null)
                return BlockType.NOTHING;
            Vector3Int blockInChunkCoordinates = Chunk.GetBlockInChunkCoordinates(containerChunk, pos);
            return Chunk.GetBlockFromChunkCoordinates(containerChunk, blockInChunkCoordinates);
        }
    }
}
