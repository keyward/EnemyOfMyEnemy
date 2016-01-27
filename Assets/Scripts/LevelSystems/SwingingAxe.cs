using UnityEngine;
using System.Collections;

public class SwingingAxe : MonoBehaviour {


    //while angle 

    public float swingDegree;
    public float swingSpeed;
    public float swingDamp;
    public float timeOffset;

	void Start ()
    {
        StartCoroutine(SwingAxe());
	}

	
	

    IEnumerator SwingAxe()
    {
        yield return new WaitForSeconds(timeOffset);

        while (true)
        {
            for(float i=transform.rotation.z; i<swingDegree; i+=swingSpeed)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, swingDegree), Time.deltaTime * swingDamp);
                yield return null;
            }
            for(float i=transform.rotation.z; i>-swingDegree; i-=swingSpeed)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, -swingDegree), Time.deltaTime * swingDamp);
                yield return null;
            } 
        }  
    }
}
