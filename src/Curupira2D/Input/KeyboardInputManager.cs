using Microsoft.Xna.Framework.Input;

namespace Curupira2D.Input
{
    public class KeyboardInputManager : IInputManager
    {
        public KeyboardState KeyboardState { get; private set; } = Keyboard.GetState();
        public KeyboardState OldKeyboardState { get; private set; } = Keyboard.GetState();

        public void Begin() => KeyboardState = Keyboard.GetState();

        public void End() => OldKeyboardState = KeyboardState;

        /// <summary>
        /// Determines if a Key is currently pressed down
        /// </summary>
        /// <param name="key"><see cref="Keys"/></param>
        /// <returns>Boolean</returns>
        public bool IsKeyDown(Keys key) => KeyboardState.IsKeyDown(key);

        /// <summary>
        /// Determines if a Key is currently not pressed this frame
        /// </summary>
        /// <param name="key"><see cref="Keys"/></param>
        /// <returns>Boolean</returns>
        public bool IsKeyUp(Keys key) => KeyboardState.IsKeyUp(key);

        /// <summary>
        /// Determines if a Key was just pressed this frame
        /// </summary>
        /// <param name="key"><see cref="Keys"/></param>
        /// <returns>Boolean</returns>
        public bool IsKeyPressed(Keys key) => KeyboardState.IsKeyDown(key) && OldKeyboardState.IsKeyUp(key);

        /// <summary>
        /// Determines if a Key was just released this frame
        /// </summary>
        /// <param name="key"><see cref="Keys"/></param>
        /// <returns>Boolean</returns>
        public bool IsKeyReleased(Keys key) => KeyboardState.IsKeyUp(key) && OldKeyboardState.IsKeyDown(key);
    }
}
