using UnityEngine;
using ColossalFramework.UI;

namespace Compass
{
    public class CompassPanel : UIPanel
    {
        private UILabel m_angleLabel;
        private UILabel m_directionLabel;
        private UIDragHandle m_dragHandle;
        private int currentPanelContent;


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

        }

        public override void Update()
        {
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
            float angle = Camera.main.transform.eulerAngles.y + angleOffset;

            if (angle > 360)
                angle -= 360f;

            m_angleLabel.text = angle.ToString("F0") + "° ";
            m_directionLabel.text = GetCardinalDirection(angle);
        }


        private string GetCardinalDirection(float angle)
        {
            string[] directions = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
            int index = Mathf.RoundToInt(angle / 22.5f);
            return directions[index % 16];
        }
    }
}
