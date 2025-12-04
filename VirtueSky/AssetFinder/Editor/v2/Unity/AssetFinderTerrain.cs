using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{

    internal static class AssetFinderTerrain
    {
	    [Serializable] internal class TerrainTextureData
	    {
		    public Texture2D[] textures;
		    public TerrainTextureData(params Texture2D[] param)
		    {
			    var count = 0;
			    if (param != null) count = param.Length;

			    textures = new Texture2D[count];
			    for (var i = 0; i < count; i++)
			    {
				    textures[i] = param[i];
			    }
		    }
	    }
	    
	    internal static int ReplaceTerrainTextureDatas(TerrainData terrain, Texture2D fromObj, Texture2D toObj)
	    {
		    var found = 0;
#if UNITY_2018_3_OR_NEWER
		    TerrainLayer[] arr3 = terrain.terrainLayers;
		    for (var i = 0; i < arr3.Length; i++)
		    {
			    if (arr3[i].normalMapTexture == fromObj)
			    {
				    found++;
				    arr3[i].normalMapTexture = toObj;
			    }

			    if (arr3[i].maskMapTexture == fromObj)
			    {
				    found++;
				    arr3[i].maskMapTexture = toObj;
			    }

			    if (arr3[i].diffuseTexture == fromObj)
			    {
				    found++;
				    arr3[i].diffuseTexture = toObj;
			    }
		    }

		    terrain.terrainLayers = arr3;
#else
                    var arr3 = terrain.splatPrototypes;
                    for (var i = 0; i < arr3.Length; i++)
                    {
                        if (arr3[i].texture ==  fromObj)
                        {
                            found++;
                            arr3[i].texture = toObj;
                        }

                        if (arr3[i].normalMap ==  fromObj)
                        {
                            found++;
                            arr3[i].normalMap = toObj;
                        }
                    }

                    terrain.splatPrototypes = arr3;
#endif
		    return found;
	    }
	    
        internal static TerrainTextureData[] GetTerrainTextureDatas(TerrainData data)
        {
#if UNITY_2018_3_OR_NEWER
            if (data == null || data.terrainLayers == null)
            {
                return new TerrainTextureData[] { };
            }
            
            var arr = new TerrainTextureData[data.terrainLayers.Length];
            for (var i = 0; i < data.terrainLayers.Length; i++)
            {
                TerrainLayer layer = data.terrainLayers[i];
                arr[i] = layer == null ? new TerrainTextureData()
                    : new TerrainTextureData(
                        layer.normalMapTexture,
                        layer.maskMapTexture,
                        layer.diffuseTexture
                    );
            }

            return arr;
#else
			var arr = new TerrainTextureData[data.splatPrototypes.Length];
			for(int i = 0; i < data.splatPrototypes.Length; i++)
			{
				var layer = data.splatPrototypes[i];
				arr[i] = new TerrainTextureData
				(
					layer.normalMap,
					layer.texture
				);
			}
			return arr;
#endif
        }
    }
}