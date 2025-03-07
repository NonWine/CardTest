using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    [SerializeField] private GoogleDriveDataController _driveDataController;

    private void Awake()
    {
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        yield return new WaitUntil(() => _driveDataController.IsInitialized);
        SceneManager.LoadScene("CardGame");
    }
}