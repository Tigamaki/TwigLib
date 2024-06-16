using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MLEM.Cameras;
using MLEM.Extensions;
using SharpFont;

namespace TwigLib.Utilities
{
    public class CameraManager
    {
        protected Vector2 default_position;
                
        protected float m_camera_scale;
        protected int m_camera_speed;

        protected Camera m_camera;
        protected GraphicsDeviceManager m_renderer;

        // Is camera bound to the level borders? off by default
        protected bool m_bound = false;
        protected Point m_bind_limits = Point.Zero;

        public CameraManager(GraphicsDeviceManager g_dm, Vector2 default_offset, float default_scale = 1, int default_speed = 1)
        {

            m_renderer = g_dm;
            m_camera = new Camera(g_dm.GraphicsDevice);
            default_position = default_offset;


            // Rather than setting scale to default, set to 1 then scale to default.
            m_camera.Scale = m_camera_scale = default_scale;

            m_camera.LookingPosition = default_position;
                                    
            m_camera_speed = default_speed;

        }

        #region attribute getters
        public Vector2 Position() { return m_camera.LookingPosition; }
        public float Scale() { return m_camera.Scale; }
        public int PanSpeed() { return m_camera_speed; }

        public Camera Camera() { return m_camera; }
        #endregion

        #region attribute setters
        public void MoveCamera(Vector2 distance)
        {
            // Camera moves opposite way to where we're panning
            // Add option to invert in each axis separately

            // Scale graphics_offset pre-emptively

            m_camera.LookingPosition -= distance;
            if (m_bound)
                BindClamp();


        }
        public void BindClamp()
        {
            var position = m_camera.LookingPosition;

            var scaled_binds = m_bind_limits;

            position.X = Math.Clamp(position.X, -scaled_binds.X, scaled_binds.X);
            position.Y = Math.Clamp(position.Y, -scaled_binds.Y, scaled_binds.Y);


            m_camera.LookingPosition = position;
        }


        public void ZoomCamera(int amt)
        {
            var old_scale = m_camera.Scale;
            var delta = Math.Clamp(amt, -0.1f, 0.1f);
            m_camera.Scale = Math.Clamp(old_scale + delta, 0.7f, 1.5f);

            // Camera shifts are inverted
            m_camera.Position *= (old_scale / m_camera.Scale);
            if(m_bound)
                BindClamp();



        }
        public void SetPanSpeed(int speed)
        {
            m_camera_speed = speed;
        }

        public void Reset()
        {
            m_camera.Scale = m_camera_scale;
            m_camera.LookingPosition = default_position;
        }

        public void BindCameraToLevel(Point level_bounds)
        {
            m_bound = true;
            // Bind Limits are 50% from each side, so halve it. 
            // Bind is for a scale of 1
            m_bind_limits = level_bounds;
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