using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MLEM.Cameras;
using SharpFont;

namespace TwigLib.Utilities
{
    public class CameraManager
    {
        protected Vector2 default_position;

        protected float m_camera_scale;
        protected int m_camera_speed;

        protected Camera m_camera;
        protected GraphicsDevice m_renderer;

        public CameraManager(GraphicsDevice graphicsDevice, Vector2 default_offset, float default_scale = 1, int default_speed = 1)
        {
            m_camera = new Camera(graphicsDevice);
            default_position = default_offset;
            m_camera.Position = default_position;

            m_camera.Scale = m_camera_scale = default_scale;            

            m_camera_speed = default_speed;

            m_renderer = graphicsDevice;
        }

        #region attribute getters
        public Vector2 Position() { return m_camera.Position; }
        public float Scale() { return m_camera.Scale; }
        public int PanSpeed() { return m_camera_speed; }

        public Camera Camera() { return m_camera; }
        #endregion

        #region attribute setters
        public void MoveCamera(Vector2 distance)
        {
            // Camera moves opposite way to where we're panning
            // Add option to invert in each axis separately
            m_camera.Position -= distance;
        }
        public void ZoomCamera(int amt)
        {
            var scale = m_camera.Scale;
            var delta = Math.Clamp(amt, -0.1f, 0.1f);
            m_camera.Scale = Math.Clamp(scale + delta, 0.7f, 1.5f);

            // Camera shifts are inverted
            m_camera.Position = m_camera.Position * (scale / m_camera.Scale);
        }
        public void SetPanSpeed(int speed)
        {
            m_camera_speed = speed;
        }

        public void Reset()
        {
            m_camera.Position = default_position;
            m_camera.Scale = m_camera_scale;
        }
        #endregion

        #region expanded functions
        public void UpdateCamera(MouseState prev_state, MouseState current_state)
        {
            if (current_state.RightButton == ButtonState.Pressed)
            {
                // Camera inverts movement internally
                MoveCamera((current_state.Position -prev_state.Position).ToVector2());
            }

            if (current_state.ScrollWheelValue != prev_state.ScrollWheelValue)
            {
                var delta = current_state.ScrollWheelValue - prev_state.ScrollWheelValue;
                ZoomCamera(delta);
            }
        }
        #endregion

    }
}