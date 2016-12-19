#define USE_NGUI_2X

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIGamePopUp : MonoBehaviour {
	
	public const float UI_DELAY = 0.75f;
	public const float UI_TRAVEL_DIST = 0.6f;
	
	public GameObject content;
	public PopUpType panelType;
	public PopUpType openFromPanelOnly;

	protected Animator animator;
	
	public static Dictionary <PopUpType, UIGamePopUp> panels;

	void Awake() {
		PopUpManager.GetInstance().AddPanel(panelType, this);
		Init();
	}

	virtual protected void Init() {
		animator = this.gameObject.GetComponent<Animator>();
	}

	//TEMP TODO SGUNITY REFACTORING
//	virtual public void InitialiseWithBuilding(Building building) {
//	}
	
	virtual public void Show() {
		//TEMP TODO SGUNITY REFACTORING
		//if (panelType == PopUpType.DEFAULT && GridView.Instance != null) GridView.Instance.NormalMode(); 
//		if (panelType == PopUpType.PLACE_BUILDING && GridView.Instance != null) GridView.Instance.BuildingMode(); 
//		if (panelType == PopUpType.EDIT_PATHS && GridView.Instance != null) GridView.Instance.PathMode(); 
		if (activePanel == this) {
			StartCoroutine(DoReShow());
		} else if (activePanel == null || activePanel.panelType == openFromPanelOnly || openFromPanelOnly == PopUpType.NONE) {
			if (activePanel != null) activePanel.Hide ();
			StartCoroutine(DoShow());
			activePanel = this;
		}
	}

	virtual public void Hide() {
		StartCoroutine(DoHide());
	}

	public static UIGamePopUp activePanel {
		get { return PopUpManager.GetInstance().activePanel; }
		set { PopUpManager.GetInstance().activePanel = value; }
	}

	/**
	 * Reshow the panel (i.e. same panel but for a different object/building).
	 */ 
	virtual protected IEnumerator DoReShow() {
	//	iTween.MoveTo(content, hidePosition, UI_DELAY);
		yield return new WaitForSeconds(UI_DELAY / 3.0f);
	//	iTween.MoveTo(content, showPosition, UI_DELAY);
	}
	
	
	/**
	 * Show the panel.
	 */ 
	virtual protected IEnumerator DoShow() {
		yield return new WaitForSeconds(UI_DELAY / 3.0f);
		content.SetActive (true);
		if(animator!=null)
			animator.SetTrigger("Opening");

		#if USE_NGUI_3X
		yield return true;
		GetComponent<UIPanel>().Refresh();
#endif
	//	iTween.MoveTo(content, showPosition, UI_DELAY);
	}
	
	/**
	 * Hide the panel. 
	 */
	virtual protected IEnumerator DoHide() {
	//	iTween.MoveTo(content, hidePosition, UI_DELAY);
		yield return new WaitForSeconds(UI_DELAY / 3.0f);
		content.SetActive (false);
	}

    public virtual void BackButtonClicked() {
        PopUpManager.GetInstance().ShowPanel (PopUpType.DEFAULT);
    }
}
