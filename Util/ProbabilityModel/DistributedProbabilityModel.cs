using System.Collections.Generic;
using UnityEngine;

public class DistributedProbabilityModel {

	private static float DEFAULT_MAX_INDEX = 100;
	private List<float> probabilityDist;
	private float maxIndex;
	private bool normalized;
	
	/**
	 * If we do not normalize, the max_index will be taken as DEFAULT_MAX_INDEX
	 * @param probabilityDist
	 * @param normalize
	 */
	public DistributedProbabilityModel(List<float> probabilityDist, bool normalized){
		this.probabilityDist = probabilityDist;
		this.normalized = normalized;
		if(normalized){
			maxIndex = 0;
			foreach(float val in probabilityDist)
				maxIndex += val; 
		}else
			maxIndex = DEFAULT_MAX_INDEX;
	}
	
	public List<float> getProbabilityDist(){
		return this.probabilityDist;
	}
	
	public int getNextIndex(){
		int listPosition = 0;
		float startIndex = 0;
		float selectedIndex = Random.Range(0.0f, 1.0f) * maxIndex;
		foreach(float probability in probabilityDist){
			float endIndex = startIndex + probability;
			if(selectedIndex>=startIndex && selectedIndex<endIndex){
				return listPosition;
			}
			startIndex = endIndex;
			listPosition++;
		}
		return -1;
	}
	
}
