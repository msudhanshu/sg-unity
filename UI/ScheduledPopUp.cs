using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScheduledPopUp {
	
	protected UIGamePopUp panel;
	protected long delay;
	protected long DEFAULT_DELAY_IN_MSEC = 10;

	public ScheduledPopUp(UIGamePopUp panel) {
		Initialize(panel, DEFAULT_DELAY_IN_MSEC);
	}

	public ScheduledPopUp(UIGamePopUp panel, long delay) {
		Initialize (panel, delay);
	}

	private void Initialize(UIGamePopUp panel, long delay) {
		this.panel = panel;
		this.delay = delay;
	}
	
	public UIGamePopUp GetPanel() {
		return panel;
	}
	
	public float GetDelay() {
		return (float)delay/1000;
	}

}
