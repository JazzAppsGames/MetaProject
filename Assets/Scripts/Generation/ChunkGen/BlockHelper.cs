using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzApps
{
    public enum BlockType
    {
        NOTHING,
        AIR,
        GRASS_DIRT,
        DIRT,
        GRASS_STONE,
        STONE,
        TREE_TRUNK,
        TREE_LEAVES_TRANSPARENT,
        TREE_LEAVES_SOLID,
        WATER,
        SAND,
    }

    public enum Direction
    {
        left,
        right,
        up,
        down,
        forward,
        backward,
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
    
    public static class DirectionExtensions
    {
        public static Vector3Int GetVector(this Direction direction)
        {
            return direction switch
            {
                Direction.left => Vector3Int.left,
                Direction.right => Vector3Int.right,
                Direction.up => Vector3Int.up,
                Direction.down => Vector3Int.down,
                Direction.forward => Vector3Int.forward,
                Direction.backward => Vector3Int.back,
                _ => throw new Exception("Invalid Input Direction!")
            };
        }
    }

    public static class Chunk
    {
        public static void LoopThroughBlocks(ChunkData chunkData, Action<Vector3Int> actionToPerform)
        {
            for (int i = 0; i < chunkData.blocks.Length; i++)
            {
                var position = GetPositionFromIndex(chunkData, i);
                actionToPerform(position);
            }
        }

        private static Vector3Int GetPositionFromIndex(ChunkData chunkData, int i)
        {
            return new Vector3Int(
                i % chunkData.chunkSize, 
                (i / chunkData.chunkSize) % chunkData.chunkHeight, 
                i / (chunkData.chunkSize * chunkData.chunkHeight));
        }
        private static int GetIndexFromPosition(ChunkData chunkData, Vector3Int coordinates)
        {
            return coordinates.x + chunkData.chunkSize * coordinates.y + chunkData.chunkSize * chunkData.chunkHeight * coordinates.z;
        }
        
        private static bool InRange(ChunkData chunkData, int axisCoordinate)
        {
            return axisCoordinate >= 0 && axisCoordinate < chunkData.chunkSize;
        }
        private static bool InRangeHeight(ChunkData chunkData, int yCoordinate)
        {
            return yCoordinate >= 0 && yCoordinate < chunkData.chunkHeight;
        }
        
        public static BlockType GetBlockFromChunkCoordinates(ChunkData chunkData, Vector3Int coordinates)
        {
            if (InRange(chunkData, coordinates.x) && InRangeHeight(chunkData, coordinates.y) && InRange(chunkData, coordinates.z))
            {
                return chunkData.blocks[GetIndexFromPosition(chunkData, coordinates)];
            }

            return chunkData.mapReference.GetBlockFromChunkCoordinates(chunkData, chunkData.mapPosition + coordinates);
        }

        public static void SetBlock(ChunkData chunkData, Vector3Int localPosition, BlockType block)
        {
            if (InRange(chunkData, localPosition.x) && InRangeHeight(chunkData, localPosition.y) && InRange(chunkData, localPosition.z))
            {
                int i = GetIndexFromPosition(chunkData, localPosition);
                chunkData.blocks[i] = block;
            }
        }

        public static Vector3Int GetBlockInChunkCoordinates(ChunkData chunkData, Vector3Int pos)
        {
            return new Vector3Int
            {
                x = pos.x - chunkData.mapPosition.x,
                y = pos.y - chunkData.mapPosition.y,
                z = pos.z - chunkData.mapPosition.z,
            };
        }
        public static MeshData GetChunkMeshData(ChunkData chunkData)
        {
            MeshData meshData = new MeshData(true);

            LoopThroughBlocks(chunkData, 
                pos => meshData = BlockHelper.GetMeshData(chunkData, pos, meshData, chunkData.blocks[GetIndexFromPosition(chunkData, pos)]));
            
            return meshData;
        }

        internal static Vector3Int ChunkPositionFromBlockCoords(Map map, Vector3Int pos)
        {
            return new Vector3Int
            {
                x = Mathf.FloorToInt(pos.x / (float)map.chunkSize) * map.chunkSize,
                y = Mathf.FloorToInt(pos.y / (float)map.chunkHeight) * map.chunkHeight,
                z = Mathf.FloorToInt(pos.z / (float)map.chunkSize) * map.chunkSize,
            };
        }
    }
    
    public class ChunkData
    {
        public BlockType[] blocks;
        public int chunkSize = 16;
        public int chunkHeight = 100;
        public Map mapReference;
        public Vector3Int mapPosition;
        public bool modifiedByPlayer = false;

        public ChunkData(int chunkSize, int chunkHeight, Vector3Int mapPosition, Map mapReference)
        {
            this.chunkSize = chunkSize;
            this.chunkHeight = chunkHeight;
            this.mapPosition = mapPosition;
            this.mapReference = mapReference;
            blocks = new BlockType[chunkSize * chunkHeight * chunkSize];
        }
    }

    public class MeshData
    {
        public List<Vector3> vertices = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector2> uv = new List<Vector2>();

        public List<Vector3> colliderVertices = new List<Vector3>();
        public List<int> colliderTriangles = new List<int>();

        public MeshData waterMesh;
        private bool isMainMesh = true;

        public MeshData(bool isMainMesh)
        {
            if (isMainMesh)
            {
                waterMesh = new MeshData(false);
            }
        }

        public void AddVertex(Vector3 vertex, bool generatesCollider)
        {
            vertices.Add(vertex);
            if (generatesCollider)
                colliderVertices.Add(vertex);
        }

        public void AddQuadTriangles(bool generatesCollider)
        {
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);
            
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);

            if (generatesCollider)
            {
                colliderTriangles.Add(vertices.Count - 4);
                colliderTriangles.Add(vertices.Count - 3);
                colliderTriangles.Add(vertices.Count - 2);
            
                colliderTriangles.Add(vertices.Count - 4);
                colliderTriangles.Add(vertices.Count - 2);
                colliderTriangles.Add(vertices.Count - 1);
            }
        }
    }
}
