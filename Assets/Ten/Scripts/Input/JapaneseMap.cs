using System.Collections.Generic;

public static class JapaneseMap
{
    public static string CombineString(string str)
    {
        var ret = new List<string>();
        string uni, bi, tri, quar;
        string returnValue = string.Empty;
        string vowel = "aiueon";

        int i = 0;
        while (i < str.Length)
        {
            uni = str[i].ToString();
            if (i + 1 < str.Length)
            {
                if (str[i] == str[i + 1] && !vowel.Contains(str[i]))
                {
                    i++;
                    ret.Add("xtu");
                    continue;
                }
                else if (str[i] == 'n' && !vowel.Contains(str[i + 1]) && str[i + 1] != 'y')
                {
                    i++;
                    ret.Add("nn");
                    continue;
                }
                bi = str[i].ToString() + str[i + 1].ToString();
            }
            else
            {
                bi = string.Empty;
            }

            if(i + 2 < str.Length)
            {
                tri = str[i].ToString() + str[i + 1].ToString() + str[i + 2].ToString();
            }
            else
            {
                tri = string.Empty;
            }
            
            if (i + 3 < str.Length)
            {
                quar = str[i].ToString() + str[i + 1].ToString() + str[i + 2].ToString() + str[i + 3];
            }
            else
            {
                quar = string.Empty;
            }

            if (map.ContainsKey(quar))
            {
                i += 4;
                ret.Add(quar);
                continue;
            }
            else if (map.ContainsKey(tri))
            {
                i += 3;
                ret.Add(tri);
                continue;
            }
            else if (map.ContainsKey(bi))
            {
                i += 2;
                ret.Add(bi);
                continue;
            }
            else
            {
                i++;
                ret.Add(uni);
                continue;
            }
        }

        foreach (string item in ret)
        {
            try
            {
                returnValue += map[item];
            }
            catch
            {
                returnValue += item;
                continue;
            }
        }

        return returnValue;
    }

