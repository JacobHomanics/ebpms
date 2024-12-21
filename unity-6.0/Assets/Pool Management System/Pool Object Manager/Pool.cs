using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace JacobHomanics.Core.PoolManagement
{
    public class Pool : MonoBehaviour
    {
        public PoolObject poolObject;

        [Header("Configuration")]
        public int numToInitialize;
        public bool initializeOnStart;

        public enum OverflowType { Expandable, Recycable }
        public OverflowType overflowType;

        [Tooltip("DO NOT alter during gameplay")]
        public enum StorageType { Queue, List }
        public StorageType storageType;

        [Header("Events")]
        public UnityEvent<Pool, PoolObject[]> OnInitialize = new();
        public UnityEvent<Pool, PoolObject[]> OnSpawn = new();
        public UnityEvent<Pool, PoolObject> OnDespawn = new();

        public List<PoolObject> standbyObjectsList = new();
        public Queue<PoolObject> standbyObjectsQueue = new();

        public List<PoolObject> activeObjects = new();

        public IEnumerable<PoolObject> AllObjects
        {
            get
            {
                foreach (var obj in standbyObjectsList) yield return obj;
                foreach (var obj in standbyObjectsQueue) yield return obj;
                foreach (var obj in activeObjects) yield return obj;
            }
        }

        private void Start()
        {
            if (initializeOnStart) Initialize();
        }

        [ContextMenu("Initialize")]
        public void Initialize()
        {
            Initialize(numToInitialize);
        }

        public void Initialize(int initializeCount)
        {
            PoolObject[] initializedObjects = new PoolObject[initializeCount];


            for (int i = 0; i < initializeCount; i++)
            {
                var instance = Instantiate(poolObject);
                instance.Initialize(this);
                initializedObjects[i] = instance;

                if (storageType == StorageType.Queue)
                    standbyObjectsQueue.Enqueue(instance);
            }

            if (storageType == StorageType.List)
                standbyObjectsList.AddRange(initializedObjects.ToList());

            OnInitialize?.Invoke(this, initializedObjects);
        }

        [ContextMenu("Terminate")]
        public void Terminate()
        {
            foreach (var obj in AllObjects)
            {
                obj.Terminate();
            }

            if (storageType == StorageType.Queue)
                standbyObjectsQueue.Clear();

            activeObjects.Clear();
        }

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            Spawn(1);
        }

        [ContextMenu("Spawn 5")]
        public void Spawn5()
        {
            Spawn(5);
        }


        public void Spawn(int num)
        {
            var spawnedObjects = new List<PoolObject>(num); // More flexible than arrays

            for (int i = 0; i < num; i++)
            {

                ICollection collection = null;

                if (storageType == StorageType.Queue)
                    collection = standbyObjectsQueue;
                if (storageType == StorageType.List)
                    collection = standbyObjectsList;

                if (collection.Count <= 0)
                {
                    if (overflowType == OverflowType.Expandable)
                    {
                        Initialize(1);
                    }
                    else if (overflowType == OverflowType.Recycable)
                    {
                        if (storageType == StorageType.Queue)
                            standbyObjectsQueue.Enqueue(activeObjects[0]);

                        if (storageType == StorageType.List)
                            standbyObjectsList.Add(activeObjects[0]);

                        activeObjects.Remove(poolObject);
                    }
                }

                PoolObject obj = null;

                if (storageType == StorageType.Queue)
                    obj = standbyObjectsQueue.Dequeue();


                if (storageType == StorageType.List)
                {
                    obj = standbyObjectsList[0];
                    standbyObjectsList.RemoveAt(0);
                }

                Spawn(obj);
                spawnedObjects.Add(obj);
            }

            OnSpawn?.Invoke(this, spawnedObjects.ToArray());
        }

        private void Spawn(PoolObject poolObject)
        {
            activeObjects.Add(poolObject);
            poolObject.Spawn();
        }

        // public void Despawn(PoolObject poolObject)
        // {
        //     if (storageType == StorageType.Queue)
        //         standbyObjectsQueue.Enqueue(poolObject);

        //     if (storageType == StorageType.List)
        //         standbyObjectsList.Add(poolObject);

        //     activeObjects.Remove(poolObject);
        //     poolObject.Despawn();
        //     OnDespawn?.Invoke(this, poolObject);
        // }
    }
}
