using LabApi.Events.Handlers;

namespace Serpents_hand;

using System.Reflection;
using HarmonyLib;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;

/// <summary>
/// Main class handling loading API.
/// </summary>
public class Serpents_handPlugin : Plugin<Serpents_handConfig>
{
    /// <summary>
    /// Gets the current instance of this plugin.
    /// </summary>
    public static Serpents_handPlugin Instance { get; private set; } = null!;

    /// <inheritdoc/>
    public override string Name => "Serpents_hand";

    /// <inheritdoc/>
    public override string Description => "Serpent's hand plugin";

    /// <inheritdoc/>
    public override string Author => "Serpent's hand plugin";

    /// <inheritdoc/>
    public override LoadPriority Priority => LoadPriority.Highest;

    /// <inheritdoc/>
    public override Version Version { get; } = Assembly.GetName().Version;

    /// <inheritdoc/>
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;

    /*/// <inheritdoc />
    public override bool IsTransparent => true;*/

    /// <summary>
    /// Gets the harmony to use for the API.
    /// </summary>
    internal static Harmony? Harmony { get; private set; }

    /// <summary>
    /// Gets the Assembly of the API.
    /// </summary>
    internal static Assembly Assembly { get; } = typeof(Serpents_handPlugin).Assembly;

    public EventHandler EventHandler { get; private set; }
    
    /// <inheritdoc/>
    public override void Enable()
    {
        EventHandler = new EventHandler();
        Instance = this;
        Harmony = new Harmony($"{Name}_{DateTime.Now}");

        PlayerEvents.Hurting += EventHandler.OnPlayerHurting;
        PlayerEvents.Death += EventHandler.OnPlayerDied;
        PlayerEvents.Left += EventHandler.OnPlayerLeft;
        PlayerEvents.ChangingRole += EventHandler.ChangingRole;
        Scp096Events.AddingTarget += EventHandler.Adding096target;
        Scp173Events.AddingObserver += EventHandler.Scp173blink;
        Scp106Events.TeleportingPlayer += EventHandler.Scp106Teleport;
    }

    /// <inheritdoc/>
    public override void Disable()
    {
        PlayerEvents.Hurting -= EventHandler.OnPlayerHurting;
        PlayerEvents.Death += EventHandler.OnPlayerDied;
        PlayerEvents.Left += EventHandler.OnPlayerLeft;
        PlayerEvents.ChangingRole -= EventHandler.ChangingRole;
        Scp096Events.AddingTarget -= EventHandler.Adding096target;
        Scp173Events.AddingObserver -= EventHandler.Scp173blink;
        Scp106Events.TeleportingPlayer -= EventHandler.Scp106Teleport;
        
        
        Harmony?.UnpatchAll(Harmony.Id);
        EventHandler = null;
        Instance = null!;
    }
}