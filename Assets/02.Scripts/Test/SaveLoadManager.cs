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

    public delegate void SaveHandler(int slotNo);   //반환형이 void이고 매개변수가 int 1개인 메서드를 대리
    public static event SaveHandler OnSaveData;     //static을 써서 전역 변수로 사용


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
