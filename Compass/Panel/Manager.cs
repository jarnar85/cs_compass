using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;


namespace Compass.Panel
{
	 public class Manager
	 {
		  private Disc m_panelDisc;
		  private Solidstate m_panelSolid;
		  private Rose m_panelRose;
		  private int currentCompassType = -1;

		  public void CreatePanels()
		  {
				UpdatePanels();
		  }


		  public void UpdatePanels()
		  {
				int typeContent = PlayerPrefs.GetInt("CompassTypeContent", 0);


				if (currentCompassType != typeContent)
				{
					 // Destroy the previous panel and reset panel position if possible
					 if (currentCompassType >= 0)
					 {
						  PlayerPrefs.DeleteKey("CompassPanelX");
						  PlayerPrefs.DeleteKey("CompassPanelY");

						  switch (currentCompassType)
						  {
								// Solid State
								case 0:
									 m_panelSolid.Hide();
									 GameObject.Destroy(m_panelSolid.gameObject);
									 m_panelSolid = null;
									 break;
								// Disc
								case 1:
									 m_panelDisc.Hide();
									 GameObject.Destroy(m_panelDisc.gameObject);
									 m_panelDisc = null;
									 break;
								// Rose
								case 2:
									 m_panelRose.Hide();
									 GameObject.Destroy(m_panelRose.gameObject);
									 m_panelRose = null;
									 break;
						  }
					 }

					 float x = 0;
					 float y = 0;

					 // Create the panel and add it to the view
					 switch (typeContent)
					 {
						  // Solid State
						  case 0:
								m_panelSolid = UIView.GetAView().AddUIComponent(typeof(Solidstate)) as Solidstate;
								// m_panelSolid.pivot = UIPivotPoint.BottomRight;
								
								x = PlayerPrefs.GetFloat("CompassPanelX", 50);
								y = PlayerPrefs.GetFloat("CompassPanelY", 100);
								m_panelSolid.absolutePosition = new Vector3(x, y);
								m_panelSolid.Show();
								break;
						  // Disc
						  case 1:
								m_panelDisc = UIView.GetAView().AddUIComponent(typeof(Disc)) as Disc;
								// m_panelDisc.pivot = UIPivotPoint.TopCenter;

								x = PlayerPrefs.GetFloat("CompassPanelX", 10);
								y = PlayerPrefs.GetFloat("CompassPanelY", 10);
								m_panelDisc.absolutePosition = new Vector3(x, y);
								m_panelDisc.Show();
								break;
						  // Rose
						  case 2:
								m_panelRose = UIView.GetAView().AddUIComponent(typeof(Rose)) as Rose;

								x = PlayerPrefs.GetFloat("CompassPanelX", 50);
								y = PlayerPrefs.GetFloat("CompassPanelY", 100);
								m_panelRose.absolutePosition = new Vector3(x, y);
								m_panelRose.Show();
								break;
					 }


					 if (currentCompassType < 0)
					 {
						  UIView uiv = UIView.GetAView();
						  float scale = uiv.scale;
						  float units = uiv.PixelsToUnits();

						  Debug.Log("Game scale: " + scale + " / " + units);
						  
						  switch (typeContent)
						  {
								// Solid State
								case 0:
									 uiv = m_panelSolid.GetUIView();
									 units = m_panelSolid.PixelsToUnits();
									 break;
								// Disc
								case 1:
									 uiv = m_panelDisc.GetUIView();
									 units = m_panelDisc.PixelsToUnits();
									 break;
								// Rose
								case 2:
									 uiv = m_panelRose.GetUIView();
									 units = m_panelRose.PixelsToUnits();
									 break;
						  }

						  scale = uiv.scale;
						  Debug.Log("Compass scale: " + scale + " / " + units);
					 }

					 currentCompassType = typeContent;
				}
		  }



	 }
}
