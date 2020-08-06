using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{
	public UnityEvent OnThresholdReached;

    void Update()
    {
		transform.Translate(Vector3.forward * Time.deltaTime);
		if (transform.position.z > 3f)
			OnThresholdReached?.Invoke();
	}
}
