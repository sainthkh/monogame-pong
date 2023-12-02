using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mg_pong;

public static class SharedResource
{
    public static GraphicsDevice GraphicsDevice;
    public static SpriteBatch SpriteBatch;
    public static Texture2D Texture;
    public static AudioSource SoundFX;
    public static Point GameBounds;
    public static SpriteFont Font;
    public static SpriteFont ResultMessageFont;
    public static SpriteFont ButtonFont;
    public static Random Rand = new Random();
}
