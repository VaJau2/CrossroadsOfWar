using UnityEngine;

public class LanguageSetting : MonoBehaviour
{
    public enum Language
    {
        English,
        Russian
    }
    
    public static Language CurrentLanguage = Language.Russian;

    public static void UpdateLanguage()
    {
        CurrentLanguage = CurrentLanguage == Language.English 
            ? Language.Russian
            : Language.English;
        
        SaveLanguage();
        Lang.GetInstance().LoadLocalization();
    }

    public static void LoadLanguage()
    {
        var prefsLang = PlayerPrefs.GetString("lang");
        
        if (string.IsNullOrEmpty(prefsLang))
        {
            var sysLang = Application.systemLanguage;
            prefsLang = sysLang == SystemLanguage.Russian ? "ru" : "en";
        }
        
        CurrentLanguage = prefsLang == "ru" 
            ? Language.Russian 
            : Language.English;
    }
    
    public static void SaveLanguage()
    {
        PlayerPrefs.SetString(
            "lang",
            CurrentLanguage == Language.Russian
                ? "ru"
                : "en"
        );
    }
}
