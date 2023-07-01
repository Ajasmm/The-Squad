using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameInput gameInput;


    #region Sigleton
    public static GameManager Instance { get { return GetGameManagerInstance(); } }
    private static GameManager instance;
    private static bool isGameQuit = false;

    private static GameManager GetGameManagerInstance()
    {
        if (isGameQuit)
            return null;

        if(instance == null)
        {
            GameObject gameManager = new GameObject("GameManager");
            gameManager.AddComponent<GameManager>();
        }
        return instance;
    }
    #endregion

    #region UnityMagic Functions
    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else if (instance != this)
        {
            Destroy(gameObject);
        }

        gameInput = new GameInput();

    }
    private void OnDestroy()
    {
        if(instance == this)
            isGameQuit = true;
    }
    #endregion
}
