using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using ICities;
using System.Collections.Generic;
using System.Reflection;



namespace Compass.UI
{
	 class HelperBase : ICities.UIHelperBase
	 {

		  protected UIHelperBase helperBase;


		  public HelperBase(UIHelperBase parent) : base()
		  {
				helperBase = parent;
		  }



		  public object AddButton(string text, OnButtonClicked eventCallback)
		  {
				return helperBase.AddButton(text, eventCallback);
		  }

		  public object AddCheckbox(string text, bool defaultValue, OnCheckChanged eventCallback)
		  {
				return helperBase.AddCheckbox(text, defaultValue, eventCallback);
		  }

		  public object AddDropdown(string text, string[] options, int defaultSelection, OnDropdownSelectionChanged eventCallback)
		  {
				return helperBase.AddDropdown(text, options, defaultSelection, eventCallback);
		  }

		  public UIHelperBase AddGroup(string text)
		  {
				return helperBase.AddGroup(text);
		  }
		  public HelperBase AddCompassGroup(string text)
		  {
				UIHelperBase group = helperBase.AddGroup(text);

				return new HelperBase(group);
		  }

		  public object AddSlider(string text, float min, float max, float step, float defaultValue, OnValueChanged eventCallback)
		  {
				return helperBase.AddSlider(text, min, max, step, defaultValue, eventCallback);
		  }

		  public object AddSpace(int height)
		  {
				return helperBase.AddSpace(height);
		  }

		  public object AddTextfield(string text, string defaultContent, OnTextChanged eventChangedCallback, OnTextSubmitted eventSubmittedCallback = null)
		  {
				return helperBase.AddTextfield(text, defaultContent, eventChangedCallback, eventSubmittedCallback);
		  }



		  public object AddKeyMapping(string text, string name, InputKey def)
		  {
				KeyMapping keyMapping = new KeyMapping(text, name, def, helperBase);

				return keyMapping;
		  }

		  internal UILabel AddLabel(string p)
		  {
				UIPanel parentPanel = (helperBase as UIHelper).self as UIPanel;

				UILabel label = parentPanel.AddUIComponent<UILabel>();
				label.text = p;


				return label;
		  }
	 }

}
