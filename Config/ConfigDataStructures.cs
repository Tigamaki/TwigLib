using Microsoft.Xna.Framework;
using MLEM.Extensions;
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
        public int tile_size{ get; set;} = 16;

        //Traversal values
        public bool traversible{ get; set;} = true;
        public int cost{ get; set;} = 1;

        //Tinting and transparency
        public XMLColor tile_color{ get; set;} = new XMLColor();
        public XMLColor alt_color{ get; set;} = new XMLColor();
        public XMLColor back_color{ get; set;} = new XMLColor();
    }

    public class XMLColor
    {
        public int r { get; set; } 
        public int g { get; set; } 
        public int b { get; set; } 
        public float a { get; set; } 

        public XMLColor()
        {
        }
        public Color fromXML()
        {
            var rgba = new Color(r, g, b);
            return new Color(rgba, a);
        }
    }
}
