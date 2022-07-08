// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Text.RegularExpressions;
using PhotoshopFiles;
using SceneGenerator;

string MakeUpperFace(  string s)
{
    TextInfo info = CultureInfo.CurrentCulture.TextInfo;
    return Regex.Replace(Regex.Replace(
            info.ToTitleCase(
                Regex.Replace(s, "_", " ")), " ", ""), "\\.psd$", "",
        RegexOptions.IgnoreCase);
}

void GenerateSceneUsage()
{
    Console.WriteLine("GenerateScene AbsPathToScene SceneName AbsPathToPsd ");
}

void GenerateQuestUsage()
{
    Console.WriteLine("GenerateQuest AbsPathToSceneSprites QuestName ");
}

void GenerateTextBoxUsage()
{
    Console.WriteLine(" GenerateTextBox AbsPathToScene SceneName TextboxName X Y");
}




return;

if (args.Length < 1)
{
    Console.WriteLine("Usage:");
    GenerateSceneUsage();
    GenerateQuestUsage();
    GenerateTextBoxUsage();
    return;
}

string Mode = args[0];


var scaffoldPhaser = new ScaffoldPhaser();

switch (Mode)
{
    case "GenerateScene":
        if (args.Length < 4)
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("SceneGenerator GenerateScene AbsPathToScene SceneName AbsPathToPsd ");
            return;
        }

        var outPutPath = args[1];  ;
        var sceneName = args[2];
        var inputPsd = args[3];

        scaffoldPhaser.GenerateScene(outPutPath, sceneName, inputPsd);
        break;
    case "GenerateQuest":
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("SceneGenerator GenerateQuest AbsPathToSceneSprites QuestName ");
            return;
        }

        var outPath = args[1];
        var QName = args[2];
        scaffoldPhaser.GenerateQuest(outPath, QName);



        break;
    case "GenerateTextBox":
        if (args.Length <= 5)
        {
            Console.WriteLine("Usage: ");
            GenerateTextBoxUsage();
            return;
        }

        var scenePAth = args[1];
        var Sce = args[2];
        var TextBoxName = args[3];
        var X = int.Parse(args[4]);
        var Y = int.Parse(args[5]);
        scaffoldPhaser.GenerateTextBox(X, Y, TextBoxName, Sce, scenePAth);
        break;
}


public class PlayScene
{
    public string ClassDef { get; set; }
}
