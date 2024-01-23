using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace mg_pong;

public static class Render {
    private static Texture2D Texture;

    static Render() {
        Texture = SharedResource.Texture;
    }

    public static void Rectangle(Rectangle rect, Color color) {

        Vector2 pos = new Vector2(rect.X, rect.Y);
        Xna.SpriteBatch.Draw(SharedResource.Texture, pos, rect,
            color * 1.0f,
            0, Vector2.Zero, 1.0f,
            SpriteEffects.None, 0.00001f);
    }
}
