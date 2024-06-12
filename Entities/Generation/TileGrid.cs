using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwigLib.Entities.Generation
{
    public class TileGrid
    {
        
        private List<List<Tile>> m_tile_grid { get; set; }
        private Point m_grid_size { get; set; }

        public TileGrid(int width, int height)
        {
            m_tile_grid = new List<List<Tile>>();
            m_grid_size= new Point(width, height);

        }

        public void PopulateGrid()
        {
            m_tile_grid.Clear();
            for(int y = 0; y < m_grid_size.Y; y++)
            {
                List<Tile> row = new List<Tile>();
                for (int x = 0; x < m_grid_size.X; x++)
                {
                    var t = new Tile();
                    t.setPosition(new Vector2(x, y));
                    row.Add(t);

                }
                m_tile_grid.Add(row);
            }
        }
        public void PopulateGrid(ref Dictionary<int, Tile> tile_sources, List<List<int>> tile_ids)
        {
            m_tile_grid.Clear();
            for (int y = 0; y < m_grid_size.Y; y++)
            {
                List<Tile> row = new List<Tile>();
                for (int x = 0; x < m_grid_size.X; x++)
                {
                    int tile_id = -1;

                    if (tile_ids.Count() > y && tile_ids[y].Count() > x)
                        tile_id = tile_ids[y][x];

                    if (tile_sources.Any(t => t.Key == tile_id))
                    {
                        Tile tile_match = new Tile(tile_sources.FirstOrDefault(t => t.Key == tile_id).Value);
                        tile_match.setPosition(new Vector2(x, y));
                        row.Add(tile_match);
                    }
                    else
                    {
                        var t = new Tile();
                        t.setPosition(new Vector2(x, y));
                        row.Add(t);
                    }
                }
                m_tile_grid.Add(row);
            }
        }

        public int Width()  { return m_grid_size.X; }
        public int Height() { return m_grid_size.Y; }

        public Tile Fetch(Point position)
        {
            return m_tile_grid[position.Y][position.X];
        }

    }
}
