using System;
using System.Collections.Generic;
using System.Text;

namespace Scala.DBProg.Core.Entities
{
    public class Auteur
    {
        public string Id { get; private set; }
        public string Naam { get; set; }
        public Auteur()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Auteur(string naam)
        {
            Id = Guid.NewGuid().ToString(); 
            Naam = naam;
        }
        public Auteur(string id, string naam)
        {
            Id = id;
            Naam = naam;
        }
        public override string ToString()
        {
            return $"{Naam}";
        }
    }
}
