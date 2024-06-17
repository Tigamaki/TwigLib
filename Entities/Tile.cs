using Microsoft.Xna.Framework;
using MLEM.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwigLib.Config;

namespace TwigLib.Entities
{
    public class Tile
    {
        #region attributes
        protected Vector2 position = Vector2.Zero;

        //Source tilesheet 
        protected string tile_sheet;
        protected Point source_position;

        protected int tile_layer;
        protected Point tile_size;

        //Traversal values
        protected bool traversible;
        protected int cost;

        //Tinting and transparency
        protected Color tile_color;
        protected Color alt_color;
        protected Color back_color; //Used to fill in space behind tile if m_tile_color alpha < 1.
        #endregion

        #region constructors
        public Tile() {
            tile_sheet = "Default";
            source_position = Point.Zero;

            tile_layer = -1;
            //default tile size is 16
            tile_size = new Point(16);

            traversible = true;
            cost = 1;

            tile_color = alt_color = back_color = Color.White;
        }
        
        // For Factory-ing from a template
        public Tile(Tile t)
        {
            position = t.Position();
            tile_sheet = t.TileSheet();
            source_position = t.SourcePosition();

            tile_layer = t.Layer();
            //default tile size is 16
            tile_size = t.TileSize();

            traversible = t.Traversible();
            cost = t.TraverseCost();

            tile_color = t.TileColor();
            alt_color = t.AltColor();
            back_color = t.BackColor();
        }

        // For Loader deserialization
        public Tile(TileData tile_data, int source_scaling = 1)
        {
            tile_sheet = tile_data.tile_sheet;
            source_position = new Point(tile_data.source_position_x, tile_data.source_position_y).Multiply(source_scaling);

            tile_layer = tile_data.tile_layer;
            //default tile size is 16
            tile_size = new Point(tile_data.tile_size_x, tile_data.tile_size_y);

            traversible = tile_data.traversible;
            cost = tile_data.cost;

            tile_color = tile_data.TileColor();
            alt_color = tile_data.AltColor();
            back_color = tile_data.BackColor();
        }

        #endregion

        #region attribute getters
        public Vector2 Position() { return position; }

        public string TileSheet() { return tile_sheet; }
        public Point SourcePosition() { return source_position; }

        public int Layer() { return tile_layer; }
        public Point TileSize() { return tile_size; }

        public bool Traversible() { return traversible; }
        public int TraverseCost() { return cost; }

        public Color TileColor() { return tile_color; }
        public Color AltColor() { return alt_color; }
        public Color BackColor() { return back_color; }
        #endregion

        #region attribute setters
        public void setPosition(Vector2 pos, int layer = -1)
        {
            position = pos;
            tile_layer = layer;
        }

        //Default tiles are 16x16. Base tiles are always square, use transparency for rectangles.
        public void setTileData(string sheet_name, int tile_pos_x = 0, int tile_pos_y = 0, int size = 16)
        {
            tile_sheet = sheet_name;
            source_position = new Point(tile_pos_x, tile_pos_y);
            tile_size = new Point(size);
        }

        public void setTraversalData(bool can_traverse = true, int move_cost = 1)
        {
            traversible = can_traverse;
            cost = move_cost;
        }

        public void setTinting(Color tint, Color alt, Color backfill)
        {
            tile_color = tint;
            if (tile_color.A < 1)
                back_color = backfill;
            else
                back_color = new Color(backfill, 0);

            alt_color = alt;
        }
        #endregion
    }
}
