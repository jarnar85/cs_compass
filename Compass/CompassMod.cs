using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

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
            CompassUIHelper helper2 = new CompassUIHelper(helper);

            CompassUIHelper groupSE = helper2.AddCompassGroup("Compass settings");

            int angleOffset = Mathf.RoundToInt(PlayerPrefs.GetFloat("CompassAngleOffset", 180f) / 90f);
            int panelContent = PlayerPrefs.GetInt("CompassPanelContent", 0);

            // Create the dropdown for angle selection
            string[] optionsAngle = { "0°", "90°", "180°", "270°" };
            UIDropDown dropdownAngle = groupSE.AddDropdown("Angle Offset", optionsAngle, angleOffset, OnAngleOffsetChanged) as UIDropDown;
            dropdownAngle.width = 300;

            // Create the dropdown for display selection
            string[] optionsPanel = { "Both", "Angle", "Cardinal Direction" };
            UIDropDown dropdownPanel = groupSE.AddDropdown("Display", optionsPanel, panelContent, OnPanelPositionChanged) as UIDropDown;
            dropdownPanel.width = 300;

            CompassUIHelper groupHK = helper2.AddCompassGroup("Hot Keys");

            // Create the component for key assignement
            InputKey keysHide = SavedInputKey.Encode(KeyCode.C, false, false, true);

            UIKeyMapping hideKey = groupHK.AddKeyMapping("Toggle the compass", "CompassKeyHide", keysHide) as UIKeyMapping;
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
