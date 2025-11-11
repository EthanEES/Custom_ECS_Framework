using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Diagnostics;


using ECS_Framework.Components;
using ECS_Framework.ECS;
using ECS_Framework.Systems;

namespace Custom_ECS_GUI;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;

    private World world;
    private Texture2D particleTexture;
    private SpriteBatch spriteBatch;
    private SpriteFont font;
    private Random rnd = new Random();
    private int particlesPerFrame = 10;
    Stopwatch stopwatch = new Stopwatch();



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Create a new ECS world
        world = new World();

        // Add systems to the world.
        world.AddSystem(new MovementSystem());
        world.AddSystem(new LifetimeSystem());
        world.AddSystem(new SizeSystem());


    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        particleTexture = new Texture2D(GraphicsDevice, 8, 8);
        font = Content.Load<SpriteFont>("Arial");

        Color[] data = new Color[8 * 8];

        for (int i = 0; i < data.Length; ++i)
            data[i] = Color.White;

        particleTexture.SetData(data);
    }

    protected override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState keyState = Keyboard.GetState();
        
        world.Update(deltaTime);


        if (keyState.IsKeyDown(Keys.Up))
            particlesPerFrame++;
        if (keyState.IsKeyDown(Keys.Down) && particlesPerFrame > 0)
            particlesPerFrame--;

        for(int i = 0; i < particlesPerFrame; i++){
            var entity = world.CreateEntity();

            // Spread X randomly, Y starts at 0
            float x = rnd.Next(0, 800);

            // Velocity
            float vx = (float)(rnd.NextDouble() - 0.5) * 1.5f;
            float vy = rnd.Next(20, 40); 
            float lt = rnd.Next(1, 40); 

            world.GetPool<Position>().Add(entity, new Position { X = x, Y = -10 });
            world.GetPool<Velocity>().Add(entity, new Velocity { X = vx, Y = vy });
            world.GetPool<Lifetime>().Add(entity, new Lifetime { TimeLeft = lt });
            world.GetPool<Size>().Add(entity, new Size { Value = rnd.Next(1, 10) });
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);


        spriteBatch.Begin();
        var posPool = world.GetPool<Position>();
        var sizePool = world.GetPool<Size>();
        foreach (var entityId in posPool.AllEntityIds())
        {
            var entity = new ECS_Framework.ECS.Entity(entityId);
            var pos = posPool.Get(entity);
            var size = sizePool.Get(entity).Value;
            
            spriteBatch.Draw(particleTexture, new Rectangle((int)pos.X, (int)pos.Y, (int)size, (int)size), Color.White);
        }
        

        var metricsText = $"Active: {world.ActiveEntityCount}\nFree: {world.FreeEntityCount}\nDestroyed: {world.DestroyedEntityCount}\nSpawnrate: {particlesPerFrame}";
        var textSize = font.MeasureString(metricsText);

        var padding = 6;
        var bgRect = new Rectangle(4, 24, (int)textSize.X + padding * 2, (int)textSize.Y + padding * 2);
        spriteBatch.Draw(particleTexture, bgRect, Color.Black * 0.6f);

        spriteBatch.DrawString(font, metricsText, new Vector2(10, 30), Color.White);

        spriteBatch.End();

        base.Draw(gameTime);
    }
}
