using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallBack : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update() {
        //it will launche the call back just one time (launched after the first screen update)
        if(isFirstUpdate) {
            isFirstUpdate = false;
            Loader.LoaderCallBack(); //launch the callback
        }
    }
}
