using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace mg_pong;

public class Paddle {
    protected Rectangle rec;
    public Rectangle Rec { get { return rec; } }

    public int X { get { return rec.X; } set { rec.X = value; } }
    public int Y { get { return rec.Y; } set { rec.Y = value; } }

    public int Width { get { return rec.Width; } set { rec.Width = value; } }
    public int Height { get { return rec.Height; } set { rec.Height = value; } }

    public float Speed { get; set; }
    public float MaxSpeed { get; set; }

    public Paddle(int y) {
        const int WIDTH = 60;
        const int HEIGHT = 20;

        int x = SharedResource.GameBounds.X / 2 - WIDTH / 2;

        rec = new Rectangle(x, y, WIDTH, HEIGHT);
        Speed = 10.0f;
        MaxSpeed = 15f;
    }

    public virtual bool Collides(Ball ball) {
        return rec.Intersects(ball.Rec);
    }

    public void Move(int amount) {
        rec.X -= amount;

        LimitPaddle();
    }

    private void LimitPaddle()
    {
        //limit how far paddles can travel on Y axis so they dont exceed top or bottom
        if (rec.X < 10) { rec.X = 10; }
        else if (rec.X + rec.Width > SharedResource.GameBounds.X - 10)
        { rec.X = SharedResource.GameBounds.X - 10 - rec.Width; }
    }
}

public class PaddleEnemy: Paddle {
    public Ball Ball { get; set; }

    public PaddleEnemy(): base(0 + 50) {
    }

    public void Control() {
        Random Rand = SharedResource.Rand;

        int amount = Rand.Next(0, 6);
        int Paddle_Center = rec.X + rec.Width / 2;
        
        if (Paddle_Center < Ball.X - 20) { rec.X += amount; }
        else if (Paddle_Center > Ball.X + 20) { rec.X -= amount; }
    }
}

public class PaddlePlayer: Paddle {
    public PaddlePlayer(): base(SharedResource.GameBounds.Y - 80) {
    }

    public void Control() {
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Left)) {
            Move(5);
        }
        else if (keyboardState.IsKeyDown(Keys.Right)) {
            Move(-5);
        }
    }
}
