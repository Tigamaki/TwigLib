using Microsoft.Xna.Framework;
using MLEM.Extensions;
using MonoGame.Framework.Content.Pipeline.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwigLib.Config
{
    public class MapData
    {
        public int MapIndex { get; set; } = -1;
        public string Name { get; set; }

        public int Width { get; set; } = 4;
        public int Height { get; set; } = 4;

        public int TileSize { get; set; } = 16;
        public float CamOffsetX { get; set; } = 0;
        public float CamOffsetY { get; set; } = 0;

        public List<MapLayerData> Layers { get; set; } = new List<MapLayerData>();

        public Point CameraOffset()
        {
            return new Vector2(CamOffsetX * TileSize, CamOffsetY * TileSize).ToPoint();
        }
    }
    public class MapLayerData
    {
        public int LayerIndex { get; set; } = -1;
        public string Tag { get; set; }
        public List<string> TileRows { get; set; } = new List<string>();

        public List<List<int>> UnpackTiles()
        {
            var _return = new List<List<int>>();
            foreach(var row in TileRows)
            {
                _return.Add(row.Split(',').Select(int.Parse).ToList());
            }
            return _return;
        }
    }

    public class TileDataSet
    {
        public int SourcePositionScaling { get; set; } = 1;
        public List<TileData> TileSet { get; set; } = new List<TileData>();
    }

    public class TileData
    {

        public int ID { get; set; } = 0;
        public string Name { get; set; } = "DEFAULT";

        //Source tilesheet 
        public string tile_sheet{ get; set;} = "blank";
        public int source_position_x{ get; set;} = 0;
        public int source_position_y{ get; set;} = 0;

        public int tile_layer{ get; set;} = -1;
        public int tile_size_x { get; set; } = 16;
        public int tile_size_y { get; set; } = 16;

        //Traversal values
        public bool traversible{ get; set;} = true;
        public int cost{ get; set;} = 1;

        //Tinting and transparency
        public string tile_color{ get; set;} = "255,255,255,1";
        public string alt_color{ get; set;} = "255,255,255,1";
        public string back_color { get; set;} = "255,255,255,1";

        public Color TileColor() { return FromXMLColor(tile_color); }
        public Color AltColor() { return FromXMLColor(alt_color); }
        public Color BackColor() { return FromXMLColor(back_color); }

        public Color FromXMLColor(string col_string)
        {
            var col_array = col_string.Split(',').Select(x => float.Parse(x)).ToList();
            return new Color(col_array[0], col_array[1], col_array[2], col_array[3]);
        }
    }
}
