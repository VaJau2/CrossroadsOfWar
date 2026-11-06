using System;
using System.Collections.Generic;
using UnityEngine;

public class Lang
{
    // Папка в Resources, где лежат файлы локализации вида <категория>_<язык>.json
    private const string LocalizationFolder = "Localization";

    public const string DefaultCategory = "localization";

    private static Lang instance;

    // Категория -> (ключ -> текст) для текущего языка
    private readonly Dictionary<string, Dictionary<string, string>> localizedText =
        new Dictionary<string, Dictionary<string, string>>();

    private Lang()
    {
        LoadLocalization();
    }

    public static Lang GetInstance()
    {
        return instance ?? (instance = new Lang());
    }

    public string GetText(string key, string category = DefaultCategory)
    {
        Dictionary<string, string> texts;
        string value;

        if (key != null
            && category != null
            && localizedText.TryGetValue(category, out texts)
            && texts.TryGetValue(key, out value))
        {
            return value;
        }

        return "[KEY NOT FOUND: " + category + "/" + key + "]";
    }

    // Перевод по коду, а если код не задан - исходный текст (для полей вида text + code)
    public string GetTextOr(string key, string fallback, string category = DefaultCategory)
    {
        return string.IsNullOrEmpty(key) ? fallback : GetText(key, category);
    }

    public void LoadLocalization()
    {
        localizedText.Clear();

        var suffix = LanguageSetting.CurrentLanguage == LanguageSetting.Language.Russian ? "_ru" : "_en";

        foreach (var jsonFile in Resources.LoadAll<TextAsset>(LocalizationFolder))
        {
            if (!jsonFile.name.EndsWith(suffix))
            {
                continue;
            }

            var category = jsonFile.name.Substring(0, jsonFile.name.Length - suffix.Length);

            try
            {
                localizedText[category] = FlatJsonParser.Parse(jsonFile.text, "файле локализации " + jsonFile.name);
            }
            catch (FormatException e)
            {
                Debug.LogError("Ошибка в файле локализации " + LocalizationFolder + "/" + jsonFile.name + ".json: " + e.Message);
            }
        }

        if (!localizedText.ContainsKey(DefaultCategory))
        {
            Debug.LogError("Не найден файл локализации " + LocalizationFolder + "/" + DefaultCategory + suffix + ".json!");
        }
    }
}
