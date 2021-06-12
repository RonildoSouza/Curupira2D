using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Input
{
    public class MouseInputManager : IInputManager
    {
        public MouseState MouseState { get; private set; } = Mouse.GetState();
        public MouseState OldMouseState { get; private set; } = Mouse.GetState();

        public void Begin() => MouseState = Mouse.GetState();

        public void End() => OldMouseState = MouseState;

        /// <summary>
        /// Gets if a Mouse Button is currently held down.
        /// </summary>
        /// <param name="button"><see cref="MouseButton"/></param>
        /// <returns>Boolean</returns>
        public bool IsMouseButtonDown(MouseButton button) => GetButtonState(MouseState, button) == ButtonState.Pressed;

        /// <summary>
        /// Gets if a Mouse Button is currently not being held.
        /// </summary>
        /// <param name="button"><see cref="MouseButton"/></param>
        /// <returns>Boolean</returns>
        public bool IsMouseButtonUp(MouseButton button) => GetButtonState(MouseState, button) == ButtonState.Released;

        /// <summary>
        /// Gets if a Mouse Button was pressed this frame
        /// </summary>
        /// <param name="button"><see cref="MouseButton"/></param>
        /// <returns>Boolean</returns>
        public bool IsMouseButtonPressed(MouseButton button) => GetButtonState(MouseState, button) == ButtonState.Pressed && GetButtonState(OldMouseState, button) == ButtonState.Released;

        /// <summary>
        /// Gets if a Mouse Button was released this frame.
        /// </summary>
        /// <param name="button"><see cref="MouseButton"/></param>
        /// <returns>Boolean</returns>
        public bool IsMouseButtonReleased(MouseButton button) => GetButtonState(MouseState, button) == ButtonState.Released && GetButtonState(OldMouseState, button) == ButtonState.Pressed;

        /// <summary>
        /// Gets the current X and Y position of the mouse.
        /// </summary>
        /// <returns>Return cursor position in <see cref="Point"/></returns>
        public Point GetPosition() => MouseState.Position;

        /// <summary>
        /// Gets the Total Mouse Scroll Wheel Value.
        /// </summary>
        /// <returns>Returns cumulative scroll wheel value since the game start</returns>
        public int GetScrollWheel() => MouseState.ScrollWheelValue;

        /// <summary>
        /// Gets how much the Scroll Wheel Value has changed this frame.
        /// </summary>
        /// <returns>Returns cumulative scroll wheel value</returns>
        public int GetScrollWheelChange() => MouseState.ScrollWheelValue - OldMouseState.ScrollWheelValue;

        ButtonState GetButtonState(MouseState state, MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => state.LeftButton,
                MouseButton.Middle => state.MiddleButton,
                MouseButton.Right => state.RightButton,
                MouseButton.XButton1 => state.XButton1,
                MouseButton.XButton2 => state.XButton2,
                _ => default,
            };
        }
    }

    public enum MouseButton
    {
        Left,
        Middle,
        Right,
        XButton1,
        XButton2,
    }
}
