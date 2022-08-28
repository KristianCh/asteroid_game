using Run;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    [SerializeField]
    private RunData _RunData;
    public RunData RunData => _RunData;
    
    public static RunManager Instance;
    
    void Start()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