    /*
     * あ行・・・28
     * か行・・・47
     * さ行・・・36
     * た行・・・58＋1
     * な行・・・10
     * は行・・・54
     * ま行・・・10
     * や行・・・9
     * ら行・・・10
     * わ行・・・7(6)
     */
    private static Dictionary<string, string> map = new Dictionary<string, string>()
    {
        // あ行
        {"a", "あ"},
        {"i", "い"},
        {"yi", "い"},
        {"u", "う"},
        {"wu", "う"},
        {"whu", "う"},
        {"e", "え"},
        {"o", "お"},
        {"xa", "ぁ"},
        {"la", "ぁ"},
        {"xi", "ぃ"},
        {"li", "ぃ"},
        {"lyi", "ぃ"},
        {"xyi", "ぃ"},
        {"xu", "ぅ"},
        {"lu", "ぅ"},
        {"xe", "ぇ"},
        {"le", "ぇ"},
        {"lye", "ぇ"},
        {"xo", "ぉ"},
        {"lo", "ぉ"},
        {"wha", "うぁ"},
        {"ye", "いぇ"},
        {"whi", "うぃ"},
        {"wi", "うぃ"},
        {"whe", "うぇ"},
        {"we", "うぇ"},
        {"who", "うぉ"},
        // か行
        {"ka", "か"},
        {"ca", "か"},
        {"ki", "き"},
        {"ku", "く"},
        {"cu", "く"},
        {"qu", "く"},
        {"ke", "け"},
        {"ko", "こ"},
        {"co", "こ"},
        {"xka", "ヵ"},
        {"lka", "ヵ"},
        {"xke", "ヶ"},
        {"lke", "ヶ"},
        {"ga", "が"},
        {"gi", "ぎ"},
        {"gu", "ぐ"},
        {"ge", "げ"},
        {"go", "ご"},
        {"kya", "きゃ"},
        {"kyi", "きぃ"},
        {"kyu", "きゅ"},
        {"kye", "きぇ"},
        {"kyo", "きょ"},
        {"qya", "くゃ"},
        {"qyu", "くゅ"},
        {"qyo", "くょ"},
        {"qwa", "くぁ"},
        {"qa", "くぁ"},
        {"qwi", "くぃ"},
        {"qi", "くぃ"},
        {"qyi", "くぃ"},
        {"qwu", "くぅ"},
        {"qwe", "くぇ"},
        {"qe", "くぇ"},
        {"qye", "くぇ"},
        {"qwo", "くぉ"},
        {"qo", "くぉ"},
        {"gya", "ぎゃ"},
        {"gyi", "ぎぃ"},
        {"gyu", "ぎゅ"},
        {"gye", "ぎぇ"},
        {"gyo", "ぎょ"},
        {"gwa", "ぐぁ"},
        {"gwi", "ぐぃ"},
        {"gwu", "ぐぅ"},
        {"gwe", "ぐぇ"},
        {"gwo", "ぐぉ"},
        // さ行
        {"sa", "さ"},
        {"si", "し"},
        {"ci", "し"},
        {"shi", "し"},
        {"su", "す"},
        {"se", "せ"},
        {"so", "そ"},
        {"za", "ざ"},
        {"zi", "じ"},
        {"ji", "じ"},
        {"zu", "ず"},
        {"ze", "ぜ"},
        {"zo", "ぞ"},
        {"sya", "しゃ"},
        {"sha", "しゃ"},
        {"syi", "しぃ"},
        {"syu", "しゅ"},
        {"shu", "しゅ"},
        {"sye", "しぇ"},
        {"she", "しぇ"},
        {"syo", "しょ"},
        {"sho", "しょ"},
        {"zya", "じゃ"},
        {"jya", "じゃ"},
        {"ja", "じゃ"},
        {"zyi", "じぃ"},
        {"jyi", "じぃ"},
        {"zyu", "じゅ"},
        {"jyu", "じゅ"},
        {"ju", "じゅ"},
        {"zye", "じぇ"},
        {"jye", "じぇ"},
        {"je", "じぇ"},
        {"zyo", "じょ"},
        {"jyo", "じょ"},
        {"jo", "じょ"},
        // た行
        {"ta", "た"},
        {"ti", "ち"},
        {"chi", "ち"},
        {"tu", "つ"},
        {"tsu", "つ"},
        {"te", "て"},
        {"to", "と"},
        {"ltu", "っ"},
        {"xtu", "っ"},
        {"ltsu", "っ"},
        {"xtsu", "っ"},
        {"tya", "ちゃ"},
        {"cha", "ちゃ"},
        {"cya", "ちゃ"},
        {"tyi", "ちぃ"},
        {"cyi", "ちぃ"},
        {"tyu", "ちゅ"},
        {"chu", "ちゅ"},
        {"cyu", "ちゅ"},
        {"tye", "ちぇ"},
        {"che", "ちぇ"},
        {"cye", "ちぇ"},
        {"tyo", "ちょ"},
        {"cho", "ちょ"},
        {"cyo", "ちょ"},
        {"tsa", "つぁ"},
        {"tsi", "つぃ"},
        {"tse", "つぇ"},
        {"tso", "つぉ"},
        {"tha", "てゃ"},
        {"thi", "てぃ"},
        {"thu", "てゅ"},
        {"the", "てぇ"},
        {"tho", "てょ"},
        {"twa", "とぁ"},
        {"twi", "とぃ"},
        {"twu", "とぅ"},
        {"twe", "とぇ"},
        {"two", "とぉ"},
        {"da", "だ"},
        {"di", "ぢ"},
        {"du", "づ"},
        {"de", "で"},
        {"do", "ど"},
        {"dya", "ぢゃ"},
        {"dyi", "ぢぃ"},
        {"dyu", "ぢゅ"},
        {"dye", "ぢぇ"},
        {"dyo", "ぢょ"},
        {"dha", "でゃ"},
        {"dhi", "でぃ"},
        {"dhu", "でゅ"},
        {"dhe", "でぇ"},
        {"dho", "でょ"},
        {"dwa", "どぁ"},
        {"dwi", "どぃ"},
        {"dwu", "どぅ"},
        {"dwe", "どぇ"},
        {"dwo", "どぉ"},
        // な行
        {"na", "な"},
        {"ni", "に"},
        {"nu", "ぬ"},
        {"ne", "ね"},
        {"no", "の"},
        {"nya", "にゃ"},
        {"nyi", "にぃ"},
        {"nyu", "にゅ"},
        {"nye", "にぇ"},
        {"nyo", "にょ"},
        // は行
        {"ha", "は"},
        {"hi", "ひ"},
        {"hu", "ふ"},
        {"fu", "ふ"},
        {"he", "へ"},
        {"ho", "ほ"},
        {"ba", "ば"},
        {"bi", "び"},
        {"bu", "ぶ"},
        {"be", "べ"},
        {"bo", "ぼ"},
        {"pa", "ぱ"},
        {"pi", "ぴ"},
        {"pu", "ぷ"},
        {"pe", "ぺ"},
        {"po", "ぽ"},
        {"hya", "ひゃ"},
        {"hyi", "ひぃ"},
        {"hyu", "ひゅ"},
        {"hye", "ひぇ"},
        {"hyo", "ひょ"},
        {"fya", "ふゃ"},
        {"fyu", "ふゅ"},
        {"fyo", "ふょ"},
        {"fwa", "ふぁ"},
        {"fa", "ふぁ"},
        {"fwi", "ふぃ"},
        {"fi", "ふぃ"},
        {"fyi", "ふぃ"},
        {"fwu", "ふぅ"},
        {"fwe", "ふぇ"},
        {"fye", "ふぇ"},
        {"fe", "ふぇ"},
        {"fo", "ふぉ"},
        {"fwo", "ふぉ"},
        {"bya", "びゃ"},
        {"byi", "びぃ"},
        {"byu", "びゅ"},
        {"bye", "びぇ"},
        {"byo", "びょ"},
        {"va", "う゛ぁ"},
        {"vi", "う゛ぃ"},
        {"vu", "う゛"},
        {"ve", "う゛ぇ"},
        {"vo", "う゛ぉ"},
        {"vya", "う゛ゃ"},
        {"vyi", "う゛ぃ"},
        {"vyu", "う゛ゅ"},
        {"vye", "う゛ぇ"},
        {"vyo", "う゛ょ"},
        {"pya", "ぴゃ"},
        {"pyi", "ぴぃ"},
        {"pyu", "ぴゅ"},
        {"pye", "ぴぇ"},
        {"pyo", "ぴょ"},
        // ま行
        {"ma", "ま"},
        {"mi", "み"},
        {"mu", "む"},
        {"me", "め"},
        {"mo", "も"},
        {"mya", "みゃ"},
        {"myi", "みぃ"},
        {"myu", "みゅ"},
        {"mye", "みぇ"},
        {"myo", "みょ"},
        // や行
        {"ya", "や"},
        {"yu", "ゆ"},
        {"yo", "よ"},
        {"xya", "ゃ"},
        {"xyu", "ゅ"},
        {"xyo", "ょ"},
        {"lya", "ゃ"},
        {"lyu", "ゅ"},
        {"lyo", "ょ"},
        // ら行
        {"ra", "ら"},
        {"ri", "り"},
        {"ru", "る"},
        {"re", "れ"},
        {"ro", "ろ"},
        {"rya", "りゃ"},
        {"ryi", "りぃ"},
        {"ryu", "りゅ"},
        {"rye", "りぇ"},
        {"ryo", "りょ"},
        // わ行
        {"wa", "わ"},
        {"xwa", "ゎ"},
        {"lwa", "ゎ"},
        {"wo", "を"},
        {"nn", "ん"},
        {"xn", "ん"}
    };
}

