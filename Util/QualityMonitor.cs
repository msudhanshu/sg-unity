#define HACKONMOBILE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QualityMonitor : MonoBehaviour {

	float updateInterval = 0.5f;
    private double lastInterval; // Last interval end time
    private int frames = 0; // Frames over current interval
	internal float fps; // Current FPS

	//public static QualityMonitor instance { get { return Singleton<QualityMonitor>.instance; } }	
	
    public virtual void Start()
    {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }

    public virtual void Update()
    {
     /*   if (instance != this)
        {
            return;
        }
*/
		++frames;
		double timeNow = Time.realtimeSinceStartup;

		if( timeNow > lastInterval + updateInterval )
		{
			fps = (float)(frames / (timeNow - lastInterval));
//            float actualInterval = (float)(timeNow - lastInterval);
			frames = 0;
			lastInterval = timeNow;
		}

	}
}
