using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;


namespace Compass
{
	 public class CompassLoading : LoadingExtensionBase
	 {
		  public override void OnLevelLoaded(LoadMode mode)
		  {
				base.OnLevelLoaded(mode);

				CompassMod.panelManager.CreatePanels();
		  }



		  public override void OnCreated(ILoading loading)
		  {
		  }
	 } 
}
