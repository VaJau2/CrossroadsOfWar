using UnityEngine;
using UnityEngine.UI;

public class TranslationDropdown : MonoBehaviour
{
	[SerializeField] private string[] optionCodes;

	[SerializeField] private Dropdown dropdown;
	
	private void Start()
	{
		OnLanguageChanged();
	}
   
	private void OnLanguageChanged()
	{
		for (var i = 0; i < optionCodes.Length; i++)
		{
			dropdown.options[i].text = Lang.GetInstance().GetText(optionCodes[i]);
		}
	}
}
