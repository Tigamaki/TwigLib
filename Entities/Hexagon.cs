using Microsoft.Xna.Framework;
using MLEM.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwigLib.Entities
{

    // https://www.redblobgames.com/grids/hexagons/
    // Hex uses Axial Co-ordinates, which is why XY still works.
    // It acts as the QR values, letting us imply S

    public class Hexagon : Tile
    {
        public Hexagon() {
            SetNeighbours();
            parent = this;
        }

        public Hexagon(Vector2 hex_position, int hex_layer = -1)
        {
            setPosition(hex_position, hex_layer);
            parent = this;
        }

        public Hexagon(Vector2 hex_position, Hexagon hex_parent, int hex_layer = -1)
        {
            setPosition(hex_position, hex_layer);
            parent = hex_parent;
        }

        protected List<Vector2> neighbours = new List<Vector2>();
        protected Hexagon parent;

        #region attribute getters
        public List<Vector2> Neighbours() { return neighbours; }
        public Vector2 Neighbour(int index) { return neighbours[Math.Clamp(index, 0, 5)]; }
        public bool HasParent() { return parent != this; }
        public Hexagon Parent() { return parent; }
        
        // Calculates the number of tiles between this and [0,0,0]
        public int DistanceToZero()
        {
            int q = (int)position.X;
            int r = (int)position.Y;

            float abs_vector = Math.Abs(q) + Math.Abs(r) + Math.Abs(0 - q - r);

            return (int)(abs_vector / 2);
        }

        public Point DrawOffset(float scale)
        {
            Point scaledSize = tile_size.Multiply(scale);

            int hex_offset_x = (int)(scaledSize.X * 27/32);
            int hex_offset_y = (int)(scaledSize.Y * 0.75f);
            var hex_offset = new Point(hex_offset_x, hex_offset_y);

            Vector2 offset_v = Vector2.Zero;

            offset_v.X = (position.X + (position.Y / 2)) * hex_offset.X;
            offset_v.X -= (scaledSize.X / 2);

            offset_v.Y = (int)position.Y * hex_offset.Y;
            offset_v.Y -= (scaledSize.Y / 2);

            return new Point((int)offset_v.X, (int)offset_v.Y);
        }
        #endregion

        #region attribute setters
        new public void setPosition(Vector2 pos, int layer = -1)
        {
            position = pos;
            tile_layer = layer;
            SetNeighbours();
        }

        protected void SetNeighbours()
        {
            float q = position.X;
            float r = position.Y;

            neighbours = new List<Vector2>()
            {
                new Vector2(q, r-1),
                new Vector2(q+1, r-1),
                new Vector2(q+1, r),
                new Vector2(q, r+1),
                new Vector2(q-1, r+1),
                new Vector2(q-1, r),
            };
        }
        #endregion

    }
}
