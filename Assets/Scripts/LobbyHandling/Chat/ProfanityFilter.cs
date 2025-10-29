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
    "imbécil",
    "estúpido",
    "bruto",
    "loco",
    "grosero",
    "feo",
    "BOCA",
    "perezoso",
    "mentiroso",
    "cobarde",
    "inútil",
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
    "problemático",
    "hostil",
    "insensible",
    "despreciable",
    "sádico",
    "manipulador",
    "tramposo",
    "deshonesto",
    "antipático",
    "tremendo",
    "malicioso",
    "cínico",
    "irresponsable",
    "vanidoso",
    "egoísta",
    "pesimista",
    "fanático",
    "rencoroso",
    "gruñón",
    "grosero",
    "temerario",
    "desatento",
    "maledicente",
    "prepotente",
    "insolente",
    "arrogante",
    "desconsiderado",
    "imbécil",
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
    "tacaño",
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
    "cínico",
    "fanfarrón",
    "arrogante",
    "engreído",
    "hostil",
    "odioso",
    "despreciable",
    "malintencionado",
    "manipulador",
    "tramposo",
    "deshonesto",
    "egoísta",
    "vanidoso",
    "fanático",
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
                ProfanityFound(word);
            }
        }

        return censoredText;
    }
    
    public void ProfanityFound(string profanity)
    {
        ProfanityFoundEvent evt = new ProfanityFoundEvent
        {
            Profanity=profanity
        };
        Debug.Log(evt);

        AnalyticsService.Instance.RecordEvent(evt);
        AnalyticsService.Instance.Flush();
    }
}
public class ProfanityFoundEvent : Unity.Services.Analytics.Event
{
    public ProfanityFoundEvent() : base("ProfanityFound")
    {

    }

    public string Profanity { set { SetParameter("ProfanityWord", value); } }
}
