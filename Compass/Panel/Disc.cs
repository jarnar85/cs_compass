using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;

namespace Compass.Panel
{
	 public class Disc : UIPanel
	 {
		  private UILabel[] m_angleLabels  = new UILabel[16];
		  private UILabel m_directionLabel;
		  private UIDragHandle m_dragHandle;
		  private int currentPanelContent;
		  private bool currentAviationMode = false;
		  private bool is_active = true;
		  private float x_posAngle = 10;
		  private float[] m_positions = {180, 225, 270, 315, 360, 405, 450, 495 , 540, -135, -90, -45, 0, 45, 90, 135};
		  private string[] directions = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
												//    0,  22.5,   45,  67.5,  90, 112.5,  135, 157.5, 180, 202.5,  225, 247.5, 270, 292.5,  315, 337.5
												//    0,     1,    2,     3,   4,     5,    6,     7,   8,     9,   10,    11,  12,    13,   14,    15

		  public override void Start()
		  {
				base.Start();

				// Set up the UI
				width = 360;
				height = 50;
				backgroundSprite = "InfoPanel";
				color = new Color32(16, 16, 16, 128);
				int zOrder = 0;

				int panelDisplay = PlayerPrefs.GetInt("CompassPanelContent", 0);
				currentPanelContent = panelDisplay;

				bool isAviationMode = (PlayerPrefs.GetInt("CompassAviationMode", 0) == 1);
				currentAviationMode = isAviationMode;

				// Add Labels
				for(int i=0; i<16; i++)
				{
					 m_angleLabels[i] = AddUIComponent<UILabel>();


					 switch (panelDisplay)
					 {
						  case 0: // both
								if(i%4 == 0)
									 m_angleLabels[i].text = directions[i];
								else if(isAviationMode)
									 m_angleLabels[i].text = Mathf.Round(i * 360 / 160).ToString("F0");
								else
									 m_angleLabels[i].text = Mathf.Round(i * 360 / 16).ToString("F0");
								break;
						  case 1: // Angle Only
								if (isAviationMode)
									 m_angleLabels[i].text = Mathf.Round(i * 360 / 160).ToString("F0");
								else
									 m_angleLabels[i].text = Mathf.Round(i * 360 / 16).ToString("F0");
								break;
						  case 2: // Cardinal Direction Only
								m_angleLabels[i].text = directions[i];
								break;
					 }

					 m_angleLabels[i].textAlignment = UIHorizontalAlignment.Center;
					 m_angleLabels[i].verticalAlignment = UIVerticalAlignment.Middle;
					 m_angleLabels[i].autoSize = false;
					 m_angleLabels[i].width = 30;
					 m_angleLabels[i].height = 30;

					 zOrder = Mathf.Max(zOrder, m_angleLabels[i].zOrder);

					 m_angleLabels[i].relativePosition = new Vector2(m_positions[i], x_posAngle);

					 if (m_positions[i] < 0 || m_positions[i] > 360)
					 {
						  m_angleLabels[i].Hide();
					 }

				}

				/*
				UI.Background bgHelper = new UI.Background(60, 50, "CompassPanelDisc_direction", Color.black, Color.white, 2, (UI.Background.borders)((int)UI.Background.borders.left + (int)UI.Background.borders.right));

				Compass.UI.Panel directionPanel = AddUIComponent<Compass.UI.Panel>();
				directionPanel.setPanel(bgHelper);
				directionPanel.width = 60;
				directionPanel.height = 50;
				directionPanel.zOrder = zOrder + 1;
				directionPanel.relativePosition = new Vector3(150, 0);
				*/

				UISprite labelBg = AddUIComponent<UISprite>();
				labelBg.spriteName = "BlackRect";
				labelBg.width = 60;
				labelBg.height = 50;
				labelBg.zOrder = zOrder + 1;
				labelBg.relativePosition = new Vector3(150, 0);
				labelBg.color = Color.black;



				m_directionLabel = AddUIComponent<UILabel>();

				if (panelDisplay == 0 && isAviationMode)
					 m_directionLabel.text = "00";
				else if (panelDisplay == 0)
					 m_directionLabel.text = "000";
				else
					 m_directionLabel.text = m_angleLabels[0].text;

				m_directionLabel.textAlignment = UIHorizontalAlignment.Center;
				m_directionLabel.verticalAlignment = UIVerticalAlignment.Middle;
				m_directionLabel.autoSize = false;
				m_directionLabel.width = 60;
				m_directionLabel.height = 50;
				// m_directionLabel.zOrder = zOrder + 1;
				m_directionLabel.zOrder = labelBg.zOrder + 1;
				// m_directionLabel.zOrder = directionPanel.zOrder + 1;
				m_directionLabel.relativePosition = new Vector3(150, 0);

				m_directionLabel.backgroundSprite = "GenericPanelDark";
				
				// Set the left and right borders to white
				UISlicedSprite leftBorder = m_directionLabel.AddUIComponent<UISlicedSprite>();
				leftBorder.relativePosition = Vector3.zero;
				leftBorder.width = 1;
				leftBorder.height = m_directionLabel.height;
				leftBorder.spriteName = "WhiteRect";
				leftBorder.color = Color.white;

				UISlicedSprite rightBorder = m_directionLabel.AddUIComponent<UISlicedSprite>();
				rightBorder.relativePosition = new Vector3(m_directionLabel.width - 1, 0);
				rightBorder.width = 1;
				rightBorder.height = m_directionLabel.height;
				rightBorder.spriteName = "WhiteRect";
				rightBorder.color = Color.white;


				// Add a UIDragHandle to make the panel moveable
				m_dragHandle = AddUIComponent<UIDragHandle>();
				m_dragHandle.width = width;
				m_dragHandle.height = height;
				m_dragHandle.zOrder = m_directionLabel.zOrder + 1;
				m_dragHandle.relativePosition = new Vector2(0, 0);

				eventDoubleClick += OnDoubleClick;
		  }

