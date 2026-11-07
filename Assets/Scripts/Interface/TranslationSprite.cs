using UnityEngine;

public class TranslationSprite : MonoBehaviour
{
	[SerializeField] private SpriteRenderer sprite;
	[SerializeField] private Sprite enSprie;
	
	private void Start()
	{
		OnLanguageChanged();
	}
   
	private void OnLanguageChanged()
	{
		if (LanguageSetting.CurrentLanguage == LanguageSetting.Language.English)
		{
			sprite.sprite = enSprie;
		}
	}
}
