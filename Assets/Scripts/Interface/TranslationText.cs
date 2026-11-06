using UnityEngine;
using UnityEngine.UI;

public class TranslationText : MonoBehaviour
{
   [System.Serializable]
   public struct TranslationEntry
   {
      public Text field;
      public string code;
   }
   
   [SerializeField] private TranslationEntry[] translations;

   [SerializeField] private bool loadLanguageOnAwake;
   [SerializeField] public LanguageSettingButton LanguageSettingButton;

   private void Awake()
   {
      if (loadLanguageOnAwake)
      {
         LanguageSetting.LoadLanguage();
      }
   }
   
   private void Start()
   {
      if (LanguageSettingButton != null)
      {
         LanguageSettingButton.onLanguageChanged.AddListener(OnLanguageChanged);
      }

      OnLanguageChanged();
   }
   
   private void OnLanguageChanged()
   {
      foreach (var entry in translations)
      {
         entry.field.text = Lang.GetInstance().GetText(entry.code);
      }
   }
}
