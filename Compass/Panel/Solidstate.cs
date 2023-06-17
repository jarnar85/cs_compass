using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;

namespace Compass
{
	 public class Solidstate : UIPanel
	 {
		  private UILabel m_angleLabel;
		  private UILabel m_directionLabel;
		  private UIDragHandle m_dragHandle;
		  private int currentPanelContent;
		  private bool is_active = true;


		  public override void Start()
		  {
				base.Start();

				// Set up the UI
				width = 100;
				height = 50;
				backgroundSprite = "MenuPanel";
				color = new Color32(16, 16, 16, 128);

				// Add Labels
				m_angleLabel = AddUIComponent<UILabel>();
				m_angleLabel.text = "0°";
				m_angleLabel.textAlignment = UIHorizontalAlignment.Right;
				m_angleLabel.verticalAlignment = UIVerticalAlignment.Middle;
				m_angleLabel.autoSize = false;
				m_angleLabel.width = 50;
				m_angleLabel.height = 50;

				m_directionLabel = AddUIComponent<UILabel>();
				m_directionLabel.text = "N";
				m_directionLabel.textAlignment = UIHorizontalAlignment.Left;
				m_directionLabel.verticalAlignment = UIVerticalAlignment.Middle;
				m_directionLabel.autoSize = false;
				m_directionLabel.width = 50;
				m_directionLabel.height = 50;

				// Add a UIDragHandle to make the panel moveable
				m_dragHandle = AddUIComponent<UIDragHandle>();
				m_dragHandle.width = width;
				m_dragHandle.height = height;
				m_dragHandle.zOrder = Mathf.Max(m_angleLabel.zOrder, m_directionLabel.zOrder)+1;

#if DEBUG
				m_angleLabel.color = new Color32(255, 0, 0, 128);
				m_directionLabel.color = new Color32(0, 255, 0, 128);
				m_dragHandle.color = new Color32(0, 0, 255, 128);
#endif

				// Position the elements within the panel
				m_dragHandle.relativePosition = new Vector2(0, 0);
				m_angleLabel.relativePosition = new Vector2(0, 0);
				m_directionLabel.relativePosition = new Vector2(50, 0);

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

				if (panelDisplay != currentPanelContent)
				{
					 switch (panelDisplay)
					 {
						  case 0: // both
								width = 100;
								m_dragHandle.width = 100;
								m_directionLabel.relativePosition = new Vector2(50, 0);

								m_angleLabel.Show();

								m_directionLabel.Show();
								m_directionLabel.textAlignment = UIHorizontalAlignment.Left;
								break;
						  case 1: // Angle Only
								if (currentPanelContent == 2)
									 m_angleLabel.Show();
								else
								{
									 width = 50;
									 m_dragHandle.width = 50;
								}

								m_directionLabel.Hide();
								break;
						  case 2: // Cardinal Direction Only
								if (currentPanelContent == 1)
									 m_directionLabel.Show();
								else
								{
									 width = 50;
									 m_dragHandle.width = 50;
								}

								m_angleLabel.Hide();
								m_directionLabel.relativePosition = new Vector2(0, 0);
								m_directionLabel.textAlignment = UIHorizontalAlignment.Center;
								break;
					 }

					 currentPanelContent = panelDisplay;
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
				bool isAviationMode = (PlayerPrefs.GetInt("CompassAviationMode", 0) == 1);

				if (angleOffset < 0)
					 angleOffset = (270 - DayNightProperties.instance.m_Longitude + 360) % 360;

				float angle = Camera.main.transform.eulerAngles.y + angleOffset;

				if (angle >= 360)
					 angle -= 360f;

				if (isAviationMode)
				{
					 float aviationAngle = Mathf.Round((angle / 10)) % 36;
					 m_angleLabel.text = aviationAngle.ToString("F0");
				}
				else
					 m_angleLabel.text = angle.ToString("F0") + "° ";

				m_directionLabel.text = GetCardinalDirection(angle, isAviationMode);
		  }

		  protected void OnDoubleClick(UIComponent component, UIMouseEventParameter eventParam)
		  {
				rotateCameraNorth();
		  }

		  private string GetCardinalDirection(float angle, bool isAviationMode = false)
		  {
				if(isAviationMode)
				{
					 string[] directions = { "N", "E", "S", "W" };
					 int index = Mathf.RoundToInt(angle / 90f);
					 return directions[index % 4];
				}
				else
				{
					 string[] directions = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
					 int index = Mathf.RoundToInt(angle / 22.5f);
					 return directions[index % 16];
				}
		  }

		  public new float PixelsToUnits()
		  {
				return base.PixelsToUnits();
		  }
	 }
}
