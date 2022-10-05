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
}
