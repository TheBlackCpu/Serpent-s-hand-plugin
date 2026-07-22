namespace Serpents_hand;

using System.ComponentModel;

/// <summary>
/// The config of this plugin.
/// </summary>
public sealed class Serpents_handConfig
{
    /// <summary>
    /// Gets or sets a value indicating whether debug logging should be enabled.
    /// </summary>
    [Description("Enable debugging mode, useful to enable when needing to debug for any issue.")]
    public bool Debug { get; set; } = false;
    
    //SHAND health
    public int health = 100;
    
    //SHAND spect spawn chance
    public int spawnchance = 60;
    
    //SHAND spawn limit
    public int spawnlimit = 15;
}