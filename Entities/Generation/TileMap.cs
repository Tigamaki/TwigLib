using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private Texture2D m_tex_sheet;
        private Point m_tile_size;

        private Utilities.Camera m_camera;


        public TileMap(int width, int height, Dictionary<int,Tile> sources, Point cam_offset, int tile_size = 16)
        {
            m_layers = new List<TileGrid>();

            // Constrain layers to the bounds of a map. Allows for consistency layouts.
            m_width_max = width;
            m_height_max = height;

            m_tile_sources = sources;
            m_camera = new Utilities.Camera(cam_offset.X,cam_offset.Y);

            m_tile_size = new Point(tile_size);
        }


        public void SetTextureSheet(Texture2D tex_sheet)
        {
            m_tex_sheet = tex_sheet;
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
            var layer = m_layers[index];
            for (int p_y = 0; p_y < layer.Height(); p_y++)
            {
                for (int p_x = 0; p_x < layer.Width(); p_x++)
                {
                    var xy_tile = layer.Fetch(new Point(p_x, p_y));
                    var t_size = xy_tile.TileSize();
                    var t_pos = (xy_tile.Position().ToPoint() * m_tile_size) + m_camera.Position();
                    spriteBatch.Draw(m_tex_sheet, new Rectangle(t_pos, m_tile_size), new Rectangle(xy_tile.SourcePosition(), t_size), xy_tile.TileColor());
                }
            }

            
        }
        #endregion

        public ref Camera Camera()
        {
            return ref m_camera;
        }

    }
}
