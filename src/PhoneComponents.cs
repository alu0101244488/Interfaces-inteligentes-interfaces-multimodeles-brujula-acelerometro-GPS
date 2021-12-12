using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PhoneComponents : MonoBehaviour
{
    
    public GameObject pivot;
    private GUIStyle guiStyle = new GUIStyle(); 

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Input.compass.enabled = true;
       
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
            yield break;

        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 25;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        
        //If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            Input.location.Stop();
            yield break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int trueHeading = (int)Math.Round(Input.compass.trueHeading, 1);
        pivot.transform.rotation = Quaternion.Euler(0, -(trueHeading + 180f), 0);
    }

    void OnGUI()
    {
        //obtener vector normalizado de la aceleración en los tres ejes
        Vector3 dir = Vector3.zero;

        dir.x = Input.acceleration.x;
        dir.y = Input.acceleration.z + 1;
        dir.z = Input.acceleration.y;
        //dir.x = -Input.acceleration.y;
        //dir.z = Input.acceleration.x;

        if (dir.sqrMagnitude > 1)
        dir.Normalize();

        //dir *= Time.deltaTime;

        //convertir en string para mostrar en ui
        string text = "Magnetometer reading: " + Input.compass.trueHeading.ToString();
        text += "\nLocation: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude;
        text += "\nAcceleration" + dir.ToString();
        

        guiStyle.fontSize = 40;
        guiStyle.normal.textColor = Color.white;
        GUILayout.Label(text, guiStyle);
    }
}
