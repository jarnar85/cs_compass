using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using ICities;
using System.Collections.Generic;
using System.Reflection;



namespace Compass.UI
{
	 public class Panel : UIPanel
	 {
		  protected Texture2D m_textureBackground;
		  protected string m_textureName;

		  public Panel(Texture2D texture, string name)
		  {
				m_textureBackground = texture;
				m_textureName = name;
		  }

		  public Panel(Background bg)
		  {
				width = bg.width;
				height = bg.height;

				m_textureBackground = bg.spriteInfo.texture;
				m_textureName = bg.spriteInfo.name;
		  }


		  public void setPanel(Texture2D texture, string name)
		  {
				m_textureBackground = texture;
				m_textureName = name;
		  }

		  public void setPanel(Background bg)
		  {
				width = bg.width;
				height = bg.height;

				m_textureBackground = bg.spriteInfo.texture;
				m_textureName = bg.spriteInfo.name;
		  }


		  protected override void OnRebuildRenderData()
		  {
				if ((Object)this.atlas == (Object)null)
					 return;

				if(!string.IsNullOrEmpty(this.backgroundSprite) )
				{
					 base.OnRebuildRenderData();
					 return;
				}

				if ((Object)this.m_textureBackground == (Object)null)
					 return;

				Shader shader = Shader.Find("UI/Default UI Shader");
				if (shader == null)
					 return;

				this.renderData.material = new Material(shader);

				UITextureAtlas.SpriteInfo atla = new UITextureAtlas.SpriteInfo()
				{
					 texture = m_textureBackground,
					 name = m_textureName,
					 border	 = new RectOffset(),
					 region	 = new Rect(0,0,width, height)
				};


				Color32 color32 = this.ApplyOpacity(this.isEnabled ? this.color : this.disabledColor);
				RenderOptions options = new RenderOptions()
				{
					 atlas			   = this.atlas,
					 color			   = color32,
					 fillAmount		   = 1f,
					 flip				   = this.m_Flip,
					 offset			   = this.pivot.TransformToUpperLeft(this.size, this.arbitraryPivotOffset),
					 pixelsToUnits	   = this.PixelsToUnits(),
					 size				   = this.size,
					 spriteInfo		   = atla
				};

				RenderSprite(this.renderData, options);
		  }

		  internal static void RenderSprite(UIRenderData data, RenderOptions options)
		  {
				options.baseIndex	   = data.vertices.Count;
				RebuildTriangles(data, options);
				RebuildVertices(data, options);
				RebuildUV(data, options);
				RebuildColors(data, options);
		  }

		  private static void RebuildTriangles(UIRenderData renderData, RenderOptions options)
		  {
				int baseIndex = options.baseIndex;
				PoolList<int> triangles = renderData.triangles;
				triangles.EnsureCapacity(triangles.Count + kTriangleIndices.Length);

				for (int index = 0; index < kTriangleIndices.Length; ++index)
					 triangles.Add(baseIndex + kTriangleIndices[index]);
		  }

		  private static void RebuildVertices(UIRenderData renderData, RenderOptions options)
		  {
				PoolList<Vector3> vertices = renderData.vertices;
				int baseIndex = options.baseIndex;
				float x1 = 0.0f;
				float y1 = 0.0f;
				float x2 = Mathf.Ceil(options.size.x);
				float y2 = Mathf.Ceil(-options.size.y);
				vertices.Add(new Vector3(x1, y1, 0.0f) * options.pixelsToUnits);
				vertices.Add(new Vector3(x2, y1, 0.0f) * options.pixelsToUnits);
				vertices.Add(new Vector3(x2, y2, 0.0f) * options.pixelsToUnits);
				vertices.Add(new Vector3(x1, y2, 0.0f) * options.pixelsToUnits);
				Vector3 vector3 = options.offset.RoundToInt() * options.pixelsToUnits;
				 
				for (int index = 0; index < 4; ++index)
					 vertices[baseIndex + index] = (vertices[baseIndex + index] + vector3).Quantize(options.pixelsToUnits);
		  }

		  private static void RebuildUV(UIRenderData renderData, RenderOptions options)
		  {
				Rect region = options.spriteInfo.region;
				PoolList<Vector2> uvs = renderData.uvs;
				uvs.Add(new Vector2(region.x, region.yMax));
				uvs.Add(new Vector2(region.xMax, region.yMax));
				uvs.Add(new Vector2(region.xMax, region.y));
				uvs.Add(new Vector2(region.x, region.y));
				Vector2 zero = Vector2.zero;

				if (options.flip.IsFlagSet(UISpriteFlip.FlipHorizontal))
				{
					 Vector2 vector2_1 = uvs[1];
					 uvs[1] = uvs[0];
					 uvs[0] = vector2_1;
					 Vector2 vector2_2 = uvs[3];
					 uvs[3] = uvs[2];
					 uvs[2] = vector2_2;
				}

				if (!options.flip.IsFlagSet(UISpriteFlip.FlipVertical))
					 return;

				Vector2 vector2_3 = uvs[0];
				uvs[0] = uvs[3];
				uvs[3] = vector2_3;
				Vector2 vector2_4 = uvs[1];
				uvs[1] = uvs[2];
				uvs[2] = vector2_4;
		  }

		  private static void RebuildColors(UIRenderData renderData, RenderOptions options)
		  {
				Color32 linear = (Color32)((Color)options.color).linear;
				PoolList<Color32> colors = renderData.colors;

				for (int index = 0; index < 4; ++index)
					 colors.Add(linear);
		  }


		  internal struct RenderOptions
		  {
				private UITextureAtlas m_Atlas;
				private UITextureAtlas.SpriteInfo m_SpriteInfo;
				private Color32 m_Color;
				private float m_PixelsToUnits;
				private Vector2 m_Size;
				private UISpriteFlip m_Flip;
				private bool m_InvertFill;
				private UIFillDirection m_FillDirection;
				private float m_FillAmount;
				private Vector3 m_Offset;
				private int m_BaseIndex;

				public UITextureAtlas atlas
				{
					 get { return this.m_Atlas; }
					 set { this.m_Atlas = value; }
				}

				public UITextureAtlas.SpriteInfo spriteInfo
				{
					 get { return this.m_SpriteInfo; }
					 set { this.m_SpriteInfo = value; }
				}

				public Color32 color
				{
					 get { return this.m_Color; }
					 set { this.m_Color = value; }
				}

				public float pixelsToUnits
				{
					 get { return this.m_PixelsToUnits; }
					 set { this.m_PixelsToUnits = value; }
				}

				public Vector2 size
				{
					 get { return this.m_Size; }
					 set { this.m_Size = value; }
				}

				public UISpriteFlip flip
				{
					 get { return this.m_Flip; }
					 set { this.m_Flip = value; }
				}

				public bool invertFill
				{
					 get { return this.m_InvertFill; }
					 set { this.m_InvertFill = value; }
				}

				public UIFillDirection fillDirection
				{
					 get { return this.m_FillDirection; }
					 set { this.m_FillDirection = value; }
				}

				public float fillAmount
				{
					 get { return this.m_FillAmount; }
					 set { this.m_FillAmount = value; }
				}

				public Vector3 offset
				{
					 get { return this.m_Offset; }
					 set { this.m_Offset = value; }
				}

				public int baseIndex
				{
					 get { return this.m_BaseIndex; }
					 set { this.m_BaseIndex = value; }
				}
		  }

		  internal static readonly int[] kTriangleIndices = new int[6]
		  {
				0,
				1,
				3,
				3,
				1,
				2
		  };

	 }

}
