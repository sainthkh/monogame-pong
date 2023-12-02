using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mg_pong;

class Button {
    private Rectangle rec;
    public Rectangle Rec { get { return rec; } }
    public Color Background { get; set; }
    public Color Color { get; set; }
    public string Text { get; set; }

    public delegate void OnClick();
    public event OnClick Click;

    public Button(Rectangle rec, string text) {
        this.rec = rec;
        Text = text;
    }

    public void Draw() {
        if (rec.Contains(Mouse.GetState().Position)) {
            Background = Color.DarkGray;
            Color = Color.White;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Click();
            }
        } else {
            Background = Color.White;
            Color = Color.Black;
        }

        DrawRectangle(rec, Background);
        SharedResource.SpriteBatch.DrawString(SharedResource.ButtonFont, Text, new Vector2(Rec.X + 50, Rec.Y + 15), Color);
    }

    private void DrawRectangle(Rectangle Rec, Color color)
    {
        Vector2 pos = new Vector2(Rec.X, Rec.Y);
        SharedResource.SpriteBatch.Draw(SharedResource.Texture, pos, Rec,
            color * 1.0f,
            0, Vector2.Zero, 1.0f,
            SpriteEffects.None, 0.00001f);
    }
}
