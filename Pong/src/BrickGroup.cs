using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace mg_pong;

public abstract class BrickGroup {
    protected Rectangle bounds;

    public abstract int Top { get; set; }
    public abstract int Left { get; set; }

    public List<Brick2> Bricks { get; set; }
    public Rectangle Bounds { 
        get { return bounds; }
    }

    public BrickGroup() {
        Bricks = new List<Brick2>();
    }

    public virtual void Generate() {}
}

enum NewBrickSide {
    Left,
    Right,
    Top,
    Bottom,
    NONE,
}

public class TetrisBrickGroup: BrickGroup {
    public override int Top { 
        get { return bounds.Top; } 
        set { 
            int diff = value - bounds.Top;
            bounds.Y += diff;
            foreach(var brick in Bricks) {
                brick.Y += diff;
            }
        } 
    }

    public override int Left { 
        get { return bounds.Left; } 
        set { 
            int diff = value - bounds.Left;
            bounds.X += diff;
            foreach(var brick in Bricks) {
                brick.X += diff;
            }
        } 
    }

    public override void Generate()
    {
        int count = Xna.Rand.Next(3, 7);
        List<NewBrickSide> sides = new List<NewBrickSide>();

        NewBrickSide forbidden = NewBrickSide.NONE;

        for(int i = 0; i < count - 1; i++) {
            int side = Xna.Rand.Next(0, 4);

            while(side == (int)forbidden) {
                side = Xna.Rand.Next(0, 4);
            }

            sides.Add((NewBrickSide)side);

            if(side == (int)NewBrickSide.Left) {
                forbidden = NewBrickSide.Right;
            }
            else if(side == (int)NewBrickSide.Right) {
                forbidden = NewBrickSide.Left;
            }
            else if(side == (int)NewBrickSide.Top) {
                forbidden = NewBrickSide.Bottom;
            }
            else if(side == (int)NewBrickSide.Bottom) {
                forbidden = NewBrickSide.Top;
            }
        }

        List<Point> toplefts = new List<Point>();
        List<Point> bottomrights = new List<Point>();

        const int BRICK_SIZE = 16;

        Point loc = new Point(0, 0);
        Bricks.Add(new Brick2(new Rectangle(BRICK_SIZE * loc.X, BRICK_SIZE * (loc.Y + 1), BRICK_SIZE, BRICK_SIZE)));
        toplefts.Add(new Point(loc.X, loc.Y + 1));
        bottomrights.Add(new Point(loc.X + 1, loc.Y));    
        
        foreach(var side in sides) {
            if (side == NewBrickSide.Left) {
                loc.X -= 1;
            }
            else if (side == NewBrickSide.Right) {
                loc.X += 1;
            }
            else if (side == NewBrickSide.Top) {
                loc.Y -= 1;
            }
            else if (side == NewBrickSide.Bottom) {
                loc.Y += 1;
            }

            var brick = new Brick2(new Rectangle(BRICK_SIZE * loc.X, BRICK_SIZE * (loc.Y + 1), BRICK_SIZE, BRICK_SIZE));
            Bricks.Add(brick);
            toplefts.Add(new Point(loc.X, loc.Y + 1));
            bottomrights.Add(new Point(loc.X + 1, loc.Y));
        }

        int minX = toplefts.Min(p => p.X);
        int minY = toplefts.Min(p => p.Y);
        int maxX = bottomrights.Max(p => p.X);
        int maxY = bottomrights.Max(p => p.Y);

        bounds = new Rectangle(BRICK_SIZE * minX, BRICK_SIZE * minY, BRICK_SIZE * (maxX - minX), BRICK_SIZE * (maxY - minY));
    }
}