		  public override void Update()
		  {
				PlayerPrefsInputKey savedInputKey = new PlayerPrefsInputKey("CompassKeyHide", SavedInputKey.Encode(KeyCode.C, false, false, true));
				if(savedInputKey.IsKeyUp())
				{
					 toggle();
				}
				
				if (!is_active)
					 return;
				
				
				base.Update();

				int panelDisplay = PlayerPrefs.GetInt("CompassPanelContent", 0);
				bool isAviationMode = (PlayerPrefs.GetInt("CompassAviationMode", 0) == 1);

				if (panelDisplay != currentPanelContent || currentAviationMode != isAviationMode)
				{
					 for (int i = 0; i < 16; i++)
					 {
						  switch (panelDisplay)
						  {
								case 0: // both
									 if (i % 4 == 0)
										  m_angleLabels[i].text = directions[i];
									 else if (isAviationMode)
										  m_angleLabels[i].text = Mathf.Round(i * 360 / 160).ToString("F0");
									 else
										  m_angleLabels[i].text = Mathf.Round(i * 360 / 16).ToString("F0");
									 break;
								case 1: // Angle Only
									 if (isAviationMode)
										  m_angleLabels[i].text = Mathf.Round(i * 360 / 160).ToString("F0");
									 else
										  m_angleLabels[i].text = Mathf.Round(i * 360 / 16).ToString("F0");
									 break;
								case 2: // Cardinal Direction Only
									 m_angleLabels[i].text = directions[i];
									 break;
						  }
					 }

					 currentPanelContent = panelDisplay;
					 currentAviationMode = isAviationMode;
				}

				// Call UpdateDirection every frame to update the text
				UpdateDirection();
		  }

		  public void toggle()
		  {
				if(is_active)
				{
					 this.Hide();
					 is_active = false;
				}
				else
				{
					 this.Show();
					 is_active = true;
				}
		  }

		  public void rotateCameraNorth()
		  {
				float angleOffset = PlayerPrefs.GetFloat("CompassAngleOffset", 180f);

				if (angleOffset < 0)
					 angleOffset = (270 - DayNightProperties.instance.m_Longitude + 360) % 360;

				CameraController cameraController = UnityEngine.Camera.main.GetComponent<CameraController>();

				cameraController.m_targetAngle.x = angleOffset;
		  }

		  protected override void OnPositionChanged()
		  {
				base.OnPositionChanged();

				PlayerPrefs.SetFloat("CompassPanelX", absolutePosition.x);
				PlayerPrefs.SetFloat("CompassPanelY", absolutePosition.y);
				PlayerPrefs.Save();
		  }

