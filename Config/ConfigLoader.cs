using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TwigLib.Config;
using TwigLib.Entities;
using TwigLib.Entities.Generation;

namespace TwigLib.Content
{
    public class ConfigLoader
    {
        string m_root;

        public ConfigLoader(string root_path) 
        { 
            m_root = root_path;
        }

        public XmlReader GetFile(string path)
        {
            return XmlReader.Create(m_root + path);
        }

        public Dictionary<int, Tile> LoadTiles(string path)
        {
            var reader = GetFile(path);
            var loaded_tiles = IntermediateSerializer.Deserialize<TileDataSet>(reader, "ConfigDataStructures");

            var tile_set = new Dictionary<int, Tile>();

            // Return empty if file not found
            if (loaded_tiles == null)
                return tile_set;


            foreach (var tile in loaded_tiles.TileSet)
            {
                tile_set.Add(tile.ID, new Tile(tile, loaded_tiles.SourcePositionScaling));
            }

            return tile_set;
        }

        public TileMap LoadMap(string path, Dictionary<int, Tile> tile_sources)
        {
            var reader = GetFile(path);
            var loaded_map = IntermediateSerializer.Deserialize<MapData>(reader, "ConfigDataStructures");
            
            if(loaded_map == null)
            {
                return new TileMap(0, 0, tile_sources, Point.Zero);
            }

            TileMap new_map = new TileMap(loaded_map.Width, loaded_map.Height, tile_sources, loaded_map.CameraOffset(), loaded_map.TileSize);
            
            foreach(var layer in loaded_map.Layers)
            {
                new_map.AddLayer(layer.UnpackTiles());
            }

            return new_map;
        }

    }
}
