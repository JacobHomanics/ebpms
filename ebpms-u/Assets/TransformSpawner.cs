using JacobHomanics.Core.PoolManagement;
using UnityEngine;

public class TransformSpawner : MonoBehaviour
{
    public void OnSpawn(PoolEntityManager manager, PoolEntity entity)
	{
		entity.transform.position = transform.position;
	}
}
