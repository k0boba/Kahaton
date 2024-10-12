using Unity.VisualScripting;
using UnityEngine;

public class TriggerMovingTime : MonoBehaviour
{
    public MovingTime MovingTime;
    private bool into = false;
    [SerializeField] private GameObject buttonsWhere;

    void Update()
    {
        if (into)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                MovingTime.GoInPresent();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MovingTime.GoInPast1();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                MovingTime.GoInPast2();
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            into = true;
            buttonsWhere.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            into = false;
            buttonsWhere.SetActive(false);
        }
    }
}
