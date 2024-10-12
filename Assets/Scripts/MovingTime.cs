using UnityEngine.UI;
using UnityEngine;

public class MovingTime : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject present;
    [SerializeField] private GameObject past1;
    [SerializeField] private GameObject past2;

    public void GoInPresent()
    {        
        GoNext(present);
    }
    public void GoInPast1()
    {
        GoNext(past1);
    }
    public void GoInPast2()
    {
        GoNext(past2);
    }

    private void GoNext(GameObject where)
    {
        player.transform.position = where.transform.position;
        player.transform.rotation = where.transform.rotation;
    }

}
