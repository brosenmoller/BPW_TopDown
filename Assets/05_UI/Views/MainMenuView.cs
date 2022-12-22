using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainMenuView : UIView
{
    [SerializeField] private Button PlayButton;

    public override void Initialize()
    {
        PlayButton.onClick.AddListener(() => SceneManager.LoadScene(0));
    }
}
