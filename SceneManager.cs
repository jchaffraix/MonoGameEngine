using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoGame.Engine
{
	public class SceneManager
	{
		private static Dictionary<string, Scene> _map;
		private static Scene _scene;
		private static float _lastTransitionTimeMs;
		const float inputSnoozeTimeMs = 500;

		public static void Initialize(Dictionary<string, Scene> map, String scene)
		{
			_map = map;

			// Initial transitioning to a scene doesn't handle input disabling.
			bool exists = _map.TryGetValue(scene, out _scene);
			if (!exists)
			{
				throw new ApplicationException($"Can't find scene for {scene}, did you forget to define it?");
			}
			_scene.Reset();
		}

		public static void TransitionToScene(GameTime gameTime, string scene)
		{
			// TODO: Validate that we are not transitioning to the current scene?
			// Especially since we are resetting the entire scene.
			bool exists = _map.TryGetValue(scene, out _scene);
			if (!exists)
			{
				throw new ApplicationException($"Can't find scene for {scene}, did you forget to define it?");
			}
			_scene.Reset();
			_lastTransitionTimeMs = (float)gameTime.TotalGameTime.TotalMilliseconds;
		}

		public static void Update(GameTime gameTime)
		{
			float time = (float)gameTime.TotalGameTime.TotalMilliseconds;
			if (time > _lastTransitionTimeMs + inputSnoozeTimeMs)
			{
				_scene.Update(gameTime);
			}
		}

		// TODO: Figure out a good abstraction to pass this SpriteBatch.
		// Passing it as an argument works but maybe we could have a base Game class.
		public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			_scene.Draw(spriteBatch, gameTime);
		}
	}
}
