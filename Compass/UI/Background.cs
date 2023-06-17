using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using ICities;
using System.Collections.Generic;
using System.Reflection;



namespace Compass.UI
{
	 public class Background
	 {
		  protected Texture2D m_texture;
		  protected int m_width;
		  protected int m_height;

		  public int width { get { return m_width; } }
		  public int height { get { return m_height; } }


		  public Texture2D[] textures
		  {
				get
				{
					 Texture2D[] array = new Texture2D[1];
					 array[0] = this.m_texture;
					 return array;
				}
				set
				{
					 m_texture = value[0];
				}
		  }

		  public UITextureAtlas.SpriteInfo spriteInfo
		  {
				get
				{
					 return new UITextureAtlas.SpriteInfo()
					 {
						  texture = m_texture,
						  name = m_texture.name,
						  border = new RectOffset()
					 };
				}

		  }

		  
		  public enum GradientDirection
		  {
				horizontal,
				vertical,
		  }

		  public enum borders
		  {
				top = 1,
				right = 2,
				bottom = 4,
				left = 8,
		  }

		  public Background(int width, int height, string name, Color color)
		  {
				m_texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
				m_texture.name = name;

				for (int x = 0; x < width; x++)
				{
					 for (int y = 0; y < height; y++)
					 {
						  m_texture.SetPixel(x, y, color);
					 }
				}

				m_texture.Apply();

				m_width = width;
				m_height = height;
		  }

		  public Background(int width, int height, string name, Color color, Color border, int borderWidth, borders borderPositions)
		  {
				m_texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
				m_texture.name = name;

				for (int x = 0; x < width; x++)
				{
					 for (int y = 0; y < height; y++)
					 {
						  if ((borderPositions & borders.top) != 0 && y < borderWidth
								|| (borderPositions & borders.bottom) != 0 && y >= height - borderWidth
								|| (borderPositions & borders.left) != 0 && x < borderWidth
								|| (borderPositions & borders.right) != 0 && x >= width - borderWidth
								)
						  {
								m_texture.SetPixel(x, y, border);
						  }
						  else
						  {
								m_texture.SetPixel(x, y, color);
						  }
					 }
				}

				m_texture.Apply();

				m_width = width;
				m_height = height;
		  }

		  public Background(int width, int height, string name, Gradient gradient, GradientDirection direction = 0f)
		  {
				m_texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
				m_texture.name = name;

				if(direction == GradientDirection.horizontal)
				{
					 for (int x = 0; x < width; x++)
					 {
						  Color color = gradient.Evaluate(x / width);

						  for (int y = 0; y < height; y++)
						  {
								m_texture.SetPixel(x, y, color);
						  }
					 }
				}
				else if( direction == GradientDirection.vertical)
				{
					 for (int y = 0; y < height; y++)
					 {
						  Color color = gradient.Evaluate(y / height);

						  for (int x = 0; x < width; x++)
						  {
								m_texture.SetPixel(x, y, color);
						  }
					 }
				}

				m_texture.Apply();

				m_width = width;
				m_height = height;
		  }
	 }

}
