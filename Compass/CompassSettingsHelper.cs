using ColossalFramework;
using UnityEngine;
using System;
using System.Collections;

namespace Compass
{

    public class PlayerPrefsInputKey : SavedInputKey
    {
        private bool m_autoSave  = false;
        private string m_name;

        private KeyCode m_DefaultKey     = KeyCode.None;
        private bool m_DefaultControl    = false;
        private bool m_DefaultShift      = false;
        private bool m_DefaultAlt        = false;


        public new InputKey value 
        {
            get
            {
                return Encode(Key, Control, Shift, Alt);
            }
            set
            {
                InputKeyObj defData = Decode(value);

                Key = defData.Key;
                Control = defData.Control;
                Shift = defData.Shift;
                Alt = defData.Alt;

                if (m_autoSave)
                    Save();
            }
        }


        public PlayerPrefsInputKey(string name)
            : base(name, string.Empty)
        {
            m_name = name;
            Load();
        }

        public PlayerPrefsInputKey(string name, InputKey def): base(name, string.Empty, def)
        {
            m_name = name;

            InputKeyObj defData = Decode(def);

            m_DefaultKey = defData.Key;
            m_DefaultControl = defData.Control;
            m_DefaultShift = defData.Shift;
            m_DefaultAlt = defData.Alt;

            Load();
        }

        public PlayerPrefsInputKey(string name, InputKey def, bool autoUpdate): base(name, string.Empty, def, autoUpdate)
        {
            m_name = name;

            InputKeyObj defData = Decode(def);
            
            m_DefaultKey     = defData.Key;
            m_DefaultControl = defData.Control;
            m_DefaultShift   = defData.Shift;
            m_DefaultAlt     = defData.Alt;

            m_autoSave = autoUpdate;
            Load();
        }

        public PlayerPrefsInputKey(string name, KeyCode def, bool control, bool shift, bool alt, bool autoUpdate)
            : base(name, string.Empty, def, control, shift, alt, autoUpdate)
        {
            m_name = name;
            m_autoSave = autoUpdate;
            Load();

            m_DefaultKey         = def;
            m_DefaultControl     = control;
            m_DefaultShift       = shift;
            m_DefaultAlt         = alt;
        }


        public void Load()
        {
            int def = Encode(m_DefaultKey, m_DefaultControl, m_DefaultShift, m_DefaultAlt);

            value = (InputKey)PlayerPrefs.GetInt(m_name, def);
        }

        public void Save()
        {
            PlayerPrefs.SetInt(m_name, (int)value);
            PlayerPrefs.Save();
        }

        public void Reset()
        {
            Key = m_DefaultKey;
            Control = m_DefaultControl;
            Shift = m_DefaultShift;
            Alt = m_DefaultAlt;

            if (m_autoSave)
                Save();
        }

        public InputKeyObj Decode(InputKey key)
        {
            int intKey = key;

            InputKeyObj keyObject = new InputKeyObj();

            keyObject.Key = (KeyCode)(intKey & 0xFFFF);
            keyObject.Control = (intKey & (1 << 30)) != 0;
            keyObject.Shift = (intKey & (1 << 29)) != 0;
            keyObject.Alt = (intKey & (1 << 28)) != 0;

            return keyObject;
        }
    }

    public class InputKeyObj
    {
        public KeyCode Key      = KeyCode.None;
        public bool Control     = false;
        public bool Shift       = false;
        public bool Alt         = false;
    }
}
