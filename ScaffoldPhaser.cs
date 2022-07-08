using System.Globalization;
using System.Text.RegularExpressions;
using PhotoshopFiles;

namespace SceneGenerator;

public class ScaffoldPhaser
{
    int Id = 1;


    int GetNextId()
    {
        return Id++;
    }

    private bool OverWriteFlag = false;

    string SafeName(Layer layer)
    {
        return Regex.Replace(layer.Name, "[^\\w0-9]", "");
    }

    string SafeName_str(string spriteName)
    {
        return Regex.Replace(spriteName, "[^\\w0-9]", "");
    }

    public void GeneratePreload(List<Layer> sprites, Layer background, string sceneName, string outPutPath)
    {
        if (File.Exists(outPutPath + "\\Preload" + sceneName + ".js"))
        {
            Console.WriteLine("Preload Exists, delete or rename to generate new");
            return;
        }

        var spriteLoader =
            string.Join("\n", sprites.Select(a =>
            {
                var safe_name = SafeName(a);
                return $@"const _{safe_name} = require( './sprites/{safe_name}/{safe_name}')";
            }));
        var spritenames = string.Join(",", sprites.Select(a => "_" + SafeName((a))));
        var scene = $@"
 
import {{Scene}} from ""phaser"";
    
            {spriteLoader}
            const SpriteNames = [{spritenames}]
            export default class Preload{sceneName} extends Scene{{
        
            constructor(){{
                super('preload_{sceneName}')  
               
            }}
            preload() {{
            
           
                for(const sprite_set of SpriteNames){{
                    for(const sprite_name of sprite_set.default){{
                        this.load.image(  sprite_name[0],  sprite_name[1]);
                    }}
                }}
            }}

   
                create() {{
            this.scene.start('{sceneName}')
        }}

    }}";

        File.WriteAllText(outPutPath + "\\Preload" + sceneName + ".js", scene);
    }

    void GenerateSceneFile(List<Layer> sprites, Layer background, string sceneName, string outPutPath)
    {
        var sceneEventTemplate = $@"import {{SceneEvents}} from ""@/game/BaseClasses/SceneSpriteEvents""
    export default class {SafeName_str(sceneName)}DefaultSceneEvents extends  SceneEvents{{

    }}
         
    
 ";

        var spriteLoader =
            string.Join("\n", sprites.Select(a =>
            {
                var safe_name = SafeName(a);
                return $@"const _{safe_name} = require( './sprites/{safe_name}/{safe_name}')";
            }));


        var spritenames = string.Join(",", sprites.Select(a => "_" + SafeName((a))));
        var scene = $@"
const TextBoxesx= require('./textboxes/index.js')
const TextBoxes = Object.keys(TextBoxesx).map(a=>TextBoxesx[a]);
 
import PlayScene from '@/game/scenes/PlayScene'
    const EventsData = require('./sceneevents/{sceneName}.js');
            {spriteLoader}
            const SpriteNames = [{spritenames}]
            
            export default class {sceneName} extends PlayScene{{
      
            constructor(){{
                super('{sceneName}')  
                this.SceneEvents = new EventsData.default();
            }}
                create(){{
             
                if(this.SpriteList.length !==0)                       
                  this.SpriteList.splice(0,this.SpriteList.length);
           
                
                for(const sprite_name of SpriteNames){{
                        this.createSprite(sprite_name,'{sceneName}');
                }}
              for(const textbox of TextBoxes){{
                  this.createTextBox(textbox);
                    }}
 super.create();
                }}
                

            
        }}";
        if (!Directory.Exists(outPutPath + "\\sceneevents\\"))
        {
            Directory.CreateDirectory(outPutPath + "\\sceneevents\\");
        }

        if (!Directory.Exists(outPutPath + $"textboxes\\ "))
        {
            Directory.CreateDirectory(outPutPath + $"textboxes\\");
        }

        if (OverWriteFlag || !File.Exists(outPutPath + $"textboxes\\index.js"))
        {
            File.WriteAllText(outPutPath + $"textboxes\\index.js", "");
        }

        File.WriteAllText(outPutPath + "\\" + "sceneevents\\" + sceneName + ".js", sceneEventTemplate);
        File.WriteAllText(outPutPath + "\\" + sceneName + ".js", scene);
    }

    public void GenerateSprites(List<Layer> sprites, string sceneName, string outPutPath)
    {
        foreach (var sprite in sprites)
        {
            GenerateSprite(sprite, outPutPath, sceneName);
        }
    }

