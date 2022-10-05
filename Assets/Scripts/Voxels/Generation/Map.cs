using System.Collections;
using System.Collections.Generic;
using JazzApps;
using UnityEngine;

namespace JazzApps
{
    public class Map : MonoBehaviour
    {
        // SOs
        public SOMapConfiguration config;
        
        // Externals
        [SerializeField] private TerrainGenerator terrainGenerator;

        // Internals
        private Dictionary<Vector3Int, ChunkData> chunkDatas = new Dictionary<Vector3Int, ChunkData>();
        private Dictionary<Vector3Int, ChunkRenderer> chunkRenderers = new Dictionary<Vector3Int, ChunkRenderer>();

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                config.seed = (short)Random.Range(short.MinValue, short.MaxValue);
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

            for (int x = 0; x < config.mapSizeInChunks; x++)
            {
                for (int z = 0; z < config.mapSizeInChunks; z++)
                {
                    ChunkData data = new ChunkData(config.chunkSize, config.chunkHeight, new Vector3Int(x * config.chunkSize, 0, z * config.chunkSize), this);
                    //GenerateBlocks(data);
                    ChunkData newData = terrainGenerator.ProcessChunkData(config, data, config.seed);
                    chunkDatas.Add(data.mapPosition, data);
                }
            }

            foreach (ChunkData data in chunkDatas.Values)
            {
                MeshData meshData = Chunk.GetChunkMeshData(data);
                GameObject chunkObject = Instantiate(config.chunkPrefab, data.mapPosition, Quaternion.identity);
                ChunkRenderer chunkRenderer = chunkObject.GetComponent<ChunkRenderer>();
                chunkRenderers.Add(data.mapPosition, chunkRenderer);
                chunkRenderer.InitializeChunk(data);
                chunkRenderer.UpdateChunk(meshData);
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
