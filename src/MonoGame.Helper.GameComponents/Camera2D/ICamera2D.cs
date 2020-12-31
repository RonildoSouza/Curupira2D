﻿/*
 * https://manbeardgames.com/tutorials/2d-camera/
 * https://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite
 * http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Helper.GameComponents.Camera2D
{
    public interface ICamera2D : IGameComponent
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
        Vector2 Zoom { get; set; }

        /// <summary>
        /// Gets or Sets the origin point of the camera relative to the ViewPort
        /// </summary>
        Vector2 Origin { get; set; }

        /// <summary>
        /// Gets or Sets the camera's x-coordinate position relative to the world space of the game
        /// </summary>
        float X { get; set; }

        /// <summary>
        /// Gets or Sets the camera's y-coordinate position relative to the world space of the game
        /// </summary>
        float Y { get; set; }

        Matrix Projection { get; }
        Matrix View { get; }

        /// <summary>
        /// Translate the given screen space xy-coordinate position to the equivalent world space xy-coordinate position
        /// </summary>
        /// <param name="position">The xy-coordinate position in screen space to translate</param>
        /// <returns>The xy-coodinate position in world space</returns>
        Vector2 ScreenToWorld(Vector2 position);

        /// <summary>
        /// Translates the given world space xy-coordinate position to the equivalent screen space xy-coordinate position
        /// </summary>
        /// <param name="position">The xy-coordinate position in world space to translate</param>
        /// <returns>The xy-coordinate position in screen space</returns>
        Vector2 WorldToScreen(Vector2 position);

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects directly in the viewport
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns><c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.</returns>
        bool IsInView(Vector2 position, Texture2D texture);
    }
}
