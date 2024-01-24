using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace mg_pong;

public static class Render {
    private static Texture2D Texture;
    private static SpriteFont Font;

    static Render() {
        Texture = SharedResource.Texture;
        Font = SharedResource.Font;
    }

    public static void LoadResources() {
        Texture = new Texture2D(Xna.GraphicsDevice, 1, 1);
        Texture.SetData<Color>(new Color[] { Color.White });

        Font = Xna.Content.Load<SpriteFont>("Content/Score");
    }

    public static void Rectangle(Rectangle rect, Color color) {

        Vector2 pos = new Vector2(rect.X, rect.Y);
        Xna.SpriteBatch.Draw(Texture, pos, rect,
            color * 1.0f,
            0, Vector2.Zero, 1.0f,
            SpriteEffects.None, 0.00001f);
    }

    public static void Text(string text, Vector2 pos, Color color) {
        Xna.SpriteBatch.DrawString(Font, text, pos, color);
    }
}
