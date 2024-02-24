using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loading_bar;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            loadingScreen = GameObject.Find("Canvas").transform.GetChild(3).gameObject;
            loading_bar = loadingScreen.transform.GetChild(0).GetComponent<Image>();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {

        }
    }

    public delegate void SaveHandler(int slotNo);   //��ȯ���� void�̰� �Ű������� int 1���� �޼��带 �븮
    public static event SaveHandler OnSaveData;     //static�� �Ἥ ���� ������ ���


    public void SaveData(int slotNo)
    {
        OnSaveData(slotNo);
    }


    public void LoadData(int slotNo)
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            PlayerPrefs.SetInt("SlotNo", slotNo);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.SetInt("SlotNo", slotNo);
            loadingScreen.SetActive(true);
            StartCoroutine(loadsceneLate());
        }
    }

    IEnumerator loadsceneLate()
    {
        AsyncOperation sceneload = SceneManager.LoadSceneAsync(2);
        while (!sceneload.isDone)
        {
            loading_bar.fillAmount = sceneload.progress;
            yield return null;
        }
        loadingScreen.SetActive(false);
    }
}
