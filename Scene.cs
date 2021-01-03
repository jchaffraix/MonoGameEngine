using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Engine
{
	// TODO: I probably need a scene manager singleton to allow transition between scenes!
	public abstract class Scene
	{
		protected Game _game;

		public Scene(Game game)
		{
			_game = game;
			Reset();
		}

		// TODO: Do I need to expose a function to preload some content?
		// Currently we load them in the constructor but is that fine?

		// Reset is called when the class is created and whenever the Scene is transitioned to afterwards.
		public abstract void Reset();

		public abstract void Update(GameTime gameTime);

		// TODO: Figure out a good abstraction to pass this SpriteBatch.
		// Passing it as an argument works but maybe we could have a base Game class.
		public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
	}

}