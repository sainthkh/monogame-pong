using Microsoft.Xna.Framework;

namespace mg_pong;

class Button {
    private Rectangle rec;
    public Rectangle Rec { get { return rec; } }
    public Color Background { get; set; }
    public Color Color { get; set; }
    public string Text { get; set; }

    public Button(Rectangle rec, string text) {
        this.rec = rec;
        Text = text;
    }
}
