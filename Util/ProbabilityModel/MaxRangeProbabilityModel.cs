using UnityEngine;

/**
 * Model starts at 1
 * Execution can happen randomly between min index and max index.
 */
public class MaxRangeProbabilityModel {
	protected int minIndex;
	protected int maxIndex;
	
	protected int currentIndex;
	protected int targetIndex;

	public MaxRangeProbabilityModel(int minIndex, int maxIndex) {
		this.maxIndex = maxIndex;
		this.minIndex = minIndex;
		this.reset();
	}
	
	public void reset() {
		currentIndex = 0;
		targetIndex = minIndex + Random.Range (0, maxIndex - minIndex + 1);
	}
	
	/**
	 * Increments the current index
	 */
	public void increment() {
		currentIndex++;
	}
	
	/**
	 * boolean specifying whether an action can be taken or not as per this model
	 * @return
	 */
	public bool canExecute() {
		return (currentIndex == targetIndex) || (currentIndex >= maxIndex) ;
	}
	
}
