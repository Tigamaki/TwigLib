using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwigLib.Entities.Actor
{
    public class BaseActor
    {
        protected Point m_position;
        protected int m_layer;

        protected string m_name;

        public BaseActor(string name, Point start_position, int start_layer)
        {
            m_name = name;
            m_position = start_position;
            m_layer = start_layer;
        }

        public string Name() { return m_name; }

        public Point Position() { return m_position; }
        public int Layer() { return m_layer; } 

    }
}
