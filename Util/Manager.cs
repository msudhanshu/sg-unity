using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//namespace SgUnity
//{
/**
 * Abstract class extended by all "manager" type objects.
 * Managers have only one instance in a scene and this class helps to enforce that.
 */ 
public abstract class Manager <T> : MonoBehaviour where T : Manager <T> {
	
	/** Static reference to this manager. */
	protected static T manager;
	/** Has this manager been initialised. */
	protected bool initialised = false;

    protected List<ManagerDependency> dependencies;
	
	/**
	 * Get the instance of the manager class or create if one has not yet been created.
	 * 
	 * @returns An instance of the manager class.
	 */ 
	public static T GetInstance(){
		if (manager == null) Create();
		return manager;
	}
	
	/**
	 * Create a new game object and attach an instance of the manager.
	 */ 
	protected static void Create() {
		GameObject go = new GameObject();
		go.name = typeof(T).Name;
		manager = (T) go.AddComponent(typeof(T));
	}
	
	/**
	 * If there is already a manager destroy self, else initialise and assign to the static reference.
	 */ 
	void Awake(){
		if (manager == null) {
			if (!initialised) {
				Init();
			}
			manager = (T) this;
		} else if (manager != this) {
			Destroy(gameObject);	
		}
	}

	public bool IsReady() {
		return initialised;
	}
	/**
	 * Initialise the manager. Override this to perform initialisation in sub-classes. Note this
	 * initialisation should never rely on another manager instance being available as it is called from Awake().
	 */ 
	virtual protected void Init() {
            StartCoroutine(StartDependencyCompleted(ManagerDependency.NONE));
	}

    IEnumerator StartDependencyCompleted(ManagerDependency dependency){
		yield return 0; // Done so that the first Init gets called after all Awakes.
		DependencyCompleted(dependency);
	}


    void DependencyCompleted(ManagerDependency dependency) {
//        Debug.Log("Dependency Completed called :"+dependency);
		if (dependencies == null) {
			PopulateDependencies();
		}
		if (dependencies != null && dependencies.Contains(dependency)) {
			dependencies.Remove(dependency);
		}

		if ((dependencies == null || dependencies.Count == 0) && !initialised) {
			//Debug.LogError("Initializing " + this.gameObject.name);
			initialised = true;
			StartInit();
		}
	}

	/* Implement this function */
	abstract public void StartInit();

	/* Implement this function */
	abstract public void PopulateDependencies();

}

//}