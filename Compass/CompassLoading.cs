using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;


namespace Compass
{
    public class CompassLoading : LoadingExtensionBase
    {
        private CompassPanel m_panel;

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);

            // Load the saved position of the panel from PlayerPrefs
            float x = PlayerPrefs.GetFloat("CompassPanelX", 100);
            float y = PlayerPrefs.GetFloat("CompassPanelY", 50);


            // Create the panel and add it to the view
            m_panel = UIView.GetAView().AddUIComponent(typeof(CompassPanel)) as CompassPanel;
            m_panel.absolutePosition = new Vector3(x, y);
            m_panel.Show();
        }
    } 
}
