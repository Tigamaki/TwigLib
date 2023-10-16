using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpFont;

namespace TwigLib.Utilities
{
    public class Camera
    {
        protected Point position;
        protected int cam_height; //AKA Zoom Level
        protected int camera_speed;

        public Camera()
        {
            Reset();
        }
        public Camera(int position_x, int position_y, int height = 1, int pan_speed = 1)
        {
            position = new Point(position_x, position_y);
            cam_height = height;
            camera_speed = pan_speed;
        }

        #region attribute getters
        public Point Position() { return position; }
        public int Height() { return cam_height; }
        public int PanSpeed() { return camera_speed; }
        #endregion

        #region attribute setters
        public void Move(int x, int y)
        {
            position.X += x;
            position.Y += y;
        }
        public void Raise(int amt)
        {
            cam_height += amt;
        }
        public void Lower(int amt)
        {
            cam_height -= amt;
        }
        public void SetPanSpeed(int speed)
        {
            camera_speed = speed;
        }

        public void Reset()
        {
            position = Point.Zero;
            cam_height = 0;
            camera_speed = 1;
        }
        #endregion

        #region expanded functions
        public void UpdatePosition(MouseState prev_state, MouseState current_state)
        {
            var cam_shift = current_state.Position - prev_state.Position;
            position += cam_shift;
        }
        #endregion

    }
}