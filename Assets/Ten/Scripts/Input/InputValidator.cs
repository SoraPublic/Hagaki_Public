using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Input/InputValidator")]
public class InputValidator : TMP_InputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        // アルファベットでなければ入力しない
        if (!isLatin(ch))
        {
            return '\0';
        }

        // アルファベットの大文字なら小文字に変換する
        ch = char.ToLower(ch);

        // 現在のテキストを更新
        text += ch;

        // 現在のカーソル位置を更新
        pos++;

        return ch;
    }

    private bool isLatin(char c)
    {
        return ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
    }
}

