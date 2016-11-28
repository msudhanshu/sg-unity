using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopUpManager : Manager<PopUpManager> {

	public UIGamePopUp activePanel;
	public Dictionary <PopUpType, UIGamePopUp> panels = new Dictionary <PopUpType, UIGamePopUp>();
	private HashSet<PopUpType> restrictOtherPanels = new HashSet<PopUpType>();

	override public void StartInit () {
		restrictOtherPanels.Add(PopUpType.PLACE_BUILDING);
	}
	
	override public void PopulateDependencies () {
	}

	Queue<ScheduledPopUp> scheduledPopUps = new Queue<ScheduledPopUp>();

	public void SchedulePopUp(UIGamePopUp panel, long delay) {
		scheduledPopUps.Enqueue(new ScheduledPopUp(panel, delay));

		if(scheduledPopUps.Count == 1 && activePanel != null && activePanel.panelType == PopUpType.DEFAULT)
			ShowScheduledPopUp();
	}

	public void SchedulePopUp(PopUpType type, long delay = 1) {
		SchedulePopUp(panels[type], delay);
	}

	public void RemoveScheduledPopUp() {
		//TODO: Based on class name, remove from the scheduled PopUps queue
	}

	private void ShowScheduledPopUp() {
		StartCoroutine(DoShowScheduledPopUp());
	}
	private IEnumerator DoShowScheduledPopUp() {
		ScheduledPopUp popup = scheduledPopUps.Dequeue();
		if (popup == null)
			yield break;
		float secs = popup.GetDelay();
		yield return new WaitForSeconds(popup.GetDelay());
		UIGamePopUp panel = popup.GetPanel();
		panel.Show();
	}

	private void HideScheduledPopUp() {
		activePanel.Hide();
		if(scheduledPopUps.Count > 0)
			ShowScheduledPopUp();
	}

	public void AddPanel(PopUpType panelType, UIGamePopUp panel) {
		panels.Add(panelType, panel);
		if(panelType == PopUpType.DEFAULT) {
			activePanel = panel;
		}
	}

	public void ShowPanel(PopUpType panelType, bool allowOtherPopUps = true) {

		if (panelType == PopUpType.DEFAULT) {
			BuildingManager3D.ActiveBuilding = null;
			allowOtherPopUps = true;

			if (scheduledPopUps.Count > 0) {
				ShowScheduledPopUp();
			}
		}

		if (restrictOtherPanels.Contains(activePanel.panelType) && panelType != PopUpType.DEFAULT)
			return;

		if(panels.ContainsKey(panelType)) {
			UIGamePopUp panel = panels[panelType];
			panel.Show();
		}
	}

	public UIGamePopUp getPanel(PopUpType type){
		if(panels.ContainsKey(type))
			return panels[type];
		return null;
	}
	public void HideActivePanel() {
		//activePanel.Hide ();
		//ShowScheduledPopUp();
	}
	

}
