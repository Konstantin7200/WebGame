using backend.Entities;
using backend.Infrastructure;
using backend.Services;
using static backend.Templates.UnitTemplate;


/*var unit = new UnitTemplate(
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
    movesAmount: 2,
    type: "HumanSwordsman"
);
var healer = new HealerTemplate(100, attacks:new List<Attack>{ new Attack(
            attackName: "Fireball",
            attackType: Attack.AttackTypes.Ranged,
            damageType: Attack.DamageTypes.Fire,
            damage: 25,
            attacksAmount: 3
        )}, 2, 25, "healer");
List<UnitTemplate> otherUnits = new List<UnitTemplate>{unit,healer};
List<LeaderTemplate> leaders = new List<LeaderTemplate>();
Units units = new(otherUnits, leaders);
File.Delete("units.json");
MyJsonSerializer.writeToJson(units,"units.json");
*/

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
Dictionary<(int, int),Unit> UnitMap = new();
Unit lastUnit =new Unit();
Turn currentTurn =new Turn();
builder.Services.AddSingleton(lastUnit);
builder.Services.AddSingleton(UnitMap);
builder.Services.AddSingleton(currentTurn);
builder.Services.AddSingleton<UnitGenerator>();
builder.Services.AddControllers();
var app=builder.Build();
app.UseCors("AllowAll");
app.MapControllers();



app.Run("http://localhost:5000");