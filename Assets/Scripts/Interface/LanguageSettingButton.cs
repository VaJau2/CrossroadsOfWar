using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LanguageSettingButton : MonoBehaviour
{
    [SerializeField] public Text[] languageLabels;
    [SerializeField] public string[] languageCodes;
    [SerializeField] public UnityEvent onLanguageChanged;

    public void Start()
    {
        UpdateLabelsText();
    }
    
    public void OnLanguageChange()
    {
        LanguageSetting.UpdateLanguage();
        onLanguageChanged.Invoke();
        UpdateLabelsText();
    }

    private void UpdateLabelsText()
    {
        for (var i = 0; i < languageLabels.Length; i++)
        {
            languageLabels[i].text = Lang.GetInstance().GetText(languageCodes[i]);
        }
    }
}
