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
        protected Point default_position;

        protected int cam_height; //AKA Zoom Level
        protected int default_height;

        protected int camera_speed;
        protected int default_speed;

        public Camera()
        {
            Reset();
        }
        public Camera(int position_x, int position_y, int height = 1, int pan_speed = 1)
        {
            default_position = position = new Point(position_x, position_y);
            default_height = cam_height = height;
            default_speed = camera_speed = pan_speed;
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
            position = default_position;
            cam_height = default_height;
            camera_speed = default_speed;
        }
        #endregion

        #region expanded functions
        public void UpdatePosition(MouseState prev_state, MouseState current_state)
        {
            var cam_shift = current_state.Position - prev_state.Position;
            position += cam_shift;
        }
        public void ResetPosition()
        {
            position = default_position;
        }
        #endregion

    }
}