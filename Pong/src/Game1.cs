using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mg_pong;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Point GameBounds = new Point(450, 800); //window resolution

    public Texture2D Texture;

    private SpriteFont font;
    private SpriteFont resultMessageFont;
    private SpriteFont buttonFont;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = GameBounds.X;
        _graphics.PreferredBackBufferHeight = GameBounds.Y;
        IsMouseVisible = true;
        SceneManager.LoadLevel(7);
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
