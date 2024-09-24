using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    Lobby,
    InGame,
}

//SingletonBehaviour ���
public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    public void LoadScene(SceneType sceneType)
    {
        Logger.Log($"{sceneType} scene loading...");
        Time.timeScale = 1f; // �Ͻ����� ���� �� Ÿ�ӽ������� 0�� �� ���� �ְ�
        //���� ��ȹ�� Ÿ�ӽ������� 1�� �ƴ� ��쵵 ���� �� �ֱ� ������
        //���� �ε����� �� Ÿ�ӽ������� �ʱ�ȭ ����
        SceneManager.LoadScene(sceneType.ToString());
    }
    
    public void ReloadScene()
    {
        Logger.Log($"{SceneManager.GetActiveScene().name} scene loading...");

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //�񵿱�� �ε��ϴ� �Լ�
    public AsyncOperation LoadSceneAsync(SceneType sceneType)
    {
        Logger.Log($"{sceneType} Scene async loading...");
        Time.timeScale = 1f;

        return SceneManager.LoadSceneAsync(sceneType.ToString());
    }
}
