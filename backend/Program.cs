

using backend.DTOes;
using backend.Entities;
using backend.Infrastructure;
using backend.Services;
using static backend.Entities.Unit;


/*var unit = new Unit(
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
    side: Unit.UnitSide.Yours,
    type: "HumanSwordsman"
);
var healer = new Healer(100, attacks:new List<Attack>{ new Attack(
            attackName: "Fireball",
            attackType: Attack.AttackTypes.Ranged,
            damageType: Attack.DamageTypes.Fire,
            damage: 25,
            attacksAmount: 3
        )}, 2, UnitSide.Yours, 25, "healer");
List<Unit> otherUnits = new List<Unit>{unit,healer};
List<Leader> leaders = new List<Leader>();
Units units = new(otherUnits, leaders);
File.Delete("units.json");
MyJsonSerializer.writeToJson(units,"units.json");
UnitGenerator.initialGeneration();*/


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
Dictionary<(int, int),UnitDTO> UnitMap = new();
builder.Services.AddSingleton(UnitMap);
builder.Services.AddSingleton<UnitGenerator>();
builder.Services.AddControllers();
var app=builder.Build();
app.UseCors("AllowAll");
app.MapControllers();



app.Run("http://localhost:5000");