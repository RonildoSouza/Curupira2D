using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Curupira2D.GameComponents.Camera2D
{
    public interface ICamera2D : IGameComponent, IDisposable
    {
        /// <summary>
        /// The Viewport reference for the camera
        /// </summary>
        Viewport Viewport { get; set; }

        /// <summary>
        /// Gets the cameras transformation matrix
        /// </summary>
        Matrix TransformationMatrix { get; }

        /// <summary>
        /// Gets the inverse of the camera's transformation matrix
        /// </summary>
        Matrix InverseMatrix { get; }

        /// <summary>
        /// Gets or Sets the xy-coordinate position of the camera relative to the world space of the game
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets or Sets the rotation angle of the camera
        /// </summary>
        float Rotation { get; set; }

        /// <summary>
        /// Gets or Sets the zoom level of the camera
        /// </summary>
        float Zoom { get; set; }

        /// <summary>
        /// Gets or Sets the origin point of the camera relative to the ViewPort
        /// </summary>
        Vector2 Origin { get; set; }

        /// <summary>
        /// Gets the projection matrix used to transform 3D coordinates into 2D screen space.
        /// </summary>
        Matrix Projection { get; }

        /// <summary>
        /// Gets the view matrix used to transform objects from world space to view space.
        /// </summary>
        Matrix View { get; }

        /// <summary>
        /// Use a BasicEffect to pass our view/projection in _spriteBatch
        /// </summary>
        BasicEffect SpriteBatchEffect { get; }

        /// <summary>
        /// Translate the given screen space xy-coordinate position to the equivalent world space xy-coordinate position
        /// </summary>
        /// <param name="position">The xy-coordinate position in screen space to translate</param>
        /// <returns>The xy-coodinate position in world space</returns>
        Vector2 ScreenToWorld(Vector2 position);

        /// <summary>
        /// Translate the given screen space xy-coordinate position to the equivalent world space xy-coordinate position
        /// </summary>
        /// <param name="x">The x coordinate position in screen space to translate</param>
        /// <param name="y">The y coordinate position in screen space to translate</param>
        /// <returns>The xy-coodinate position in world space</returns>
        Vector2 ScreenToWorld(float x, float y);

        /// <summary>
        /// Translates the given world space xy-coordinate position to the equivalent screen space xy-coordinate position
        /// </summary>
        /// <param name="position">The xy-coordinate position in world space to translate</param>
        /// <returns>The xy-coordinate position in screen space</returns>
        Vector2 WorldToScreen(Vector2 position);

        /// <summary>
        /// Translates the given world space xy-coordinate position to the equivalent screen space xy-coordinate position
        /// </summary>
        /// <param name="position">The xy-coordinate position in world space to translate</param>
        /// <param name="x">The x coordinate position in screen space to translate</param>
        /// <param name="y">The y coordinate position in screen space to translate</param>
        Vector2 WorldToScreen(float x, float y);

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects directly in the viewport
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="width">The texture width.</param>
        /// <param name="height">The texture height.</param>
        /// <returns><c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.</returns>
        bool IsInView(Vector2 position, int width, int height);

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects directly in the viewport
        /// </summary>
        /// <param name="x">The position x.</param>
        /// <param name="y">The position y.</param>
        /// <param name="width">The texture width.</param>
        /// <param name="height">The texture height.</param>
        /// <returns><c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.</returns>
        bool IsInView(float x, float y, int width, int height);

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects directly in the viewport
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns><c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.</returns>
        bool IsInView(Vector2 position, Texture2D texture);

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects directly in the viewport
        /// </summary>
        /// <param name="x">The position x.</param>
        /// <param name="y">The position y.</param>
        /// <param name="texture">The texture.</param>
        /// <returns><c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.</returns>
        bool IsInView(float x, float y, Texture2D texture);

        /// <summary>
        /// Reset setters properties to default values
        /// </summary>
        void Reset();
    }
}