    public void GenerateSprite(Layer sprite, string outPutPath, string sceneName)
    {
        var safeName = SafeName(sprite);
        var spriteTemplate = $@"
        import _{safeName} from './images/{safeName}.png'
let paths = [
        ['{sceneName + "_" + safeName}', _{safeName}, {GetNextId()}],
      
    ]

      import {{store}} from ""@/store"";
 
   export const event_file = require('./{safeName}_events.js');
 
 
 
export default paths
export let Items = () => paths.map(a => {{
    return {{Name: a[0], Id: a[2]}}
}});
export const Name = ""{safeName}""
export const Id = 22;
export const GroupId = ""{string.Join(",", sprite.Channels.Select(a => a.ID))}""
export const Visible = {sprite.Visible.ToString().ToLower()}
export const X = {sprite.Rect.X};
export const Y = {sprite.Rect.Y};
export const Height = {sprite.Rect.Height};
export const Interactive= true
export const Width = {sprite.Rect.Width};
export const Alpha = {sprite.Opacity / 255f};
export let Scale = 1;

 
";
        var SceneEventTemplate = @$"import SceneSpriteEvents from ""@/game/BaseClasses/SceneSpriteEvents"";

   export const exportClass = class _{safeName}DefaultSceneEvents extends  SceneSpriteEvents{{

    }}
        export default exportClass
";
        var EventList = @$"import  DefaultSceneEvents from './events/{safeName}'
 
          export const Default   = DefaultSceneEvents
   
";

        if (!Directory.Exists(outPutPath + $"sprites\\{safeName}\\images\\"))
        {
            Directory.CreateDirectory(outPutPath + $"sprites\\{safeName}\\images\\");
        }

        if (!Directory.Exists(outPutPath + $"sprites\\{safeName}\\events\\"))
        {
            Directory.CreateDirectory(outPutPath + $"sprites\\{safeName}\\events\\");
        }

        if (OverWriteFlag || !File.Exists($"{outPutPath}sprites\\{safeName}\\events\\{safeName}.js"))
        {
            File.WriteAllText($"{outPutPath}sprites\\{safeName}\\events\\{safeName}.js", SceneEventTemplate);
        }
        else
        {
            Console.WriteLine("Sprite Default Event Exists, delete or rename to generate new " + safeName);
        }

        if (OverWriteFlag || !File.Exists($"{outPutPath}sprites\\{safeName}\\{safeName}_events.js"))
            File.WriteAllText($"{outPutPath}sprites\\{safeName}\\{safeName}_events.js", EventList);
        else
        {
            Console.WriteLine(
                "Sprite Default Event_List (_events) Exists, delete or rename to generate new " + safeName);
        }

        StoreData(sprite, $"{outPutPath}sprites\\{safeName}\\images\\{safeName}.png");
        if (OverWriteFlag || !File.Exists($"{outPutPath}sprites\\{safeName}\\{safeName}.js"))
            File.WriteAllText($"{outPutPath}sprites\\{safeName}\\{safeName}.js", spriteTemplate);
        else
        {
            Console.WriteLine("Sprite Exists, delete or rename to generate new " + safeName);
        }
    }

    public void GenerateTextBox(int X, int Y, string Name, string SceneName, string scenepath)
    {
        var def = $@"import {{{Name}TextBox}} from ""@/game/scenes/{SceneName}/textboxes/{Name}/{Name}TextBox"";

    export const Name = ""{Name}""
    export const Id = ""{GetNextId()}""
    export const X = {X}
    export const Y = {Y}
    export const FontFamily = 'Georgia, ""Goudy Bookletter 1911"", Times, serif'
    export const Interactive = false;
    export const EventsData = {{}}
    export const TextClass = {Name}TextBox;";
        var stub = $@"import TextBox from ""@/game/BaseClasses/TextBox"";

    export class {Name}TextBox extends TextBox {{
        

    }}";


        var indexJS = "";
        if (File.Exists(scenepath + $"textboxes\\index.js"))
        {
            indexJS = File.ReadAllText(scenepath + $"textboxes\\index.js");
        }


        if (indexJS.Contains(Name + ".js"))
        {
            Console.WriteLine("Text Box Exists, delete out of textbox  / index.js to overwrite");
            return;
        }

        if (Name == "DebugTextBox")
        {
            if (!indexJS.Contains("DebugTextBox"))
                indexJS +=
                    @" export const DebugTextBoxTextBox = require( ""@/game/BaseClasses/DebugTextBox/DebugTextBox"");";
        }
        else
        {
            indexJS += $@"

export const {Name}TextBox = require( ""./{Name}/{Name}.js"");

";
            
            if (!Directory.Exists(scenepath + $"textboxes\\{Name}\\"))
            {
                Directory.CreateDirectory(scenepath + $"textboxes\\{Name}\\");
            }
            File.WriteAllText(scenepath + $"textboxes\\{Name}\\{Name}.js", def);
            File.WriteAllText(scenepath + $"textboxes\\{Name}\\{Name}TextBox.js", stub);
        }

        File.WriteAllText(scenepath + $"textboxes\\index.js", indexJS);
    
    }

