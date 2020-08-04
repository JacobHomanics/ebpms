using JacobHomanics.Core.PoolManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	public PoolEntityManager pem;

    // Start is called before the first frame update
    void Start()
    {
		pem.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
