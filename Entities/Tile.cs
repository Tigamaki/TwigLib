using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwigLib.Entities
{
    public class Tile
    {
        #region attributes
        protected Vector2 position;

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
            position = Vector2.Zero;
            tile_sheet = "Default";
            source_position = Point.Zero;

            tile_layer = -1;
            tile_size = new Point(1);

            traversible = true;
            cost = 1;

            tile_color = alt_color = back_color = Color.White;
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
        public void setTileData(string sheet_name, int tile_pos_x = 0, int tile_pos_y = 0, int tile_size = 16)
        {
            tile_sheet = sheet_name;
            source_position = new Point(tile_pos_x, tile_pos_y);
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
