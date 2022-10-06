using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzApps
{
    public abstract class BlockLayerHandler : MonoBehaviour
    {
        // Encapsulated Fields
        public BlockLayerHandler Next { set => _next = value; }
        private BlockLayerHandler _next;

        public bool Handle(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeedOffset)
        {
            if (TryHandling(chunkData, pos, surfaceHeightNoise, mapSeedOffset))
                return true;
            if (_next != null)
                return _next.Handle(chunkData, pos, surfaceHeightNoise, mapSeedOffset);
            return false;
        }

        public abstract bool TryHandling(ChunkData chunkData, Vector3Int pos, int surfaceHeightNoise, Vector2Int mapSeedOffset);
    }
}