		  protected void UpdateDirection()
		  {
				float angleOffset = PlayerPrefs.GetFloat("CompassAngleOffset", 180f);

				if (angleOffset < 0)
					 angleOffset = (270 - DayNightProperties.instance.m_Longitude + 360) % 360;

				float angle = (Camera.main.transform.eulerAngles.y + angleOffset) % 360;

				if (currentPanelContent == 2 && currentAviationMode)
				{
					 m_directionLabel.text = directions[Mathf.RoundToInt(angle / 90)*4];
				}
				else if(currentPanelContent == 2)
				{
					 m_directionLabel.text = directions[Mathf.RoundToInt(angle / 22.5f)];
				}
				else if (currentAviationMode)
				{
					 float aviationAngle = Mathf.Round((angle / 10)) % 36;
					 m_directionLabel.text = aviationAngle.ToString("00");
				}
				else
					 m_directionLabel.text = angle.ToString("000") + "° ";


				float currentIndex = (angle / 22.5f) % 16;

				for(int i = 0; i < 16; i++)
				{
					 float position = 45f*((16 - currentIndex + i) % 16)+180;

					 if (position > 540)
						  position -= 720f;

					 m_positions[i] = position-15;

					 m_angleLabels[i].relativePosition = new Vector2(position, x_posAngle);

					 if (m_positions[i] < -30 || m_positions[i] > 360)
						  m_angleLabels[i].Hide();
					 else
						  m_angleLabels[i].Show();
				}
		  }

		  protected void OnDoubleClick(UIComponent component, UIMouseEventParameter eventParam)
		  {
				rotateCameraNorth();
		  }

		  protected void fillMainAtlas()
		  {
				Gradient bgGradient = new Gradient();

				GradientColorKey[] colorKeys = new GradientColorKey[4];
				colorKeys[0] = new GradientColorKey(Color.clear, 0f);
				colorKeys[1] = new GradientColorKey(Color.black, 0.08f);
				colorKeys[2] = new GradientColorKey(Color.black, 0.92f);
				colorKeys[3] = new GradientColorKey(Color.clear, 1f);

				GradientAlphaKey[] alphaKeys = new GradientAlphaKey[4];
				alphaKeys[0] = new GradientAlphaKey(0f, 0f);
				alphaKeys[1] = new GradientAlphaKey(1f, 0.08f);
				alphaKeys[2] = new GradientAlphaKey(1f, 0.92f);
				alphaKeys[3] = new GradientAlphaKey(0f, 1f);

				bgGradient.SetKeys(colorKeys, alphaKeys);

				UI.Background bgHelper = new UI.Background(360, 50, "CompassPanelDisc_background", bgGradient, UI.Background.GradientDirection.horizontal);

				// Material mat = this.atlas.material;

				// this.atlas = new UITextureAtlas();
				// this.atlas.name = "CompassPanelDisc_panelAtlas";
				// this.atlas.material = mat;
				this.atlas.AddTextures(bgHelper.textures);

				Debug.Log("Number of textures in bgHelper: " + bgHelper.textures.Length);

				for (int i=0; i < bgHelper.textures.Length; i++)
				{
					 Debug.Log("Texture " + i + " dimensions: " + bgHelper.textures[i].width + "x" + bgHelper.textures[i].height);
				}


		  }

		  protected void fillDirectionAtlas()
		  {
				string spriteName = "CompassPanelDisc_direction";

				UI.Background bgHelper = new UI.Background(60, 50, spriteName, Color.black, Color.white, 2, (UI.Background.borders)((int)UI.Background.borders.left + (int)UI.Background.borders.right));

				Texture2D[] textures = new Texture2D[m_directionLabel.atlas.count + 1];

				for (int index = 0; index < m_directionLabel.atlas.count; ++index)
				{
					 textures[index] = m_directionLabel.atlas.sprites[index].texture;
				}

				textures[m_directionLabel.atlas.count] = bgHelper.textures[0];



				Material mat = m_directionLabel.atlas.material;

				m_directionLabel.atlas = ScriptableObject.CreateInstance<UITextureAtlas>();
				m_directionLabel.atlas.name = spriteName;
				m_directionLabel.atlas.material = mat;
				m_directionLabel.atlas.AddTextures(textures);


				// m_directionLabel.atlas.Remove(spriteName);
				// m_directionLabel.atlas.AddSprite(bgHelper.spriteInfo);
				// 
				// 
				// Rect[] rectArray = m_directionLabel.atlas.texture.PackTextures(textures, m_directionLabel.atlas.padding, 4096);
				// for (int index = 0; index < m_directionLabel.atlas.count; ++index)
				//     m_directionLabel.atlas.sprites[index].region = rectArray[index];
				// m_directionLabel.atlas.sprites.Sort();
				// m_directionLabel.atlas.RebuildIndexes();
				// UIView.RefreshAll();

				// m_directionLabel.atlas.AddTextures(bgHelper.textures);
		  }

		  public new float PixelsToUnits()
		  {
				return base.PixelsToUnits();
		  }
	 }
}
