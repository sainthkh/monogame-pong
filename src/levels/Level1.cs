using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mg_pong;

public class Level1: Scene {
    private bool GameEnded = false;

    private PaddleEnemy enemy;
    private PaddlePlayer player;

    private Ball ball;

    private List<Brick> bricks = new List<Brick>();

    private Color BackgroundColor = Color.Black;

    private int PointsEnemy;
    private int PointsPlayer;
    private int PointsPerGame = 7;

    private int JingleCounter = 0;

    public override void Load() {
        Point GameBounds = SharedResource.GameBounds;

        ball = new Ball();
        ball.X = GameBounds.X / 2;
        ball.Y = GameBounds.Y / 2;

        enemy = new PaddleEnemy();
        enemy.Ball = ball;
        player = new PaddlePlayer();

        PointsEnemy = 0; PointsPlayer = 0;
        JingleCounter = 0;

        int bricksPerRow = 6;
        int brickWidth = GameBounds.X / bricksPerRow;

        for (int i = 0; i < bricksPerRow; i++)
        {
            Brick brick = new Brick(i * brickWidth, 5, brickWidth);
            brick.Color = i % 2 == 0 ? Color.DarkGray : Color.Gray;
            bricks.Add(brick);
        }

        for (int i = 0; i < bricksPerRow; i++)
        {
            Brick brick = new Brick(i * brickWidth, GameBounds.Y - 20, brickWidth);
            brick.Color = i % 2 == 1 ? Color.DarkGray : Color.Gray;
            bricks.Add(brick);
        }
    }
    public override void Update(GameTime gameTime) {
        AudioSource SoundFX = SharedResource.SoundFX;
        Point GameBounds = SharedResource.GameBounds;
        Random Rand = SharedResource.Rand;

        if (GameEnded) {
            return;
        }

        #region Update Ball

        ball.Move();

        //check for collision with paddles
        if (enemy.Collides(ball))
        {
            int Paddle_Center = enemy.X + enemy.Width / 2;
            ball.Direction = new Vector2(
                (ball.X - Paddle_Center) / (enemy.Width / 2),
                ball.Direction.Y * -1.1f
            );

            ball.Y = enemy.Y + enemy.Height;
            SoundFX.PlayWave(220.0f, 50, WaveType.Sin, 0.3f);
        }
        if (player.Collides(ball))
        {
            int Paddle_Center = player.X + player.Width / 2;
            ball.Direction = new Vector2(
                (ball.X - Paddle_Center) / (player.Width / 2),
                ball.Direction.Y * -1.1f
            );
            ball.Y = player.Y - 10;
            SoundFX.PlayWave(220.0f, 50, WaveType.Sin, 0.3f);
        }

        // check for collision with bricks
        foreach (Brick brick in bricks)
        {
            if (brick.IsAlive && brick.Collides(ball))
            {
                brick.IsAlive = false;
                ball.DirectionY *= -1;
                SoundFX.PlayWave(220.0f, 50, WaveType.Sin, 0.3f);

                break;
            }
        }

        //bounce on screen
        if (ball.Y < 0) //point for right
        {
            ball.Y = 1;
            ball.DirectionY *= -1;
            PointsPlayer++;
            SoundFX.PlayWave(440.0f, 50, WaveType.Square, 0.3f);
        }
        else if (ball.Y > GameBounds.Y) //point for left
        {
            ball.Y = GameBounds.Y - 1;
            ball.DirectionY *= -1;
            PointsEnemy++;
            SoundFX.PlayWave(440.0f, 50, WaveType.Square, 0.3f);
        }

        if (ball.X < 0 + 10) //limit to minimum Y pos
        {
            ball.X = 10 + 1;
            ball.DirectionX *= -(1 + Rand.Next(-100, 101) * 0.005f);
        }
        else if (ball.X > GameBounds.X - 10) //limit to maximum Y pos
        {
            ball.X = GameBounds.X - 11;
            ball.DirectionX *= -(1 + Rand.Next(-100, 101) * 0.005f);
        }

        #endregion

        enemy.Control();
        player.Control();

        #region Check Win
        //Check for win condition, reset
        if (PointsEnemy >= PointsPerGame || PointsPlayer >= PointsPerGame)
        {
            GameEnded = true;
        }
        #endregion Check Win

        #region Play Reset Jingle
        //use jingle counter as a timeline to play notes
        JingleCounter++;

        int speed = 7;
        if (JingleCounter == speed * 1) { SoundFX.PlayWave(440.0f, 100, WaveType.Sin, 0.2f); }
        else if (JingleCounter == speed * 2) { SoundFX.PlayWave(523.25f, 100, WaveType.Sin, 0.2f); }
        else if (JingleCounter == speed * 3) { SoundFX.PlayWave(659.25f, 100, WaveType.Sin, 0.2f); }
        else if (JingleCounter == speed * 4) { SoundFX.PlayWave(783.99f, 100, WaveType.Sin, 0.2f); }
        //only play this jingle once
        else if (JingleCounter > speed * 4) { JingleCounter = int.MaxValue - 1; }
        #endregion Play Reset Jingle
    }
    public override void Draw(GameTime gameTime) {
        GraphicsDevice GraphicsDevice = SharedResource.GraphicsDevice;
        SpriteBatch _spriteBatch = SharedResource.SpriteBatch;
        SpriteFont font = SharedResource.Font;
        SpriteFont resultMessageFont = SharedResource.ResultMessageFont;
        SpriteFont buttonFont = SharedResource.ButtonFont;
        Point GameBounds = SharedResource.GameBounds;

        GraphicsDevice.Clear(BackgroundColor);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

        //draw dots down center
        int total = GameBounds.Y / 20;
        for (int i = 0; i < total; i++)
        {
            DrawRectangle(_spriteBatch, new Rectangle(5 + (i * 20), GameBounds.Y / 2 - 4, 8, 8), Color.White * 0.2f);
        }

        //draw paddles
        DrawRectangle(_spriteBatch, enemy.Rec, Color.White);
        DrawRectangle(_spriteBatch, player.Rec, Color.White);

        // draw bricks
        foreach (Brick brick in bricks)
        {
            if (brick.IsAlive)
            {
                DrawRectangle(_spriteBatch, brick.Rec, brick.Color);
            }
        }

        //draw ball
        DrawRectangle(_spriteBatch, ball.Rec, Color.White);

        _spriteBatch.DrawString(font, PointsEnemy.ToString(), new Vector2(GameBounds.X - 25, GameBounds.Y / 2 - 40), Color.White);
        _spriteBatch.DrawString(font, PointsPlayer.ToString(), new Vector2(15, GameBounds.Y / 2 + 20), Color.White);

        if (GameEnded)
        {
            bool playerWon = PointsPlayer > PointsEnemy;
            Color bgColor = playerWon ? Color.Blue : Color.Red;

            DrawRectangle(_spriteBatch, new Rectangle(0, 0, GameBounds.X, GameBounds.Y), bgColor * 0.8f);

            string text = playerWon ? "You Win!" : "You Lose!";
            _spriteBatch.DrawString(resultMessageFont, text, new Vector2(GameBounds.X / 2 - 120, GameBounds.Y / 2 - 180), Color.White);

            Button button = new Button(new Rectangle(GameBounds.X / 2 - 95, GameBounds.Y / 2 - 100, 200, 50), "Play Again");

            if (button.Rec.Contains(Mouse.GetState().Position))
            {
                button.Background = Color.DarkGray;
                button.Color = Color.White;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    // Reset();
                    GameEnded = false;
                }
            }
            else
            {
                button.Background = Color.White;
                button.Color = Color.Black;
            }

            DrawRectangle(_spriteBatch, button.Rec, button.Background);
            _spriteBatch.DrawString(buttonFont, button.Text, new Vector2(button.Rec.X + 50, button.Rec.Y + 15), button.Color);
        }

        _spriteBatch.End();
    }

    private void DrawRectangle(SpriteBatch sb, Rectangle Rec, Color color)
    {
        Vector2 pos = new Vector2(Rec.X, Rec.Y);
        sb.Draw(SharedResource.Texture, pos, Rec,
            color * 1.0f,
            0, Vector2.Zero, 1.0f,
            SpriteEffects.None, 0.00001f);
    }

    private void LimitPaddle(ref Rectangle Paddle)
    {
        //limit how far paddles can travel on Y axis so they dont exceed top or bottom
        if (Paddle.X < 10) { Paddle.X = 10; }
        else if (Paddle.X + Paddle.Width > SharedResource.GameBounds.X - 10)
        { Paddle.X = SharedResource.GameBounds.X - 10 - Paddle.Width; }
    }
}