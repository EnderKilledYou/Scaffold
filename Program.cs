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


var Files = new string[]
{
    "Vines.psd",
    "piano_play.psd",
    "school art_class.psd",
    "school english_classroom.psd",
    "school homeroom.psd",
    "school roof_door.psd",
    "home bedroom.psd",
    "school 1east_corridor.psd",
    "school bathroom.psd",
    "school entrance.psd",
    "school locker.psd",
    "school secret_locker.psd",
    "home hall.psd",
    "school 1floor_hall.psd",
    "school cafeteria.psd",
    "school forest_glade.psd",
    "school music_class.psd",
    "home kitchen.psd",
    "school 1west_corridor.psd",
    "school clubroom.psd",
    "school ground_floor.psd",
    "school nurse_room.psd",
    "home_bathroom.psd",
    "school admin_wing.psd",
    "school computer_room.psd",
    "school gym.psd",
    "school roof.psd",
};

 Files.Select(a =>
{
    var replace = MakeUpperFace( a);
    return new
    {
        Name = replace,
        File = @"C:\Users\gamin\Downloads\PSDs-20220701T174121Z-002\PSDs\_Final_Backgrounds\" + a
    };
}).ToList().ForEach(scene =>
{
    Console.WriteLine(scene.File);
    Console.WriteLine(scene.Name);
    var outPutPath = @"C:\forhire\timewizard\phaser-demo\src\game\scenes\";
    var sceneName =scene.Name; //SchoolBathroom
    var inputPsd = scene.File; //"schoolbathroom.psd";
    
    new ScaffoldPhaser().GenerateScene(outPutPath, sceneName, inputPsd);
//     Console.WriteLine($@"import Preload{scene.Name} from 
// ""@/game/scenes/{scene.Name}/Preload{scene.Name}"";
//     import {scene.Name} from
//  ""@/game/scenes/{scene.Name}/{scene.Name}"";");
//     //Console.WriteLine("Preload"+ scene.Name + ","+scene.Name +",");
});


return;
 
    // Console.WriteLine(scene.File);
    // Console.WriteLine(scene.Name);
    // var outPutPath = @"C:\forhire\timewizard\phaser-demo\src\game\scenes\";
    // var sceneName =scene.Name; //SchoolBathroom
    // var inputPsd = scene.File; //"schoolbathroom.psd";
    //
    // new ScaffoldPhaser().GenerateScene(outPutPath, sceneName, inputPsd);
 

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

        var outPutPath = args[1]; // @"C:\forhire\timewizard\phaser-demo\src\game\scenes\";
        var sceneName = args[2]; //SchoolBathroom
        var inputPsd = args[3]; //"schoolbathroom.psd";

        scaffoldPhaser.GenerateScene(outPutPath, sceneName, inputPsd);
        break;
    case "GenerateQuest":
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("SceneGenerator GenerateQuest AbsPathToSceneSprites QuestName ");
            return;
        }

        var outPath = args[1]; // @"C:\forhire\timewizard\phaser-demo\src\game\scenes\";
        var QName = args[2];
        scaffoldPhaser.GenerateQuest(outPath, QName);


        //var eventList = $"{outPath}sprites\\{safeName}\\{safeName}_events.js";
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