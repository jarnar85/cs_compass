using UnityEngine;
using ColossalFramework.UI;
using ICities;

namespace Compass
{
    public class CompassMod : IUserMod
    {
        public string Name
        {
            get { return "Compass"; }
        }

        public string Description
        {
            get { return "This mod displays the cardinal direction."; }
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            // Create a new group in the settings panel
            var group = helper.AddGroup("Compass settings");
            int angleOffset = Mathf.RoundToInt(PlayerPrefs.GetFloat("CompassAngleOffset", 180f) / 90f);
            int panelContent = PlayerPrefs.GetInt("CompassPanelContent", 0);

            // Create the dropdown for angle selection
            string[] optionsAngle = { "0°", "90°", "180°", "270°" };
            UIDropDown dropdownAngle = group.AddDropdown("Angle Offset", optionsAngle, angleOffset, OnAngleOffsetChanged) as UIDropDown;
            dropdownAngle.width = 300;

            // Create the dropdown for display selection
            string[] optionsPanel = { "Both", "Angle", "Cardinal Direction" };
            UIDropDown dropdownPanel = group.AddDropdown("Display", optionsPanel, panelContent, OnPanelPositionChanged) as UIDropDown;
            dropdownPanel.width = 300;
        }

        private void OnAngleOffsetChanged(int sel)
        {
            // Get the selected angle from the dropdown options
            PlayerPrefs.SetFloat("CompassAngleOffset", sel * 90f);
            PlayerPrefs.Save();
        }

        private void OnPanelPositionChanged(int sel)
        {
            // Get the selected angle from the dropdown options
            PlayerPrefs.SetInt("CompassPanelContent", sel);
            PlayerPrefs.Save();
        }
    }
}
