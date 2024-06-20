using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using System;
using System.Diagnostics.Contracts;
using TwigLib.Config;
using TwigLib.Utilities;

namespace TwigLib.Entities.Generation
{
    public class TileMap
    {
        private List<TileGrid> m_layers;
        private int m_width_max;
        private int m_height_max;

        Dictionary<int, Tile> m_tile_sources;
        private Dictionary<string, Texture2D> m_tex_sheet;
        private Point m_tile_size;

        private Point m_cam_offset;

        public TileMap(int width, int height, Dictionary<int,Tile> sources, Point cam_offset, int tile_size = 16)
        {
            m_layers = new List<TileGrid>();

            // Constrain layers to the bounds of a map. Allows for consistency layouts.
            m_width_max = width;
            m_height_max = height;

            m_tile_sources = sources;

            m_tile_size = new Point(tile_size);

            m_cam_offset = cam_offset;
        }


        public void SetTextureSheets(Dictionary<string, Texture2D> tex_sheets)
        {
            m_tex_sheet = tex_sheets;
        }

        public void AddBackground(int tile_id)
        {
            var bg_grid = new TileGrid(m_width_max, m_height_max);

            //Create a one-tile BG.
            var bg_tiles = new List<List<int>>();
            for(int i = 0; i < m_width_max;i++)
            {
                var bg_row = new List<int>();
                for (int j = 0; j < m_width_max; j++)
                {
                    bg_row.Add(tile_id);
                }
                bg_tiles.Add(bg_row);
            }
            bg_grid.PopulateGrid(ref m_tile_sources, bg_tiles);

            m_layers.Add(bg_grid);
        }

        public void AddLayer(List<List<int>> tile_ids)
        {
            var new_layer = new TileGrid(m_width_max, m_height_max);
            new_layer.PopulateGrid(ref m_tile_sources, tile_ids);

            // Don't add the same layer twice.
            m_layers.Add(new_layer);

        }
        
        public int NumLayers() { return m_layers.Count; }
        public TileGrid Layer(int index) { return m_layers[index]; }
        public List<TileGrid> Layers() { return m_layers; }

        public int Width() { return m_width_max; }
        public int Height() { return m_height_max; }

        public Point TileSize() { return m_tile_size; }

        #region rendering
        public void DrawLayers(ref SpriteBatch spriteBatch)
        {
            for (int i = 0; i < NumLayers(); i++)
            {
                DrawLayer(ref spriteBatch, i);
            }
        }

        public void DrawLayer(ref SpriteBatch spriteBatch, int index)
        {

            // We want to shunt the map up and left by half its total size, so that the camera is centered on it.

            var layer = m_layers[index];

            for (int p_y = 0; p_y < layer.Height(); p_y++)
            {
                for (int p_x = 0; p_x < layer.Width(); p_x++)
                {
                    var xy_tile = layer.Fetch(new Point(p_x, p_y));
                    var t_size = xy_tile.TileSize();
                    var t_pos = (xy_tile.Position().ToPoint() * m_tile_size) + DrawOffset();
                    spriteBatch.Draw(m_tex_sheet[xy_tile.TileSheet()], new Rectangle(t_pos, m_tile_size), new Rectangle(xy_tile.SourcePosition(), t_size), xy_tile.TileColor());
                }
            }

            
        }
        #endregion

        public Point CameraOffset()
        {
            return m_cam_offset.Multiply(-1);
        }
        public Point DrawOffset()
        {
            return new Point(m_width_max, m_height_max) * m_tile_size.Divide(-2);
        }
        
        // Returns the "bounds" of the map, AKA how far from the edge of the screen it can be placed.
        // By default, half map size. may add flexibility
        public Point Bounds()
        {
            return new Point(
                m_width_max * m_tile_size.X / 2,
                m_height_max * m_tile_size.Y / 2
            );
        }

        // Returns the stack of tiles at a given co-ordinate, unless a specific layer is given
        public Dictionary<int, Tile> TilesAtCoordinate(Point position, int? layer = null)
        {
            Dictionary<int, Tile> return_dict = new Dictionary<int, Tile>();

            if (layer != null)
                return_dict.Add(layer.Value, m_layers[layer.Value].Fetch(position));
            else
            {
                for (var i = 0; i < m_layers.Count(); i++) {
                    return_dict.Add(i, m_layers[i].Fetch(position));
                }
            }

            return return_dict;

        }

    }
}
