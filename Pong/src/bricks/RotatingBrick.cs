using System;
using Microsoft.Xna.Framework;

namespace mg_pong;

public class RotatingBrick: RegeneratableBrick {
    private float rotationSpeed = 0.4f;

    public float Rotation { get; set; }
    public Point Pivot { get; set; }
    public float Radius { get; set; }

    public RotatingBrick(int x, int y, int width, int height = 15) : base(x, y, width, height) { }

    public override void Update(GameTime gameTime) {
        base.Update(gameTime);

        rec.X = (int)(Pivot.X + Math.Cos(Rotation * Math.PI / 180) * Radius);
        rec.Y = (int)(Pivot.Y + Math.Sin(Rotation * Math.PI / 180) * Radius);

        Rotation += rotationSpeed;

        if (Rotation >= 360) {
            Rotation = 0;
        }
    }
}