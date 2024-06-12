using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwigLib.Utilities
{
    public class Pathfinder_Grid
    {

        List<Node> m_open;
        List<Point> m_closed;
        List<Node> m_node_set;

        public Pathfinder_Grid() { 
        
            m_open = new List<Node>();
            m_closed = new List<Point>();
            m_node_set= new List<Node>();
        }

        public List<Point> AStar(Point origin, Point target, List<Point> valid_tiles)
        {

            m_open = new List<Node>();
            m_closed = new List<Point>();

            //valid tiles excludes any non-traversible ones.
            m_node_set = new List<Node>();
            valid_tiles.ForEach(t => m_node_set.Add(new Node(t, origin, target)));

            //Add start node to Open
            m_open.Add(m_node_set.Where(x => x.Location() == origin).First());

            bool path_found = false;
            while (!path_found)
            {
                if (m_open.Count == 0)
                {
                    path_found = true;
                    break;
                }

                var current = m_open.OrderBy(x => x.F_Total()).First();
                m_open.Remove(current);
                m_closed.Add(current.Location());

                // If target is found, exit loop.
                if (current.Location() == target)
                {
                    path_found = true;
                    break;
                }

                foreach (var n in Neighbours(current))
                {
                    int c_f_total = current.F_Total();

                    if (!m_open.Any(x => x.Location() == n.Location()))
                    {
                        m_open.Add(n);
                    }

                    var n_open = m_open.Where(x => x.Location() == n.Location()).FirstOrDefault();
                    if (c_f_total <= n_open.F_Total())
                    {
                        n_open.New_F_Cost(current.F_Cost());
                        n_open.Parent = current;
                    }

                }

            }

            var end_node = m_node_set.OrderBy(x => PointDistance(x.Location(), target)).First();
            var final_path = end_node.PathTo();
            return final_path;
        }

        protected List<Node> Neighbours(Node n)
        {
            var neighbours = new List<Node>();
            List<Point> cardinals = new List<Point>()
            {
                new Point(1, 0), //E
                new Point(-1, 0),//W
                new Point(0,1),  //N
                new Point(0,-1), //S
            };

            foreach (var c in cardinals)
            {
                Node c_node = m_node_set.Where(c_n => c_n.Location() == n.Location() + c).FirstOrDefault();
                if (c_node != null && !m_closed.Contains(c_node.Location()))
                    neighbours.Add(c_node);
            }

            return neighbours.OrderBy(x => x.F_Total()).ToList();
        }
        protected int PointDistance(Point first, Point last)
        {
            Point distance_xy = first - last;
            return Math.Abs(distance_xy.X) + Math.Abs(distance_xy.Y);
        }

    }
    public class Node
    {
        private Point m_location;

        private Point m_cost_G;
        private Point m_cost_H;

        //combined distance from start and end points;
        private Point m_cost_F;

        public Node Parent;

        public Node(Point location, Point start, Point end)
        {
            m_location = location;

            // G = distance from start
            m_cost_G = start - location;
            m_cost_G.X = Math.Abs(m_cost_G.X);
            m_cost_G.Y = Math.Abs(m_cost_G.Y);

            // H = distance from end
            m_cost_H = end - location;
            m_cost_H.X = Math.Abs(m_cost_H.X);
            m_cost_H.Y = Math.Abs(m_cost_H.Y);

            // F = G + H
            m_cost_F = m_cost_G + m_cost_H;

            // By default, node is only linked to itself.
            Parent = this;
        }

        public Point Location()
        {
            return m_location;
        }

        public Point G_Cost()
        {
            return m_cost_G;
        }

        //Gets distance from destination.
        public Point H_Cost()
        {
            return m_cost_H;
        }
        public Point F_Cost()
        {
            return m_cost_F;
        }
        public int F_Total()
        {
            return Math.Abs(m_cost_F.X) + Math.Abs(m_cost_F.Y);
        }
        public void New_F_Cost(Point value)
        {
            m_cost_F = value;
        }

        public List<Point> PathTo()
        {
            var path = new List<Point>();
            path.Add(m_location);
            if (Parent != this)
                path.AddRange(Parent.PathTo());


            return path;
        }
    }
}
