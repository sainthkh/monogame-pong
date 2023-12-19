using System;
using Microsoft.Xna.Framework;

namespace mg_pong;

public class BrickOnRail: Brick {
    public float Speed { get; set; }
    public float CoolTime { get; set; }

    public BrickOnRail(int x, int y, int width, int height = 15) : base(x, y, width, height) { 
        Speed = 5f;
        CoolTime = 10.0f;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        rec.X += (int)Speed;

        if (rec.X >= SharedResource.GameBounds.X + 30) {
            rec.X = 0;
        }
    }
}
