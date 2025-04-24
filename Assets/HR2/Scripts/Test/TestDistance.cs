using UnityEngine;

public class TestDistance : MonoBehaviour
{
    [SerializeField] private Transform distanceObjectToTest;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, distanceObjectToTest.position) < 100f)
        {
            Debug.Log("Yey");
        }
    }
}
