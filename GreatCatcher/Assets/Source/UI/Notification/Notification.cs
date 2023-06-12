using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Notification : MonoBehaviour
{
    [SerializeField] private float _activeSeconds;
    
    protected abstract void Open();

    protected abstract void Close();
    
    protected IEnumerator NotificationShown(Notification notification)
    {
        var waitForSeconds = new WaitForSeconds(_activeSeconds);
        notification.Open();
        yield return waitForSeconds;
        notification.Close();
    }
}
