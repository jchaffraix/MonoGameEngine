using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace MonoGame.Engine
{
	static class Extensions
	{
		private static Texture2D _pixel;
		private static Texture2D _circle;

		public static void Initialize(GraphicsDevice device)
		{
			_pixel = new Texture2D(device, 1, 1);
			_pixel.SetData(new[] { Color.White });

			// Set up a circle in case.
			_circle = CreateCircle(device, 50);
		}

		public static Texture2D CreateCircle(GraphicsDevice device, int radius)
		{
			int diameter = 2 * radius;
			float radiusSq = radius * radius;
			Texture2D circle = new Texture2D(device, diameter, diameter);
			Color[] colorData = new Color[diameter * diameter];
			for (int i = 0; i < diameter; ++i)
			{
				for (int j = 0; j < diameter; ++j)
				{
					float distanceToCenter = MathF.Pow(i - radius, 2) + MathF.Pow(j - radius, 2);
					if (distanceToCenter > radiusSq)
					{
						colorData[i * diameter + j] = Color.Transparent;
					}
					else
					{
						colorData[i * diameter + j] = Color.White;
					}
				}
			}
			circle.SetData(colorData);
			return circle;
		}

		public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness = 2f)
		{
			Vector2 delta = end - start;
			spriteBatch.Draw(_pixel, start, null, color, delta.ToAngle(), new Vector2(0, 0.5f), new Vector2(delta.Length(), thickness), SpriteEffects.None, 0f);
		}

		public static void DrawStrokedRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float thickness = 2f)
		{
			Vector2 topLeft = new Vector2(rect.X, rect.Y);
			Vector2 topRight = new Vector2(rect.X + rect.Width, rect.Y);
			Vector2 bottomRight = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);
			Vector2 bottomLeft = new Vector2(rect.X, rect.Y + rect.Height);

			spriteBatch.DrawLine(topLeft, topRight, color, thickness);
			spriteBatch.DrawLine(topRight, bottomRight, color, thickness);
			spriteBatch.DrawLine(bottomRight, bottomLeft, color, thickness);
			spriteBatch.DrawLine(bottomLeft, topLeft, color, thickness);
		}

		public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color)
		{
			spriteBatch.Draw(_pixel, rect, null, color);
		}

		public static void DrawFullCircle(this SpriteBatch spriteBatch, Vector2 position, float radius, Color color)
		{
			float scale = 2f * radius / _circle.Width;
			spriteBatch.Draw(_circle, position, null, color, 0, new Vector2(_circle.Width / 2, _circle.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0);
		}
		public static void DrawStrokedCircle(this SpriteBatch spriteBatch, Vector2 position, float radius, Color color)
		{
			const int points = 20;
			const float angleIncrement = 2f * MathF.PI / points;
			// TODO: Avoid recomputing the same angle twice.
			for (int i = 0; i < points; ++i)
			{
				float startAngle = i * angleIncrement;
				float endAngle = (i + 1) * angleIncrement;
				spriteBatch.DrawLine(new Vector2(position.X + radius * MathF.Cos(startAngle), position.Y + radius * MathF.Sin(startAngle)), new Vector2(position.X + radius * MathF.Cos(endAngle), position.Y + radius * MathF.Sin(endAngle)), color);
			}
		}

		public static void DrawCapsuleWithEndCircles(this SpriteBatch spriteBatch, Vector2 startPoint, Vector2 endPoint, float radius, Color color, float thickness = 2f)
		{
			// A capsule is 2 lines with 2 circles.
			Vector2 line = endPoint - startPoint;
			// TODO: Use Vector2.Normalize instead of rolling this in?
			float distance = MathF.Sqrt(MathF.Pow(line.X, 2) + MathF.Pow(line.Y, 2));
			Vector2 normal = new Vector2(-line.Y, line.X) / distance;
			float offset = radius - thickness / 2f;
			spriteBatch.DrawLine(startPoint + offset * normal, endPoint + offset * normal, color);
			spriteBatch.DrawLine(startPoint - offset * normal, endPoint - offset * normal, color);

			float scale = 2f * radius / _circle.Width;
			spriteBatch.Draw(_circle, startPoint, null, color, 0, new Vector2(_circle.Width / 2, _circle.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0);
			spriteBatch.Draw(_circle, endPoint, null, color, 0, new Vector2(_circle.Width / 2, _circle.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0);
		}

		public static void DrawFullCapsule(this SpriteBatch spriteBatch, Vector2 startPoint, Vector2 endPoint, float radius, Color color, float thickness = 2f)
		{
			// A capsule is 2 lines with 2 circles.
			Vector2 line = endPoint - startPoint;
			// TODO: Use Vector2.Normalize instead of rolling this in?
			float distance = MathF.Sqrt(MathF.Pow(line.X, 2) + MathF.Pow(line.Y, 2));
			Vector2 normal = new Vector2(-line.Y, line.X) / distance;
			float offset = radius + thickness / 2f;
			// TODO: Can I ensure that this works?
			Vector2 startRect = startPoint + offset * normal;
			Vector2 endRect = endPoint - offset * normal;
			float startX = MathF.Min(startRect.X, endRect.X);
			float endX = MathF.Max(startRect.X, endRect.X);
			float startY = MathF.Min(startRect.Y, endRect.Y);
			float endY = MathF.Max(startRect.Y, endRect.Y);
			spriteBatch.DrawRectangle(new Rectangle((int)startX, (int)startY, (int)(endX - startX), (int)(endY - startY)), color);

			float scale = 2f * radius / _circle.Width;
			spriteBatch.Draw(_circle, startPoint, null, color, 0, new Vector2(_circle.Width / 2, _circle.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0);
			spriteBatch.Draw(_circle, endPoint, null, color, 0, new Vector2(_circle.Width / 2, _circle.Height / 2), new Vector2(scale, scale), SpriteEffects.None, 0);
		}

		public static void DrawStrokedCapsule(this SpriteBatch spriteBatch, Vector2 startPoint, Vector2 endPoint, float radius, Color color, float thickness = 2f)
		{
			// A capsule is 2 lines with 2 half circles.
			Vector2 line = endPoint - startPoint;
			line.Normalize();
			Vector2 normal = new Vector2(-line.Y, line.X);
			float offset = radius - thickness / 2f;
			spriteBatch.DrawLine(startPoint + offset * normal, endPoint + offset * normal, color);
			spriteBatch.DrawLine(startPoint - offset * normal, endPoint - offset * normal, color);

			// TODO: I need the formula for building the end circles based on the initial position and angle.
			// I thought a rotation would work but it has to be around the start/end point.
		}

		public static float ToAngle(this Vector2 vector)
		{
			return MathF.Atan2(vector.Y, vector.X);
		}
	}
}