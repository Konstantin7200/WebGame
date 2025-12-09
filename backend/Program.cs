using backend.Entities;
using backend.GameConfig;
using backend.Infrastructure;
using backend.Services;
using backend.Templates;
using static backend.Templates.UnitTemplate;


/*var unit = new HealerTemplate(
    healingPower:15,
    health: 100,
    attacks: new List<Attack>
    {
        new Attack(
            attackName: "Sword Slash",
            attackType: Attack.AttackTypes.Melee,
            damageType: Attack.DamageTypes.Slash,
            damage: 15,
            attacksAmount: 2
        )
    },
    resistances:new Dictionary<Attack.DamageTypes, double> {
        { Attack.DamageTypes.Arcane,0 },
        { Attack.DamageTypes.Fire,0 },
        { Attack.DamageTypes.Smash,0 },
        { Attack.DamageTypes.Slash,0 },
        { Attack.DamageTypes.Pierce,0 }
    },
    movesAmount: 2,
    type: "HumanSwordsman"
);
List<UnitTemplate> otherUnits = new List<UnitTemplate> { unit};
List<UnitTemplate> leaders = new List<UnitTemplate>();
UnitTemplates units = new(otherUnits, leaders);
File.Delete("units.json");
MyJsonSerializer.writeToJson(units, "units.json");*/


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
PlayerConfig playerConfig = new();
GameState gameState = new(new(), new(), new());
builder.Services.AddSingleton(gameState);
builder.Services.AddSingleton(playerConfig);
builder.Services.AddControllers();
var app=builder.Build();
app.UseCors("AllowAll");
app.MapControllers();



app.Run("http://localhost:5000");