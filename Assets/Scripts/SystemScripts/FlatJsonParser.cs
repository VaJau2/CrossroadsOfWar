using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

// Разбор плоского JSON-объекта вида {"ключ": "текст", ...} в словарь.
// Newtonsoft.Json в Unity 2018.2 (рантайм .NET 3.5) недоступен, а JsonUtility не умеет читать словари.
// Значения - только строки. Висячая запятая перед } допускается (как в Newtonsoft).
// При ошибке в данных бросает FormatException с номером строки.
public static class FlatJsonParser
{
    // sourceName - имя файла, только для сообщений в консоль
    public static Dictionary<string, string> Parse(string json, string sourceName)
    {
        var result = new Dictionary<string, string>();
        var pos = 0;

        // TextAsset.text может начинаться с BOM
        if (json.Length > 0 && json[0] == '﻿')
        {
            pos++;
        }

        SkipWhitespace(json, ref pos);
        Expect(json, ref pos, '{');

        while (true)
        {
            SkipWhitespace(json, ref pos);

            if (pos < json.Length && json[pos] == '}')
            {
                pos++;
                break;
            }

            var key = ReadString(json, ref pos);
            SkipWhitespace(json, ref pos);
            Expect(json, ref pos, ':');
            SkipWhitespace(json, ref pos);
            var value = ReadString(json, ref pos);

            if (result.ContainsKey(key))
            {
                Debug.LogWarning("Повторяющийся ключ \"" + key + "\" в " + sourceName + ", строка " + GetLine(json, pos));
            }

            result[key] = value;

            SkipWhitespace(json, ref pos);

            if (pos < json.Length && json[pos] == ',')
            {
                pos++;
                continue;
            }

            Expect(json, ref pos, '}');
            break;
        }

        SkipWhitespace(json, ref pos);

        if (pos < json.Length)
        {
            throw Error(json, pos, "лишние символы после конца объекта");
        }

        return result;
    }

    private static string ReadString(string json, ref int pos)
    {
        Expect(json, ref pos, '"');

        var sb = new StringBuilder();

        while (true)
        {
            if (pos >= json.Length)
            {
                throw Error(json, pos, "незакрытая строка");
            }

            var c = json[pos++];

            if (c == '"')
            {
                return sb.ToString();
            }

            if (c != '\\')
            {
                sb.Append(c);
                continue;
            }

            if (pos >= json.Length)
            {
                throw Error(json, pos, "незакрытая строка");
            }

            var escaped = json[pos++];

            switch (escaped)
            {
                case '"':
                case '\\':
                case '/':
                    sb.Append(escaped);
                    break;
                case 'b':
                    sb.Append('\b');
                    break;
                case 'f':
                    sb.Append('\f');
                    break;
                case 'n':
                    sb.Append('\n');
                    break;
                case 'r':
                    sb.Append('\r');
                    break;
                case 't':
                    sb.Append('\t');
                    break;
                case 'u':
                    int code;

                    if (pos + 4 > json.Length
                        || !int.TryParse(json.Substring(pos, 4), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out code))
                    {
                        throw Error(json, pos, "неверная последовательность \\u");
                    }

                    sb.Append((char) code);
                    pos += 4;
                    break;
                default:
                    throw Error(json, pos - 1, "неизвестная escape-последовательность \\" + escaped);
            }
        }
    }

    private static void SkipWhitespace(string json, ref int pos)
    {
        while (pos < json.Length && char.IsWhiteSpace(json[pos]))
        {
            pos++;
        }
    }

    private static void Expect(string json, ref int pos, char expected)
    {
        if (pos >= json.Length)
        {
            throw Error(json, pos, "ожидался символ '" + expected + "', а файл закончился");
        }

        if (json[pos] != expected)
        {
            throw Error(json, pos, "ожидался символ '" + expected + "', а встретился '" + json[pos] + "'");
        }

        pos++;
    }

    private static FormatException Error(string json, int pos, string message)
    {
        return new FormatException(message + " (строка " + GetLine(json, pos) + ")");
    }

    private static int GetLine(string json, int pos)
    {
        var line = 1;

        for (var i = 0; i < pos && i < json.Length; i++)
        {
            if (json[i] == '\n')
            {
                line++;
            }
        }

        return line;
    }
}
