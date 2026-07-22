using CommandSystem;

namespace Serpents_hand;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SerpentCommand : ICommand
{
    public string Command => "spawnhand";  
    public string[] Aliases => ["hand"];
    public string Description => "spawns the Serpent's hand";
    
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)  
    {  
            
        if (arguments.Count == 0)  
        {
            Serpents_handPlugin.Instance.EventHandler.selectHand();
            response = "Serpent's hand spawned successfully!";  
            return true;
        }
        
        if (arguments[0].ToLower() == "all")
        {
            Serpents_handPlugin.Instance.EventHandler.selectHand("all");
            response = "All spectators has been spawned into Serpent's hand!";
            return true;
        }
        else
        {
            Serpents_handPlugin.Instance.EventHandler.selectHand(arguments[0]);
            response = $"Spawned {arguments[0]} as Serpent's hand.";  
            return true;
        }
    }  
}