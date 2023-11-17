using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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

class Button {
    private Rectangle rec;
    public Rectangle Rec { get { return rec; } }
    public Color Background { get; set; }
    public Color Color { get; set; }
    public String Text { get; set; }

    public Button(Rectangle rec, String text) {
        this.rec = rec;
        Text = text;
    }
}

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Point GameBounds = new Point(450, 800); //window resolution

    public Texture2D Texture;
    private Color BackgroundColor = Color.Black;

    private SpriteFont font;
    private SpriteFont resultMessageFont;
    private SpriteFont buttonFont;

    private Scene currentScene;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = GameBounds.X;
        _graphics.PreferredBackBufferHeight = GameBounds.Y;
        IsMouseVisible = true;
        currentScene = new Level1();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        SharedResource.GraphicsDevice = GraphicsDevice;
        SharedResource.SpriteBatch = _spriteBatch;

        font = Content.Load<SpriteFont>("Content/Score");
        resultMessageFont = Content.Load<SpriteFont>("Content/GameResult");
        buttonFont = Content.Load<SpriteFont>("Content/ButtonText");

        Texture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
        Texture.SetData<Color>(new Color[] { Color.White });

        SharedResource.Font = font;
        SharedResource.Texture = Texture;
        SharedResource.SoundFX = new AudioSource();
        SharedResource.GameBounds = GameBounds;
        SharedResource.ResultMessageFont = resultMessageFont;
        SharedResource.ButtonFont = buttonFont;

        currentScene.Load();
    }

    protected override void Update(GameTime gameTime)
    {
        currentScene.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        currentScene.Draw(gameTime);

        base.Draw(gameTime);
    }
}
