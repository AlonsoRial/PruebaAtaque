using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnitSelectonManager.Instance.allUnitsList.Add(gameObject);
    }

    private void OnDestroy()
    {
        UnitSelectonManager.Instance.allUnitsList.Remove(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
