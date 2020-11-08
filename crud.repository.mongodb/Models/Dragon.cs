using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crud.repository.mongodb.Models
{
    public class Dragon
    {
        public ObjectId Id { get; private set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public int Gold { get; set; }
        public int MaxHP { get; set; }
        public Breath Weapon { get; set; }
        public  DateTime DateBorn { get; set; }
        public DateTime? DateDied { get; set; }

        public Dragon()
        {
            DateBorn = DateTime.Now;
        }
    }

    public class Breath
    {
        public enum BreathType
        {
            Fire, 
            Ice,
            Lightning,
            PoisonGas,
            Darkness,
            Light
        };

        public string Name { get; set; }
        public string Descrtiption { get; set; }
        public BreathType Type { get; set; }
    }
}