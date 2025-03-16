using UnityEngine;

public class CreatorBase : MonoBehaviour
{
    [SerializeField] protected bool createOnStart = false;
                     
    [SerializeField] protected bool pauseOnFinish = false;
                     
    [SerializeField] protected int populationCount = 10000000;
                     
    [SerializeField] protected int framesToRun = 3;
                     
    [SerializeField] protected int threadCount = 16;


}
