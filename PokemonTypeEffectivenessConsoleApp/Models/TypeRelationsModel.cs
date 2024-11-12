using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PokemonTypeEffectivenessConsoleApp.Models
{
    public class TypeRelationsModel
    {
        [JsonPropertyName("double_damage_to")]
        public List<DamageTypeModel> DoubleDamageTo { get; set; } = new List<DamageTypeModel>();

        [JsonPropertyName("double_damage_from")]
        public List<DamageTypeModel> DoubleDamageFrom { get; set; } = new List<DamageTypeModel>();

        [JsonPropertyName("half_damage_to")]
        public List<DamageTypeModel> HalfDamageTo { get; set; } = new List<DamageTypeModel>();

        [JsonPropertyName("half_damage_from")]
        public List<DamageTypeModel> HalfDamageFrom { get; set; } = new List<DamageTypeModel>();

        [JsonPropertyName("no_damage_to")]
        public List<DamageTypeModel> NoDamageTo { get; set; } = new List<DamageTypeModel>();

        [JsonPropertyName("no_damage_from")]
        public List<DamageTypeModel> NoDamageFrom { get; set; } = new List<DamageTypeModel>();
    }
}
