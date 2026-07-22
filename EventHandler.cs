using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp096Events;
using LabApi.Events.Arguments.Scp106Events;
using LabApi.Events.Arguments.Scp173Events;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using PlayerRoles;

namespace Serpents_hand;

public class EventHandler
{
    public HashSet<Player> SerpentPlayers { get; } = new HashSet<Player>();     //hashlist containing all the serpents currently alive
    
    private double CalculatedChance()
    {
        int playercount = Player.ReadyList.Count();
        
        if(playercount <= 10) return 2.0;            
        else if(playercount >= 50) return 15.0;      
        else
        {
            return (20 + (playercount - 10) * (70.0 / 20.0));
        }

    }
    
    public void selectHand(string who)
    {
        var spectList = Player.ReadyList.Where(pl => pl.Role == RoleTypeId.Spectator).ToList();
        Player targetplayer = Player.Get(who);
        
        if (who == "all")
        {
            foreach (var selectedspect in spectList)
            {
                spawnHand(selectedspect);
            }
        }
        else
        {
            if(targetplayer != null) spawnHand(targetplayer);
        }
    }
    
    public void selectHand()
    {
        var spectList = Player.ReadyList.Where(pl => pl.Role == RoleTypeId.Spectator).ToList();
    
        System.Random r = new System.Random();
        int i = 0; 

        while (i < (int)CalculatedChance())
        { 
            if (spectList.Count == 0) break; 
            
            int randomIndex = r.Next(spectList.Count);
            Player selectedSpectator = spectList[randomIndex];
            spawnHand(selectedSpectator);
            spectList.RemoveAt(randomIndex);
            
            i++;
        }
    }
    
    public void spawnHand(Player player)
    {
        player.SetRole(RoleTypeId.ChaosConscript);
        player.Position = new UnityEngine.Vector3(38.092f, 300.960f, -38.499f);
        player.MaxHealth = Serpents_handPlugin.Instance.Config.health;
        player.Health = Serpents_handPlugin.Instance.Config.health;
        player.ClearInventory();    
        player.AddItem(ItemType.ArmorHeavy);
        player.AddItem(ItemType.GunA7);
        player.AddItem(ItemType.GunAK);
        player.AddItem(ItemType.Painkillers);
        player.AddItem(ItemType.Medkit);
        player.AddItem(ItemType.KeycardChaosInsurgency);
        player.AddItem(ItemType.SCP268);
        player.AddItem(ItemType.GrenadeFlash);
        player.AddAmmo(ItemType.Ammo762x39, 120);
        player.SendBroadcast("You are a <color=green>Serpent's hand conscript</color>", 10);
        player.SendHint("Help <color=red>SCPs</color>.", 10);
        
        if (!SerpentPlayers.Contains(player)) SerpentPlayers.Add(player);
    }

    public void OnPlayerHurting(PlayerHurtingEventArgs ev)
    {
        if(ev.Attacker is null || ev.Player is null) return;
        
        if (ev.Attacker.Faction == Faction.SCP && SerpentPlayers.Contains(ev.Player))
        {
            ev.IsAllowed = false;
            ev.Attacker.SendHint("You can't hurt <color=green>Serpent's hand</color>.", 5);
        }
        else if (SerpentPlayers.Contains(ev.Attacker) && ev.Player.Faction == Faction.SCP)
        {
            ev.IsAllowed = false;
            ev.Attacker.SendHint("You can't hurt <color=red>SCPs</color>", 5);
        }
    }

    public void Adding096target(Scp096AddingTargetEventArgs ev)         //stops serpent hand from enraging 096
    {
        if(SerpentPlayers.Contains(ev.Target)) ev.IsAllowed = false;
    }

    public void Scp173blink(Scp173AddingObserverEventArgs ev)           //stops serpents from activating blink on 173
    {
        if(SerpentPlayers.Contains(ev.Target)) ev.IsAllowed = false;
    }

    public void Scp106Teleport(Scp106TeleportingPlayerEvent ev)
    {
        if(SerpentPlayers.Contains(ev.Target)) ev.IsAllowed = false;
    }



    public void OnPlayerDied(PlayerDeathEventArgs ev)
    {
        if (ev.Player != null && SerpentPlayers.Contains(ev.Player))
        {
            SerpentPlayers.Remove(ev.Player);
        }
    }
    
    public void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        if (ev.Player != null && SerpentPlayers.Contains(ev.Player))
        {
            SerpentPlayers.Remove(ev.Player);
        }
    }

    public void ChangingRole(PlayerChangingRoleEventArgs ev)
    {
        if (ev.Player != null && SerpentPlayers.Contains(ev.Player))
        {
            SerpentPlayers.Remove(ev.Player);
        }
    }

}