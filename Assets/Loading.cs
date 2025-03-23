using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

public class NavMeshLoader : MonoBehaviour
{
    public string gameSceneName = "GameScene"; // Tên scene chính của bạn

    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
   

        yield return new WaitForSeconds(1f); // Chờ 1 giây để tránh lag

        // Load Scene Chính
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName);
        while (!asyncLoad.isDone)
        {
            yield return null; // Chờ cho đến khi load xong
        }
    }
}
