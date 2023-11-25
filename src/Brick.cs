using Microsoft.Xna.Framework;

namespace mg_pong;

public class Brick {
    private Rectangle rec;
    public Rectangle Rec { get { return rec; } }
    public Color Color { get; set; }
    public bool IsAlive { get; set; }

    public Brick(int x, int y, int width, int height = 15) {
        rec = new Rectangle(x, y, width, height);
        IsAlive = true;
    }

    public bool Collides(Ball ball) {
        return rec.Intersects(ball.Rec);
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void OnHit(Ball ball) {
        IsAlive = false;
    }
}
