using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SceneLoader : MonoBehaviour
{
    /* TEST 
    void Start()
    {
        StartCoroutine(LoadSceneAsync(1, 0, 0));
        StartCoroutine(LoadSceneAsync(1, 0, 1));
        StartCoroutine(LoadSceneAsync(1, 1, 0));
        StartCoroutine(LoadSceneAsync(1, 0, -1));
        StartCoroutine(LoadSceneAsync(1, -1, 0));
    }

    IEnumerator LoadSceneAsync(int sceneIndex, int x, int y) {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        //Wait until scene loaded
        while (loadOperation.isDone == false)
            yield return new WaitForEndOfFrame();
        //Scene loaded, get the root reference
        Scene loadedScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
        Transform root = loadedScene.GetRootGameObjects()[0].transform;
        //Set position of the scene
        root.position = new Vector3(x*1000,y*1000,0);
    }
    */
}
