using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using ICities;
using System.Collections.Generic;
using System.Reflection;



namespace Compass
{
    class CompassUIHelper : UIHelperBase
    {

        protected UIHelperBase helperBase;


        public CompassUIHelper(UIHelperBase parent) : base()
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
        public CompassUIHelper AddCompassGroup(string text)
        {
            UIHelperBase group = helperBase.AddGroup(text);

            return new CompassUIHelper(group);
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
            UIKeyMapping keyMapping = new UIKeyMapping(text, name, def, helperBase);

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

    class UIKeyMapping : UIInteractiveComponent
    {
        private UIPanel parentPanel;
        private static List<PlayerPrefsInputKey> s_keyMappingsCache;
        private PlayerPrefsInputKey m_inputKey;
        private UIPanel m_panel;
        private UILabel m_label;
        private UIButton m_button;

        protected float m_width;


        public new float width
        {
            get { return m_width; }
            set
            {
                m_width = value;

                m_panel.width = value;
                m_label.width = value / 2;
                m_button.relativePosition = new Vector2(m_label.width, m_button.relativePosition.y);
                m_button.width = value / 2;
            }
        }

        public UIKeyMapping(string label, string name, InputKey def, UIHelperBase parent)
        {
            parentPanel = (parent as UIHelper).self as UIPanel;
            parentPanel.autoFitChildrenVertically = true;
            parentPanel.autoLayoutDirection = LayoutDirection.Vertical;

            m_width = parentPanel.width;

            PlayerPrefsInputKey savedInputKey = new PlayerPrefsInputKey(name, def, true);

            m_panel = parentPanel.AttachUIComponent(UITemplateManager.GetAsGameObject("KeyBindingTemplate")) as UIPanel;

            m_label = m_panel.Find<UILabel>("Name");
            m_label.text = label;

            m_button = m_panel.Find<UIButton>("Binding");
            m_button.text = savedInputKey.ToLocalizedString("KEYNAME");

            m_button.objectUserData = savedInputKey;
            m_button.eventClick += OnButtonClick;
            m_button.eventKeyDown += OnKeyDown;
            
            if(IsKeyBindingUsed())
                m_button.textColor = Color.red;
            else
                m_button.textColor = Color.white;
        }


        public override void OnDestroy()
        {
            ClearCache();
            base.OnDestroy();
        }

        public override void OnDisable()
        {
            ClearCache();
            base.OnDisable();
        }


        protected void OnButtonClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            if (m_inputKey == null)
            {
                eventParam.Use();
                m_inputKey = (PlayerPrefsInputKey)eventParam.source.objectUserData;
                UIButton uibutton = eventParam.source as UIButton;
                uibutton.buttonsMask = (UIMouseButton.Left | UIMouseButton.Right | UIMouseButton.Middle | UIMouseButton.Special0 | UIMouseButton.Special1 | UIMouseButton.Special2 | UIMouseButton.Special3);
                uibutton.text = "Press any key";
                eventParam.source.Focus();
                UIView.PushModal(eventParam.source);
                return;
            }


            if (!(eventParam.buttons == UIMouseButton.Left || eventParam.buttons == UIMouseButton.Right))
            {
                eventParam.Use();
                UIView.PopModal();

                InputKey value = PlayerPrefsInputKey.Encode(ButtonToKeycode(eventParam.buttons), this.IsControlDown(), this.IsShiftDown(), this.IsAltDown());
                m_inputKey.value = value;
                GameSettings.SaveAll();

                UIButton uibutton2 = eventParam.source as UIButton;
                uibutton2.text = m_inputKey.ToLocalizedString("KEYNAME");
                uibutton2.buttonsMask = UIMouseButton.Left;
                m_inputKey = null;
            }
        }

        protected void OnKeyDown(UIComponent comp, UIKeyEventParameter eventParam)
        {
            if (m_inputKey != null && !IsModifierKey(eventParam.keycode))
            {
                eventParam.Use();
                UIView.PopModal();

                InputKey value = (eventParam.keycode == KeyCode.Escape) ? m_inputKey.value : PlayerPrefsInputKey.Encode(eventParam.keycode, eventParam.control, eventParam.shift, eventParam.alt);
                if (eventParam.keycode == KeyCode.Backspace)
                {
                    value = PlayerPrefsInputKey.Empty;
                }
                m_inputKey.value = value;
                GameSettings.SaveAll();

                (eventParam.source as UITextComponent).text = m_inputKey.ToLocalizedString("KEYNAME");
                m_inputKey = null;
            }
        }


        protected KeyCode ButtonToKeycode(UIMouseButton button)
        {
            if (button == UIMouseButton.Left)
            {
                return KeyCode.Mouse0;
            }
            if (button == UIMouseButton.Right)
            {
                return KeyCode.Mouse1;
            }
            if (button == UIMouseButton.Middle)
            {
                return KeyCode.Mouse2;
            }
            if (button == UIMouseButton.Special0)
            {
                return KeyCode.Mouse3;
            }
            if (button == UIMouseButton.Special1)
            {
                return KeyCode.Mouse4;
            }
            if (button == UIMouseButton.Special2)
            {
                return KeyCode.Mouse5;
            }
            if (button == UIMouseButton.Special3)
            {
                return KeyCode.Mouse6;
            }
            return KeyCode.None;
        }

        protected bool IsModifierKey(KeyCode code)
        {
            return code == KeyCode.LeftControl || code == KeyCode.RightControl || code == KeyCode.LeftShift || code == KeyCode.RightShift || code == KeyCode.LeftAlt || code == KeyCode.RightAlt;
        }

        protected bool IsControlDown()
        {
            return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        }

        protected bool IsShiftDown()
        {
            return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        }

        protected bool IsAltDown()
        {
            return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        }

        protected static void ClearCache()
        {
            s_keyMappingsCache = null;
        }

        protected static List<PlayerPrefsInputKey> GetKeyMappings()
        {
            if (s_keyMappingsCache != null)
            {
                return s_keyMappingsCache;
            }
        
            OptionsMainPanel optionsPanel = UIView.library.Get<OptionsMainPanel>("OptionsPanel");
            if (optionsPanel == null)
            {
                Debug.LogError("OptionsMainPanel not found in UIView.library");
                return new List<PlayerPrefsInputKey>();
            }
        
        
            UIComponent keyShortcuts = optionsPanel.Find("KeyMapping");
            if (keyShortcuts == null)
            {
                Debug.LogError("KeyMapping not found in keymappingPanel");
                return new List<PlayerPrefsInputKey>();
            }

            var keyMappings = keyShortcuts.GetType().GetField("m_Keymappings", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(keyShortcuts) as List<PlayerPrefsInputKey>;
        
            if (keyMappings == null)
            {
                Debug.LogError("m_Keymappings field not found in keyShortcuts");
                return new List<PlayerPrefsInputKey>();
            }
        
            s_keyMappingsCache = keyMappings;
        
            Debug.Log("Found " + keyMappings.Count + " key mappings");
            foreach (PlayerPrefsInputKey keyMapping in keyMappings)
            {
                Debug.Log(keyMapping.name + ": Key=" + keyMapping.Key + ", Control=" + keyMapping.Control + ", Shift=" + keyMapping.Shift + ", Alt=" + keyMapping.Alt);
            }
        
            return s_keyMappingsCache;
        }


        protected bool IsKeyBindingUsed()
        {
            // Debug.LogError("IsKeyBindingUsed");
            // List<PlayerPrefsInputKey> keyMappings = GetKeyMappings();
            // 
            // Debug.LogError((keyMappings != null) ? keyMappings.GetType().FullName : "KeyMapping null");
            // 
            // foreach (PlayerPrefsInputKey keyMapping in keyMappings)
            // {
            //     if (keyMapping.name != m_inputKey.name && keyMapping.Key == m_inputKey.Key && keyMapping.Control == m_inputKey.Control && keyMapping.Shift == m_inputKey.Shift && keyMapping.Alt == m_inputKey.Alt)
            //         return true;
            // }

            return false;
        }

        protected override void OnVisibilityChanged()
        {
            ClearCache();
            base.OnVisibilityChanged();
        }
    }
}
