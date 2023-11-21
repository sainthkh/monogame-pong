using Microsoft.Xna.Framework;

namespace mg_pong;

class Brick {
    private Rectangle rec;
    public Rectangle Rec { get { return rec; } }
    public Color Color { get; set; }
    public bool IsAlive { get; set; }

    public Brick(int x, int y, int width) {
        rec = new Rectangle(x, y, width, 15);
        IsAlive = true;
    }

    public bool Collides(Ball ball) {
        return rec.Intersects(ball.Rec);
    }
}
