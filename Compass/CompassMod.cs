using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using UnityEngine;
using System.Collections.Generic;

namespace Compass
{
	 
	 public class CompassMod : IUserMod
	 {
		  public const string version = "1.0.0.7";

		  public static Panel.Manager panelManager = new Panel.Manager();
		  
		  public string Name
		  {
				get { return "Compass"; }
		  }

		  public string Description
		  {
				get { return "This mod displays the cardinal direction."; }
		  }

		  public CompassMod()
		  {
				Debug.Log("CompassMod constructor called.");
		  }


		  public void OnSettingsUI(UIHelperBase helper)
		  {
				// Create a new group in the settings panel
				UI.HelperBase helper2 = new UI.HelperBase(helper);

				UI.HelperBase groupSE = helper2.AddCompassGroup("Compass settings");

				int angleOffset = Mathf.RoundToInt(PlayerPrefs.GetFloat("CompassAngleOffset", 180f) / 90f)+1;
				int typeContent = PlayerPrefs.GetInt("CompassTypeContent", 0);
				int panelContent = PlayerPrefs.GetInt("CompassPanelContent", 0);
				bool isAviationMode = (PlayerPrefs.GetInt("CompassAviationMode", 0) == 1);

				// Create the dropdown for angle selection
				string[] optionsAngle = { "Auto", "0°", "90°", "180°", "270°" };
				UIDropDown dropdownAngle = groupSE.AddDropdown("Angle Offset", optionsAngle, angleOffset, OnAngleOffsetChanged) as UIDropDown;
				dropdownAngle.width = 300;

				// Create the dropdown for the compass type
				string[] optionsType = { "Solid State", "Disc"}; //, "Rose" };
				UIDropDown dropdownType = groupSE.AddDropdown("Display", optionsType, typeContent, OnTypeChanged) as UIDropDown;
				dropdownType.width = 300;

				// Create the dropdown for display selection
				string[] optionsPanel = { "Both", "Angle", "Cardinal Direction" };
				UIDropDown dropdownPanel = groupSE.AddDropdown("Display", optionsPanel, panelContent, OnPanelDisplayChanged) as UIDropDown;
				dropdownPanel.width = 300;
				
				UICheckBox aviationMode = groupSE.AddCheckbox("Display direction as aviation short hand", isAviationMode, OnPanelModeChanged) as UICheckBox;

				UI.HelperBase groupHK = helper2.AddCompassGroup("Hot Keys");

				// Create the component for key assignement
				InputKey keysHide = SavedInputKey.Encode(KeyCode.C, false, false, true);

				UI.KeyMapping hideKey = groupHK.AddKeyMapping("Toggle the compass", "CompassKeyHide", keysHide) as UI.KeyMapping;


				UI.HelperBase groupRE = helper2.AddCompassGroup("Reset Options");

				UIButton resetButton = groupRE.AddButton("Reset all Settings", OnResetAll) as UIButton;



#if DEBUG
				UI.HelperBase groupDE = helper2.AddCompassGroup("Debug Data");

				UILabel versionLabel = groupDE.AddLabel("Version: " + version);

				UILabel latLabel = groupDE.AddLabel("Latitude: NA");
				UILabel lonLabel = groupDE.AddLabel("Longitude: NA");

				DayNightProperties dnprop = DayNightProperties.instance;
				if((object) dnprop != (object) null)
				{
					 latLabel.text = "Latitude: " + dnprop.m_Longitude;
					 lonLabel.text = "Longitude: " + dnprop.m_Latitude;
				}
#endif
		  }

		  private void OnAngleOffsetChanged(int sel)
		  {
				// Get the selected angle from the dropdown options
				PlayerPrefs.SetFloat("CompassAngleOffset", (sel-1) * 90f);
				PlayerPrefs.Save();
		  }

		  private void OnTypeChanged(int sel)
		  {
				// Get the selected angle from the dropdown options
				PlayerPrefs.SetInt("CompassTypeContent", sel);
				PlayerPrefs.Save();

				panelManager.UpdatePanels();
		  }

		  private void OnPanelDisplayChanged(int sel)
		  {
				// Get the selected angle from the dropdown options
				PlayerPrefs.SetInt("CompassPanelContent", sel);
				PlayerPrefs.Save();
		  }

		  private void OnPanelModeChanged(bool isChecked)
		  {
				// Get the selected angle from the dropdown options
				PlayerPrefs.SetInt("CompassAviationMode", isChecked ? 1:0 );
				PlayerPrefs.Save();
		  }

		  private void OnResetAll()
		  {
				PlayerPrefs.DeleteKey("CompassAngleOffset");
				PlayerPrefs.Save();
				PlayerPrefs.DeleteKey("CompassAviationMode");
				PlayerPrefs.Save();
				PlayerPrefs.DeleteKey("CompassKeyHide");
				PlayerPrefs.Save();
				PlayerPrefs.DeleteKey("CompassPanelContent");
				PlayerPrefs.Save();
				PlayerPrefs.DeleteKey("CompassPanelX");
				PlayerPrefs.Save();
				PlayerPrefs.DeleteKey("CompassPanelY");
				PlayerPrefs.Save();
				PlayerPrefs.DeleteKey("CompassTypeContent");
				PlayerPrefs.Save();
		  }

	 }
}
