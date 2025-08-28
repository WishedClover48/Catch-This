using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletPool : MonoBehaviour, IPunPrefabPool
{
    public GameObject prefab;
    public int initialSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = PhotonNetwork.Instantiate(prefab.name, transform.position, transform.rotation);
            obj.name = prefabId;
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(false);

        return obj;
    }

    public void Destroy(GameObject gameObject)
    {
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }

    private void a()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = PhotonNetwork.Instantiate(prefab.name,transform.position,transform.rotation);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        // Register this pool with Photon
        PhotonNetwork.PrefabPool = this;
    }
}