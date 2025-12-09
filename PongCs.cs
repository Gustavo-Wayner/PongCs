using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.VisualBasic;

namespace PongCs;

public enum GameState
{
    Playing,
    Paused
}


public class Pong : Game
{
    public float getMagnitude(Vector2 vec) { return (float)Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y); }
    public Vector2 getUnitVector(Vector2 vec) { return vec / getMagnitude(vec); }
    public Vector2 setMagnitude(Vector2 vec, float mag) { return getUnitVector(vec) * mag; }
    public bool wasPressed;

    private GameState state = GameState.Playing;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont font;
    private Vector2 ballMomentum;
    private float ballMaxSpeed;
    private float ballMinSpeed;
    private float Increment;

    Player player;
    Actor Ball;
    private int scoreP;
    private int scoreO;
    Actor Opponent;

    private Vector2 seek_force;
    private float MaxSeekForce;
    public Random rand;

    public Pong()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
        rand = new Random();
        ballMinSpeed = 6.0f;
        ballMaxSpeed = 30.0f;
        Increment = 1.0f;
        MaxSeekForce = 4.0f;

        ballMomentum = new Vector2(-ballMinSpeed, rand.Next(-4, 4));
        ballMomentum = getUnitVector(ballMomentum) * ballMinSpeed;
        scoreP = 0;
        scoreO = 0;
        wasPressed = false;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D barSprite = Content.Load<Texture2D>("bar");
        Texture2D ballSprite = Content.Load<Texture2D>("ball");

        player = new Player(new Vector2(10, 200), barSprite, 10);
        Opponent = new Actor(new Vector2(770, 200), barSprite, 10);
        Ball = new Actor(new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), ballSprite, 5);

        font = Content.Load<SpriteFont>("fonts/myFont");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        switch (state)
        {
            case GameState.Playing:
                player.Position.Y = Mouse.GetState().Y;
                if (Mouse.GetState().Y < 0) player.Position.Y = 0;
                if ((Mouse.GetState().Y + player.Sprite.Height * player.scale) > Window.ClientBounds.Height) player.Position.Y = Window.ClientBounds.Height - (player.Sprite.Height * player.scale);

                if ((Ball.Position.Y + ballMomentum.Y) < 0) ballMomentum.Y = -ballMomentum.Y;
                else if ((Ball.Position.Y + ballMomentum.Y + Ball.Sprite.Height * Ball.scale) > Window.ClientBounds.Height) ballMomentum.Y = -ballMomentum.Y;
                if (new Actor(Ball.Position + ballMomentum, Ball.Sprite, 5).hitbox.Intersects(player.hitbox))
                {
                    Vector2 bounce = Ball.Center - player.Center;
                    while (!Ball.hitbox.Intersects(player.hitbox))
                    {
                        Ball.Position += getUnitVector(ballMomentum);
                    }
                    ballMomentum = getUnitVector(bounce) * (getMagnitude(ballMomentum) + Increment);
                }
                else if (new Actor(Ball.Position + ballMomentum, Ball.Sprite, 5).hitbox.Intersects(Opponent.hitbox))
                {
                    Vector2 bounce = new Vector2(Ball.Center.X - Opponent.Center.X, Ball.Center.Y - Opponent.Center.Y);
                    while (!Ball.hitbox.Intersects(Opponent.hitbox))
                    {
                        Ball.Position += getUnitVector(ballMomentum);
                    }
                    ballMomentum = getUnitVector(bounce) * (getMagnitude(ballMomentum) + Increment);
                }
                seek_force = new Vector2(0, Ball.Center.Y - Opponent.Center.Y);

                if (getMagnitude(ballMomentum) > ballMaxSpeed)
                {
                    ballMomentum = setMagnitude(ballMomentum, ballMaxSpeed);
                }
                else if (getMagnitude(ballMomentum) < ballMinSpeed)
                {
                    ballMomentum = setMagnitude(ballMomentum, ballMinSpeed);
                }

                if (getMagnitude(seek_force) > MaxSeekForce) seek_force = setMagnitude(seek_force, MaxSeekForce);

                Opponent.Position += seek_force;
                Ball.Position += ballMomentum;

                if (Ball.Position.X < 0)
                {
                    ballMomentum = new Vector2(-ballMinSpeed, rand.Next(-4, 4));
                    ballMomentum = getUnitVector(ballMomentum) * ballMinSpeed;
                    Ball.Position = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
                    scoreO++;
                }
                else if (Ball.Position.X > Window.ClientBounds.Width)
                {
                    ballMomentum = new Vector2(-ballMinSpeed, rand.Next(-4, 4));
                    ballMomentum = getUnitVector(ballMomentum) * ballMinSpeed;
                    Ball.Position = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
                    scoreP++;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    if (!wasPressed) state = GameState.Paused;
                    wasPressed = true;
                }
                else { wasPressed = false; }

                break;

            case GameState.Paused:

                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    if (!wasPressed) state = GameState.Playing;
                    wasPressed = true;
                }
                else { wasPressed = false; }

                break;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        player.Draw(ref _spriteBatch);
        Ball.Draw(ref _spriteBatch);
        Opponent.Draw(ref _spriteBatch);
        _spriteBatch.DrawString(font, scoreP.ToString(), new Vector2(10, 10), Color.Black);
        _spriteBatch.DrawString(font, scoreO.ToString(), new Vector2(Window.ClientBounds.Width - 34, 10), Color.Black);


        _spriteBatch.End();

        base.Draw(gameTime);
    }
}