    void StoreData(Layer layer, string filename)
    {
        try
        {
            using System.Drawing.Image myPsdImage = ImageDecoder.DecodeImage(layer);
            if (myPsdImage != null)
            {
                myPsdImage.Save(filename);
            }
            else
            {
                Console.WriteLine("No image data");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw ex;
        }
    }

    public void GenerateQuest(string outPath, string qName)
    {
        if (!outPath.EndsWith(Path.DirectorySeparatorChar)) outPath += Path.DirectorySeparatorChar;
        var spriteDirectories = Directory.GetDirectories(outPath);
        Console.WriteLine(spriteDirectories);
        var spriteList = spriteDirectories.Select(a => a.Split("\\").Last()).ToList();
        foreach (var sprite in spriteList)
        {
            var dir = outPath + sprite;
            if (!dir.EndsWith(Path.DirectorySeparatorChar)) dir += Path.DirectorySeparatorChar;
            var eventsJs = dir + sprite + "_events.js";
            AddEventToSprite(qName, dir, eventsJs, sprite);
        }
    }

    private void AddEventToSprite(string qName, string dir, string eventsJs, string sprite)
    {
        var eventfile = $"{dir}events\\{qName}.js";
        var existing = "";
        var shouldWrite = false;
        if (File.Exists(eventsJs))
        {
            shouldWrite = true;
            existing = File.ReadAllText(eventsJs);
        }

        if (!existing.Contains($"{qName}.js"))
        {
            existing += @$"import  _{qName} from './events/{qName}.js'
 
                export const {qName} = _{qName}";
            shouldWrite = true;
            Console.WriteLine($"Sprite {sprite} event added to auto wire up");
        }

        if (OverWriteFlag || shouldWrite)
        {
            Console.WriteLine($"Sprite {sprite} event added to auto wire up");
            File.WriteAllText(eventsJs, existing);
        }

        if (OverWriteFlag || !File.Exists(eventfile))
        {
            var questEvent = @$"import  {{exportClass}} from './{sprite}';
                                 export class {SafeName_str(sprite)}QuestEvent extends  exportClass{{

                                }}
                  export default {SafeName_str(sprite)}QuestEvent;
";
            File.WriteAllText(eventfile, questEvent);
            Console.WriteLine($"Sprite {sprite} event file written");
        }
        else
        {
            Console.WriteLine("Sprite event file exists, delete to overwrite");
        }
    }

    public void GenerateScene(string outPutPath, string sceneName, string inputPsd)
    {
        PsdFile file = new();
        Layer background = null;
        file.Load(inputPsd);
        if (file.Layers.Count == 0)
        {
            Console.WriteLine("No layers, do nothing");
            return;
        }

        if (!Directory.Exists(outPutPath + sceneName))
        {
            Directory.CreateDirectory(outPutPath + sceneName);
        }

        int i = 0;

        var sprites = file.Layers.Where(a => !a.Name.ToLower().Contains("layer"))
            .Where(b => !b.Rect.IsEmpty).Select(
                a =>
                {
                    a.Name = MakeUpperFace(a.Name);
                    return a;
                }).ToList();

        for (var index = 0; index < sprites.Count; index++)
        {
            var sprite = sprites[index];


            foreach (var layer in sprites.Skip(index + 1).Where(a => a.Name == sprite.Name))
            {
                layer.Name += "" + i++;
            }
        }

        background = sprites.First();

        GeneratePreload(sprites, background, sceneName, outPutPath + sceneName + "\\");
        GenerateSceneFile(sprites, background, sceneName, outPutPath + sceneName + "\\");
        GenerateSprites(sprites, sceneName, outPutPath + sceneName + "\\");
        GenerateTextBox(0, 0, "DebugTextBox", sceneName, outPutPath + sceneName + "\\");
        sprites.Clear();
    }

    string MakeUpperFace(string s)
    {
        TextInfo info = CultureInfo.CurrentCulture.TextInfo;
        return Regex.Replace(Regex.Replace(
                info.ToTitleCase(
                    Regex.Replace(s.Trim(), "_", " ")), " ", ""), "\\.psd$", "",
            RegexOptions.IgnoreCase);
    }
}