using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace mg_pong;

class Ball {
    private Rectangle rec;
    public Rectangle Rec { get { return rec; } }

    private Vector2 direction;
    public Vector2 Direction { 
        get
        {
            return direction;
        }

        set 
        {
            direction = value;
            direction.Normalize();
        }
    }

    public float X { get { return rec.X; } set { rec.X = (int)value; } }
    public float Y { get { return rec.Y; } set { rec.Y = (int)value; } }

    public float DirectionX {
        get { return direction.X; } 
        set { 
            direction.X = value; 
            direction.Normalize();
        } 
    }
    public float DirectionY {
        get { return direction.Y; } 
        set { 
            direction.Y = value; 
            direction.Normalize();
        }
    }

    public float Speed { get; set; }
    public float MaxSpeed { get; set; }

    public Ball() {
        rec = new Rectangle(0, 0, 10, 10);
        Direction = new Vector2(0.1f, 1f);
        Speed = 10.0f;
        MaxSpeed = 15f;
    }

    public void Move() {
        rec.X += (int)(Direction.X * Speed);
        rec.Y += (int)(Direction.Y * Speed);
    }
}

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Point GameBounds = new Point(450, 800); //window resolution

    private Rectangle PaddleTop;
    private Rectangle PaddleBottom;

    private Ball ball;

    public Texture2D Texture;
    private Color BackgroundColor = Color.Black;

    private Random Rand = new Random();
    private byte HitCounter = 0;

    private int PointsTop;
    private int PointsBottom;
    private int PointsPerGame = 4;

    private AudioSource SoundFX;
    private int JingleCounter = 0;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = GameBounds.X;
        _graphics.PreferredBackBufferHeight = GameBounds.Y;
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Reset();
    }

    protected override void Update(GameTime gameTime)
    {
#if !__IOS__
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
#endif
        #region Update Ball

        ball.Move();

        //check for collision with paddles
        HitCounter++;
        if (HitCounter > 10)
        {
            if (PaddleTop.Intersects(ball.Rec))
            {
                int Paddle_Center = PaddleTop.X + PaddleTop.Width / 2;
                ball.Direction = new Vector2(
                    (ball.X - Paddle_Center) / (PaddleTop.Width / 2),
                    ball.Direction.Y * -1.1f
                );

                HitCounter = 0;
                ball.Y = PaddleTop.Y + PaddleTop.Height;
                SoundFX.PlayWave(220.0f, 50, WaveType.Sin, 0.3f);
            }
            if (PaddleBottom.Intersects(ball.Rec))
            {
                int Paddle_Center = PaddleBottom.X + PaddleBottom.Width / 2;
                ball.Direction = new Vector2(
                    (ball.X - Paddle_Center) / (PaddleBottom.Width / 2),
                    ball.Direction.Y * -1.1f
                );
                HitCounter = 0;
                ball.Y = PaddleBottom.Y - 10;
                SoundFX.PlayWave(220.0f, 50, WaveType.Sin, 0.3f);
            }
        }

        //bounce on screen
        if (ball.Y < 0) //point for right
        {
            ball.Y = 1;
            ball.DirectionY *= -1;
            PointsBottom++;
            SoundFX.PlayWave(440.0f, 50, WaveType.Square, 0.3f);
        }
        else if (ball.Y > GameBounds.Y) //point for left
        {
            ball.Y = GameBounds.Y - 1;
            ball.DirectionY *= -1;
            PointsTop++;
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

        #region Simulate Left Paddle Input
        {   //simple ai, not very good, moves random amount each frame
            int amount = Rand.Next(0, 6);
            int Paddle_Center = PaddleTop.X + PaddleTop.Width / 2;
            if (Paddle_Center < ball.X - 20) { PaddleTop.X += amount; }
            else if (Paddle_Center > ball.X + 20) { PaddleTop.X -= amount; }
            LimitPaddle(ref PaddleTop);
        }
        #endregion Simulate Left Paddle Input

        #region Handle Player Paddle Input
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left))
            { PaddleBottom.X -= 5; }
            else if (state.IsKeyDown(Keys.Right))
            { PaddleBottom.X += 5; }

            LimitPaddle(ref PaddleBottom);
        }
        #endregion

        #region Check Win
        //Check for win condition, reset
        if (PointsTop >= PointsPerGame) { Reset(); }
        else if (PointsBottom >= PointsPerGame) { Reset(); }
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

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(BackgroundColor);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

        //draw dots down center
        int total = GameBounds.Y / 20;
        for (int i = 0; i < total; i++)
        {
            DrawRectangle(_spriteBatch, new Rectangle(5 + (i * 20), GameBounds.Y / 2 - 4, 8, 8), Color.White * 0.2f);
        }

        //draw paddles
        DrawRectangle(_spriteBatch, PaddleTop, Color.White);
        DrawRectangle(_spriteBatch, PaddleBottom, Color.White);

        //draw ball
        DrawRectangle(_spriteBatch, ball.Rec, Color.White);

        //draw current game points
        for (int i = 0; i < PointsTop; i++)
        {
            DrawRectangle(_spriteBatch, new Rectangle((GameBounds.X / 2 - 25) - i * 12, 10, 10, 10), Color.White * 1.0f);
        }
        for (int i = 0; i < PointsBottom; i++)
        {
            DrawRectangle(_spriteBatch, new Rectangle((GameBounds.X / 2 + 15) + i * 12, 10, 10, 10), Color.White * 1.0f);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawRectangle(SpriteBatch sb, Rectangle Rec, Color color)
    {
        Vector2 pos = new Vector2(Rec.X, Rec.Y);
        sb.Draw(Texture, pos, Rec,
            color * 1.0f,
            0, Vector2.Zero, 1.0f,
            SpriteEffects.None, 0.00001f);
    }

    private void LimitPaddle(ref Rectangle Paddle)
    {
        //limit how far paddles can travel on Y axis so they dont exceed top or bottom
        if (Paddle.X < 10) { Paddle.X = 10; }
        else if (Paddle.X + Paddle.Width > GameBounds.X - 10)
        { Paddle.X = GameBounds.X - 10 - Paddle.Width; }
    }

    private void Reset()
    {
        if (Texture == null)
        {   //create texture to draw with if it does not exist
            Texture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            Texture.SetData<Color>(new Color[] { Color.White });
        }

        int PaddleWidth = 60;
        PaddleTop = new Rectangle(150, 0 + 10, PaddleWidth, 20);
        PaddleBottom = new Rectangle(150, GameBounds.Y - 30, PaddleWidth, 20);

        ball = new Ball();
        ball.X = GameBounds.X / 2;
        ball.Y = GameBounds.Y / 200;

        PointsTop = 0; PointsBottom = 0;
        JingleCounter = 0;

        //setup sound sources
        if (SoundFX == null)
        {
            SoundFX = new AudioSource();
        }
    }
}
