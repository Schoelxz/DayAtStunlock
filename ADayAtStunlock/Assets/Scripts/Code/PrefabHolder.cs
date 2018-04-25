using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    [System.Serializable]
    public class PrefabObject
    {
        public GameObject prefabObject;
        public string key;
    }

    private static PrefabHolder m_myInstance;
    public static PrefabHolder MyInstance
    {
        get { return m_myInstance; }
    }

    [SerializeField]
    private PrefabObject[] prefabs;
    private Dictionary<string, GameObject> dictionaryPrefab = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> PrefabDictionary
    {
        get { return dictionaryPrefab; }
    }

    private void Awake()
    {
        if (m_myInstance == null)
            m_myInstance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        foreach (var item in prefabs)
        {
            dictionaryPrefab.Add(item.key, item.prefabObject);
        }
    }

}
