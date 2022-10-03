using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JazzApps;
using UnityEngine;

namespace JazzApps
{
    public class BlockDataManager : MonoBehaviour
    {
        public static float textureOffset = 0.001f;
        public static float tileSizeX, tileSizeY;

        public static Dictionary<BlockType, TextureData> blockTextureDataDictionary;

        public SOBlockData SOBlockData;

        private void Awake()
        {
            // Initialization
            blockTextureDataDictionary = new Dictionary<BlockType, TextureData>();
            foreach (var item in SOBlockData.textureDatas)
            {
                blockTextureDataDictionary.Add(item.blockType, item);
            }
            /*
            foreach (var item in SOBlockData.textureDatas.Where(item => blockTextureDataDictionary.ContainsKey(item.blockType) == false))
                blockTextureDataDictionary.Add(item.blockType, item);
                */
            tileSizeX = SOBlockData.textureSizeX;
            tileSizeY = SOBlockData.textureSizeY;
        }
    }

    public static class BlockHelper
    {
        private static Direction[] directions =
        {
            Direction.left,
            Direction.right,
            Direction.up,
            Direction.down,
            Direction.forward,
            Direction.backward,
        };
        
        public static Vector2[] FaceUVs(Direction direction, BlockType blockType)
        {
            Vector2[] UVs = new Vector2[4];
            var tilePos = TexturePosition(direction, blockType);
            UVs[0] = new Vector2(
                BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.tileSizeX - BlockDataManager.textureOffset,
                BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.textureOffset);
            UVs[1] = new Vector2(
                BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.tileSizeX - BlockDataManager.textureOffset,
                BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.tileSizeY - BlockDataManager.textureOffset);
            UVs[2] = new Vector2(
                BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.textureOffset,
                BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.tileSizeY - BlockDataManager.textureOffset);
            UVs[3] = new Vector2(
                BlockDataManager.tileSizeX * tilePos.x + BlockDataManager.textureOffset,
                BlockDataManager.tileSizeY * tilePos.y + BlockDataManager.textureOffset);
            return UVs;
        }

        public static MeshData GetMeshData(ChunkData chunk, Vector3Int pos, MeshData meshData, BlockType blockType)
        {
            if (blockType == BlockType.AIR || blockType == BlockType.NOTHING)
                return meshData;

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var neighbourBlockCoordinates = pos + direction.GetVector();
                var neighbourBlockType = Chunk.GetBlockFromChunkCoordinates(chunk, neighbourBlockCoordinates);
                if (neighbourBlockType != BlockType.NOTHING && BlockDataManager.blockTextureDataDictionary[neighbourBlockType].isSolid == false)
                {
                    if (blockType == BlockType.WATER)
                    {
                        if (neighbourBlockType == BlockType.AIR)
                            meshData.waterMesh = GetFaceDataIn(direction, chunk, pos, meshData.waterMesh, blockType);
                    }
                    else
                    {
                        meshData = GetFaceDataIn(direction, chunk, pos, meshData, blockType);
                    }
                }
            }
            return meshData;
        }
        
        public static MeshData GetFaceDataIn(Direction direction, ChunkData chunk, Vector3Int pos, MeshData meshData, BlockType blockType)
        {
            GetFaceVertices(direction, pos, meshData, blockType);
            meshData.AddQuadTriangles(BlockDataManager.blockTextureDataDictionary[blockType].generatesCollider);
            meshData.uv.AddRange(FaceUVs(direction, blockType));
            return meshData;
        }
        
        public static void GetFaceVertices(Direction direction, Vector3Int pos, MeshData meshData, BlockType blockType)
        {
            var generatesCollider = BlockDataManager.blockTextureDataDictionary[blockType].generatesCollider;
            int x = pos.x;
            int y = pos.y;
            int z = pos.z;
            //order of vertices matters for the normals and how we render the mesh
            switch (direction)
            {
                case Direction.left:
                    meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                    break;
                case Direction.right:
                    meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                    break;
                case Direction.up:
                    meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                    break;
                case Direction.down:
                    meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                    break;
                case Direction.forward:
                    meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), generatesCollider);
                    break;
                case Direction.backward:
                    meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), generatesCollider);
                    meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), generatesCollider);
                    break;
                default:
                    break;
            }
        }

        public static Vector2Int TexturePosition(Direction direction, BlockType blockType)
        {
            return direction switch
            {
                Direction.up => BlockDataManager.blockTextureDataDictionary[blockType].up,
                Direction.down => BlockDataManager.blockTextureDataDictionary[blockType].down,
                _ => BlockDataManager.blockTextureDataDictionary[blockType].side
            };
        }
    }
}
