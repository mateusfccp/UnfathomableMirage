using System;
using System.Globalization;
using System.IO;
using System.Linq;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UnfathomableMirage.Services;

namespace UnfathomableMirage;

public class MainGame : Game
{
    private RefractionGradientManager _refractionGradientManager;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _pixelTexture;
    private FontSystem _fontSystem;

    public MainGame()
    {
        _refractionGradientManager = new RefractionGradientManager();
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Window.AllowUserResizing = true;
        _graphics.HardwareModeSwitch = true;
        _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        _graphics.PreferMultiSampling = true;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        _pixelTexture.SetData([Color.White]);

        _fontSystem = new FontSystem();
        _fontSystem.AddFont(File.ReadAllBytes(@"Content/Fonts/Geneva.ttf"));
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.OemPlus))
        {
            _refractionGradientManager.LayerCount = Math.Min(48, _refractionGradientManager.LayerCount + 1);
        }
        else if (keyboardState.IsKeyDown(Keys.OemMinus))
        {
            _refractionGradientManager.LayerCount = Math.Max(1, _refractionGradientManager.LayerCount - 1);
        }
        else if (keyboardState.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        var refractionIndexFont = _fontSystem.GetFont(12);
        var layerHeight =
            (int)double.Ceiling((double)GraphicsDevice.Viewport.Height / _refractionGradientManager.LayerCount);
        for (var i = 0; i < _refractionGradientManager.LayerCount; i++)
        {
            var layerColor = Color.Lerp(Color.MidnightBlue, Color.OrangeRed,
                i / (float)_refractionGradientManager.LayerCount);
            layerColor.A = 30;
            var rect = new Rectangle(0, i * layerHeight, GraphicsDevice.Viewport.Width, layerHeight);
            _spriteBatch.Draw(_pixelTexture, rect, layerColor);
            _spriteBatch.DrawString(refractionIndexFont,
                _refractionGradientManager.Layers.ElementAt(i).ToString("F2"),
                new Vector2(4.0f, i * layerHeight + layerHeight / 2.0f - refractionIndexFont.FontSize / 2.0f),
                Color.DarkSlateGray);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}