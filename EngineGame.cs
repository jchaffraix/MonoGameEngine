using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame.Engine
{
    abstract public class EngineGame : Game
    {
        protected SpriteBatch _spriteBatch;

        // Used to build the map from scene name to actual scenes.
        protected abstract Dictionary<String, Scene> sceneMap();

        // Scene to start with.
        protected abstract String startingScene();

        protected override void Initialize()
        {
            SceneManager.Initialize(sceneMap(), startingScene());
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Extensions.Initialize(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            SceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            SceneManager.Draw(_spriteBatch, gameTime);
            base.Draw(gameTime);
        }
    }
}