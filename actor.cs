using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


public class Actor
{
    public Vector2 Position;
    public Texture2D Sprite;
    public int scale;
    public Vector2 Center
    {
        get
        {
            return Position + new Vector2(Sprite.Width * scale * 0.5f, Sprite.Height * scale * 0.5f);
        }
    }

    public Rectangle hitbox
    {
        get
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width * scale, Sprite.Height * scale);
        }
    }

    public Actor(Vector2 _Position, Texture2D _Sprite, int _scale = 1)
    {
        Position = _Position;
        Sprite = _Sprite;
        scale = _scale;
    }

    public void Draw(ref SpriteBatch _spriteBatch)
    {
        _spriteBatch.Draw(Sprite, new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width * scale, Sprite.Height * scale), Color.White);
    }
}

public class Player : Actor
{
    public Player(Vector2 _position, Texture2D _sprite, int _scale = 1) : base(_position, _sprite, _scale) { }
}