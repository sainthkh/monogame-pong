using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mg_pong;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Point WindowSize = new Point(1200, 900); //window resolution
    private Point GameSize = new Point(450, 800);

    public Texture2D Texture;

    private SpriteFont font;
    private SpriteFont resultMessageFont;
    private SpriteFont buttonFont;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = WindowSize.X;
        _graphics.PreferredBackBufferHeight = WindowSize.Y;
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        SharedResource.GraphicsDevice = GraphicsDevice;
        SharedResource.SpriteBatch = _spriteBatch;
        Xna.GraphicsDevice = GraphicsDevice;
        Xna.SpriteBatch = _spriteBatch;

        font = Content.Load<SpriteFont>("Content/Score");
        resultMessageFont = Content.Load<SpriteFont>("Content/GameResult");
        buttonFont = Content.Load<SpriteFont>("Content/ButtonText");

        Texture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
        Texture.SetData<Color>(new Color[] { Color.White });

        SharedResource.Font = font;
        SharedResource.Texture = Texture;
        SharedResource.SoundFX = new AudioSource();
        SharedResource.GameBounds = GameSize;
        SharedResource.ResultMessageFont = resultMessageFont;
        SharedResource.ButtonFont = buttonFont;

        GameBounds.X = GameSize.X;
        GameBounds.Y = GameSize.Y;
        WindowBounds.X = WindowSize.X;
        WindowBounds.Y = WindowSize.Y;

        SceneManager.Load(SceneTypes.Game);
        SceneManager.CurrentScene.Load();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        SceneManager.CurrentScene.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        SceneManager.CurrentScene.Draw(gameTime);
    }
}
