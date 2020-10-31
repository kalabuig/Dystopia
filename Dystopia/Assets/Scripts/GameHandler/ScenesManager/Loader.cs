using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    //Dummy class (we need a monobehaviour to use coroutines)
    private class LoadingMonoBehaviour : MonoBehaviour { }

    public enum Scene {
        LoadingScene,
        MainMenuScene,
        GameScene,
        SettingsScene
    }

    private static Action onLoaderCallBack;

    private static AsyncOperation loadingAsyncOperation;

    public static void Load(Scene scene) {
        //Configurin the call back to load the scene
        onLoaderCallBack = () => { //When callback ...
            GameObject loadingGO = new GameObject("LoadingGO");
            loadingGO.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene)); //Load the scene
        };
        //Load the loading scene
        SceneManager.LoadScene(Scene.LoadingScene.ToString()); 
    }

    private static IEnumerator LoadSceneAsync(Scene scene) {
        yield return null; //wait 1 frame before loading
        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString()); //Load the scene
        while(loadingAsyncOperation.isDone == false) {
            yield return null; //wait until loaded
        }
    }

    public static float GetProgress() {
        if(loadingAsyncOperation != null) {
            return loadingAsyncOperation.progress; //return de progres (between 0 and 1)
        } else {
            return 1f;
        }
    }

    public static void LoaderCallBack() {
        //Launching the call back just one time (launched after the first update)
        if(onLoaderCallBack != null) {
            onLoaderCallBack();
            onLoaderCallBack = null;
        }
    }
}
