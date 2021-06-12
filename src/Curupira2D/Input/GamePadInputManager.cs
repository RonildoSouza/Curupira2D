using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Input
{
    public class GamePadInputManager : IInputManager
    {
        public GamePadState GamePadState { get; private set; } = GamePad.GetState(PlayerIndex.One);
        public GamePadState OldGamePadState { get; private set; } = GamePad.GetState(PlayerIndex.One);

        public void Begin() => GamePadState = GamePad.GetState(PlayerIndex.One);

        public void End() => OldGamePadState = GamePadState;

        /// <summary>
        /// Returns if the Player One gamepad is connected or not.
        /// </summary>
        /// <returns>Boolean</returns>
        public bool IsGamepadConnected() => GamePadState.IsConnected;

        /// <summary>
        /// Determines if a Button is currently pressed down this frame
        /// </summary>
        /// <param name="button"><see cref="Buttons"/></param>
        /// <returns>Boolean</returns>
        public bool IsButtonDown(Buttons button) => GamePadState.IsButtonDown(button);

        /// <summary>
        /// Determines if a Button is currently not pressed this frame
        /// </summary>
        /// <param name="button"><see cref="Buttons"/></param>
        /// <returns>Boolean</returns>
        public bool IsButtonUp(Buttons button) => GamePadState.IsButtonUp(button);

        /// <summary>
        /// Determines if a Button was just pressed this frame.
        /// </summary>
        /// <param name="button"><see cref="Buttons"/></param>
        /// <returns>Boolean</returns>
        public bool IsButtonPressed(Buttons button) => GamePadState.IsButtonDown(button) && OldGamePadState.IsButtonUp(button);

        /// <summary>
        /// Determines if a Button was just released this frame.
        /// </summary>
        /// <param name="button"><see cref="Buttons"/></param>
        /// <returns>Boolean</returns>
        public bool IsButtonReleased(Buttons button) => GamePadState.IsButtonUp(button) && OldGamePadState.IsButtonDown(button);

        /// <summary>
        /// Gets how much the specified Trigger is currently compressed as a value between 0 and 1.
        /// </summary>
        /// <param name="trigger"><see cref="Triggers"/></param>
        /// <returns>A value from 0.0f to 1.0f</returns>
        public float GetTriggerState(Triggers trigger) => trigger == Triggers.Left ? GamePadState.Triggers.Left : GamePadState.Triggers.Right;

        /// <summary>
        /// Gets a Vector of how much the specified Thumbstick is pressed
        /// in the x and y direction.
        /// </summary>
        /// <param name="stick"><see cref="Thumbsticks"/></param>
        /// <returns>A <see cref="Vector2"/> indicating the current position of the stick</returns>
        public Vector2 GetThumbstickState(Thumbsticks stick) => stick == Thumbsticks.Left ? GamePadState.ThumbSticks.Left : GamePadState.ThumbSticks.Right;
    }

    public enum Triggers
    {
        Left,
        Right,
    }

    public enum Thumbsticks
    {
        Left,
        Right,
    }
}
