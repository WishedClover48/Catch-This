using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.Analytics;

public class ProfanityFilter : MonoBehaviour
{
    private List<string> bannedWords = new List<string> {
    "tonto",
    "idiota",
    "imb�cil",
    "est�pido",
    "bruto",
    "loco",
    "grosero",
    "feo",
    "BOCA",
    "perezoso",
    "mentiroso",
    "cobarde",
    "in�til",
    "pesado",
    "molesto",
    "torpe",
    "ignorante",
    "vago",
    "chismoso",
    "desagradable",
    "maleducado",
    "odioso",
    "cruel",
    "necio",
    "burro",
    "desleal",
    "travieso",
    "problem�tico",
    "hostil",
    "insensible",
    "despreciable",
    "s�dico",
    "manipulador",
    "tramposo",
    "deshonesto",
    "antip�tico",
    "tremendo",
    "malicioso",
    "c�nico",
    "irresponsable",
    "vanidoso",
    "ego�sta",
    "pesimista",
    "fan�tico",
    "rencoroso",
    "gru��n",
    "grosero",
    "temerario",
    "desatento",
    "maledicente",
    "prepotente",
    "insolente",
    "arrogante",
    "desconsiderado",
    "imb�cil",
    "torpe",
    "ignorante",
    "brusco",
    "irritable",
    "cabezota",
    "insoportable",
    "malhumorado",
    "grosero",
    "vengativo",
    "cruel",
    "taca�o",
    "necio",
    "injusto",
    "mentiroso",
    "perezoso",
    "desordenado",
    "inconstante",
    "rudo",
    "pesado",
    "tramposo",
    "desleal",
    "desagradable",
    "despistado",
    "desconfiado",
    "insensato",
    "ignorante",
    "obstinado",
    "torpe",
    "inmaduro",
    "ruidoso",
    "malicioso",
    "prepotente",
    "desobediente",
    "descarado",
    "grosero",
    "mentiroso",
    "c�nico",
    "fanfarr�n",
    "arrogante",
    "engre�do",
    "hostil",
    "odioso",
    "despreciable",
    "malintencionado",
    "manipulador",
    "tramposo",
    "deshonesto",
    "ego�sta",
    "vanidoso",
    "fan�tico",
    "rencoroso",
    "cruel"
    };

    public string CensorText(string input)
    {
        string censoredText = input;
        string CensorInProgres;
        foreach (string word in bannedWords)
        {
            CensorInProgres = censoredText;
            string pattern = @"\b" + Regex.Escape(word) + @"\b";
            string replacement = new string('*', word.Length);

            censoredText = Regex.Replace(censoredText, pattern, replacement, RegexOptions.IgnoreCase);
            if (CensorInProgres != censoredText)
            {
                
                try
                {
                    Debug.Log("ProfanityFound: " + word);
                    //ProfanityFound(word);
                    FlagChat(1,1, "Boca!");
                }
                catch (Exception e)
                {
                    Debug.LogError("Error with ProfanityFound");
                    Debug.LogException(e);
                }
            }
        }

        return censoredText;
    }
    
    public void FlagChat(int playerID, int roomID, string slur)
    {
        ChatFlaggedEvent evt = new ChatFlaggedEvent{ PlayerID = playerID, RoomID = roomID, Slur = slur};
        
        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();
        
        Debug.Log("Sent chat");
    }
}
