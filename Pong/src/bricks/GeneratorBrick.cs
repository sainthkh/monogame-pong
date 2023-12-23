using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace mg_pong;

public class GeneratorBrick: Brick {
    public List<Brick> ToBeGenerated { get; set; }

    public GeneratorBrick(int x, int y, int width, int height = 15) : base(x, y, width, height) { 
        ToBeGenerated = new List<Brick>();
    }

    public override void OnHit(Ball ball)
    {
        base.OnHit(ball);

        foreach (var brick in ToBeGenerated) {
            brick.IsAlive = true;
        }
    }
}
