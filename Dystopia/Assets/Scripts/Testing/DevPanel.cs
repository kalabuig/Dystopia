using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevPanel : MonoBehaviour
{
    public void ToogleActive() {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}
