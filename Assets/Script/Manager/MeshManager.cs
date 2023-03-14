using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour
{
    static MeshManager instance;
    public static MeshManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("@MeshManager").AddComponent<MeshManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null || instance == this)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(this);
    }
    public Mesh[] meshes;
    public Material[] materials;
}
