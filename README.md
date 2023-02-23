# Scaffold
autogenerates project files for Phaser Project

# Create Sprites and a scene from a psd
> This will walk the psd and slice every layer out and create a scene for you

`dotnet program.cs GenerateScene AbsPathToScene SceneName AbsPathToPsd`

# Create A Quest
> This will create a quest in a scene ( a game state within game states)

`dotnet program.cs GenerateQuest AbsPathToSceneSprites QuestName`

# Add a text box
> This will add a text bot to a scene at a location. Useful for dialog.

`dotnet program.cs GenerateTextBox AbsPathToScene SceneName TextboxName X Y`
