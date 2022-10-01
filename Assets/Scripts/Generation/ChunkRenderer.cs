using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JazzApps;
using UnityEditor;
using UnityEngine;

namespace JazzApps
{
    // Requires
    [RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshCollider))]
    public class ChunkRenderer : MonoBehaviour
    {
        // Externals
        public bool showGizmo = false;

        // Internals
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;
        private Mesh mesh;

        public ChunkData ChunkData { get; private set; }

        public bool ModifiedByPlayer
        {
            get { return ChunkData.modifiedByPlayer; }
            set { ChunkData.modifiedByPlayer = value; }
        }

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
            mesh = meshFilter.mesh;
        }

        public void InitializeChunk(ChunkData chunkData)
        {
            this.ChunkData = chunkData;
        }

        private void RenderMesh(MeshData meshData)
        {
            mesh.Clear();

            mesh.subMeshCount = 2;
            mesh.vertices = meshData.vertices.Concat(meshData.waterMesh.vertices).ToArray();

            mesh.SetTriangles(meshData.triangles.ToArray(), 0);
            mesh.SetTriangles(meshData.waterMesh.triangles.Select(val => val + meshData.vertices.Count).ToArray(), 1);

            mesh.uv = meshData.uv.Concat(meshData.waterMesh.uv).ToArray();
            mesh.RecalculateNormals();

            meshCollider.sharedMesh = null;
            var collisionMesh = new Mesh {
                vertices = meshData.colliderVertices.ToArray(),
                triangles = meshData.colliderTriangles.ToArray() };
            collisionMesh.RecalculateNormals();

            meshCollider.sharedMesh = collisionMesh;
        }

        public void UpdateChunk()
        {
            RenderMesh(Chunk.GetChunkMeshData(ChunkData));
        }

        public void UpdateChunk(MeshData data)
        {
            RenderMesh(data);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (showGizmo)
            {
                if (Application.isPlaying && ChunkData != null)
                {
                    if (Selection.activeObject == gameObject)
                        Gizmos.color = new Color(0, 1, 0, 0.4f);
                    else
                        Gizmos.color = new Color(0, 0, 0, 0f);
                    
                    Gizmos.DrawCube(transform.position + 
                        new Vector3(
                            ChunkData.chunkSize / 2f,
                            ChunkData.chunkHeight / 2f, 
                            ChunkData.chunkSize / 2f), 
                    new Vector3(
                            ChunkData.chunkSize, 
                            ChunkData.chunkHeight,
                            ChunkData.chunkSize));
                }
            }
        }
#endif
    }
}
