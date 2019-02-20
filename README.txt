Load world data from XML file
	Monsters
	Quests, Recipes, and Traders
Use Aspect-Oriented Programming for PropertyChanged code
UI changes
	RichTextBox fix
	Tooltips for quests and recipes
	DataTemplate for Player data
	Progress bar for health (and experience?)
Save/load player game data
Complex battle logic

---
Hi David,

Have you reached lesson 09.2, where we add the Trade screen, and set its DataContext? If so, I’ll give you some hints to (hopefully) help you. If you try it, and have problems, let me know so I can give you more details.

Steps
1. Create a new window in the WPFUI project. I’ll call it CharacterCreation.xaml, for this example.
2. Modify WPFUI\App.xaml. Change the StartupUri attribute from “MainWindow.xaml” to CharacterCreation.xaml”. This will make CharacterCreation the first screen displayed when you run the program.
3. Put your character name textbox in this CharacterCreation, and a “Start Game” button with a “Click” event handler function in CharacterCreation.xaml.cs.
4. Modify the constructor in MainWindow.xaml.cs to accept a string parameter for the characterName.
5. In the CharacterCreation “Click” function, you’ll do the code similar to creating the TradeScreen – except, instead of setting the DataContext, pass in the character’s name when you instantiate the MainWindow object.
6. In the CharacterCreation “Click” function, do “Show()”, instead of “ShowDialog”. Then, add “this.Close();” (which will close the CharacterCreation window).
7. Modify the GameSession constructor to accept a string parameter of the character’s name.
8. In the MainWindow.xaml.cs constructor, set “_gameSession” to “new GameSession(characterName)”
9. In the GameSession constructor, use the characterName parameter when you instantiate the CurrentPlayer object.

Let me know if any of that isn’t clear, or if it doesn’t work (I did that from memory, and might have missed something).
---