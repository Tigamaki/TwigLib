using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TwigLib.Entities.Generation;

namespace TwigLib.Utilities
{
    public class MapManager
    {
        protected Dictionary<string, TileMap> m_map_list;
        protected string m_active_map;

        protected Point m_window_center;

        public MapManager(Dictionary<string, TileMap> map_list, string initial_map, Point window_size)
        {
            m_window_center = window_size.Divide(2);

            m_map_list = map_list;
            m_active_map = initial_map;
        }

        public void SaveMap(string name, TileMap map)
        {
            if(m_map_list.Any(x=>x.Key == name))
            {
                m_map_list[name] = map;
            }
            else
            {
                m_map_list.Add(name, map);
            }
        }

        public void SelectMap(string name) { 
            m_active_map = name;
        }

        public TileMap GetMap(string name = "")
        {
            if (name == "")
                name = m_active_map;

            return m_map_list[name];
        }

        public void Draw(ref SpriteBatch _spriteBatch)
        {
            m_map_list[m_active_map].DrawLayers(ref _spriteBatch);
        }

        public void DrawLayers(ref SpriteBatch _spriteBatch, int first = 0, int last = 1)
        {

            // If request is impossible, abort
            if (last < first || first < 0 || last >= m_map_list[m_active_map].NumLayers())
                return;

            for(int i = first; i < last; i++)
            {
                m_map_list[m_active_map].DrawLayer(ref _spriteBatch, i);
            }
            
        }

    }
}
