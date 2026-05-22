namespace LifeyLife.Core.Models;

/// <summary>
/// Mirrors the PostgreSQL dare_category enum.
/// Lowercase names are intentional — Dapper maps the DB string values by name.
/// </summary>
public enum DareCategory
{
    physical   = 0,   // → Strength
    social     = 1,   // → Charisma
    mental     = 2,   // → Intelligence
    creative   = 3,   // → Dexterity
    wellness   = 4,   // → Vitality
    discipline = 5    // → Willpower
}
