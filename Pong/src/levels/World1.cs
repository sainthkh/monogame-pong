using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mg_pong;

public class World1: Scene {
    private bool GameEnded = false;

    private PaddleEnemy enemy;
    private PaddlePlayer player;

    private Ball ball;

    protected List<Brick> bricks;

    private Color BackgroundColor = Color.Black;

    private int PointsEnemy;
    private int PointsPlayer;
    private int PointsPerGame = 7;

    private int JingleCounter = 0;

    private Button playAgainButton;
    private Button nextLevelButton;

    public override void Load() {
        Point GameBounds = SharedResource.GameBounds;
        var SoundFX = SharedResource.SoundFX;

        ball = new Ball();
        ball.X = GameBounds.X / 2;
        ball.Y = GameBounds.Y / 2;

        enemy = new PaddleEnemy();
        enemy.Ball = ball;
        player = new PaddlePlayer();

        PointsEnemy = 0; PointsPlayer = 0;
        JingleCounter = 0;

        bricks = new List<Brick>();

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

        AddMoreBlocks();

        playAgainButton = new Button(new Rectangle(GameBounds.X / 2 - 100, GameBounds.Y / 2 - 100, 200, 50), "Play Again");
        playAgainButton.Click += () => {
            Load();
            GameEnded = false;
        };

        nextLevelButton = new Button(new Rectangle(GameBounds.X / 2 - 100, GameBounds.Y / 2, 200, 50), "Next");
        nextLevelButton.Click += () => {
            OnClickNextLevel();
        };

        ball.HitTopWall += (Ball ball) => {
            PointsPlayer++;
            SoundFX.PlayWave(440.0f, 50, WaveType.Square, 0.3f);
        };

        ball.HitBottomWall += (Ball ball) => {
            PointsEnemy++;
            SoundFX.PlayWave(440.0f, 50, WaveType.Square, 0.3f);
        };

        ball.HitPlayer += (Ball ball, Paddle player) => {
            SoundFX.PlayWave(220.0f, 50, WaveType.Sin, 0.3f);
        };
    }

    public virtual void OnClickNextLevel() {}

    public override void Update(GameTime gameTime) {
        AudioSource SoundFX = SharedResource.SoundFX;
        Point GameBounds = SharedResource.GameBounds;
        Random Rand = SharedResource.Rand;

        if (GameEnded) {
            return;
        }

        #region Update Ball

        ball.Move();
        
        foreach (Brick brick in bricks)
        {
            brick.Update(gameTime);
        }

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
        ball.CheckPlayerCollision(player);

        // check for collision with bricks
        foreach (Brick brick in bricks)
        {
            if (brick.IsAlive && brick.Collides(ball))
            {
                brick.OnHit(ball);
                ball.DirectionY *= -1;
                SoundFX.PlayWave(220.0f, 50, WaveType.Sin, 0.3f);

                break;
            }
        }

        ball.CheckWallCollision();

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
            DrawTextCenter(_spriteBatch, text, resultMessageFont, GameBounds.Y / 2 - 180, Color.White);

            playAgainButton.Draw();

            if (playerWon)
            {
                nextLevelButton.Draw();
            }
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

    private void DrawTextCenter(SpriteBatch sb, string text, SpriteFont font, int y, Color color)
    {
        Vector2 size = font.MeasureString(text);
        Vector2 pos = new Vector2(SharedResource.GameBounds.X / 2 - size.X / 2, y);
        sb.DrawString(font, text, pos, color);
    }

    protected virtual void AddMoreBlocks() {}
}