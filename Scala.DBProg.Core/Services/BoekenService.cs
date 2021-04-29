using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Scala.DBProg.Core.Entities;

namespace Scala.DBProg.Core.Services
{
    public class BoekenService
    {
        // ==============
        // CRUD AUTEURS
        // ==============
        public List<Auteur> GetAuteurs()
        {
            List<Auteur> auteurs = new List<Auteur>();
            string sql = "select id, naam from auteurs order by naam";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            if (dataTable == null)
            {
                return null;
            }
            else
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    string id = dr["id"].ToString();
                    string naam = dr["naam"].ToString();
                    Auteur auteur = new Auteur(id, naam);
                    auteurs.Add(auteur);
                }
                return auteurs;
            }
        }
        public bool AuteurToevoegen(Auteur auteur)
        {
            string sql = $"insert into auteurs (id, naam) values ('{auteur.Id}', '{Helper.HandleQuotes(auteur.Naam)}')";
            return DBService.ExecuteCommand(sql);
        }
        public bool AuteurWijzigen(Auteur auteur)
        {
            string sql = $"update auteurs set naam = '{Helper.HandleQuotes(auteur.Naam)}' where id = '{auteur.Id}'";
            return DBService.ExecuteCommand(sql);
        }
        public bool AuteurVerwijderen(Auteur auteur)
        {
            if (IsAuteurInGebruik(auteur))
                return false;
            string sql = $"delete from auteurs where id = '{auteur.Id}'";
            return DBService.ExecuteCommand(sql);
        }
        public bool IsAuteurInGebruik(Auteur auteur)
        {
            string sql = $"select count(*) from boekeb-n where auteurId = '{auteur.Id}'";
            string count = DBService.ExecuteScalar(sql);
            if (count == null)
                return false;
            if (int.Parse(count) == 0)
                return false;
            else
                return true;
        }
        public bool BestaatAuteurId(string auteurId)
        {
            string sql = $"select count(*) from auteurs where id = '{auteurId}'";
            string count = DBService.ExecuteScalar(sql);
            if (count == null)
                return false;
            if (int.Parse(count) == 0)
                return false;
            else
                return true;
        }
        public Auteur ZoekAuteurViaNaam(string naam)
        {
            string sql = $"select * from auteurs where naam = '{Helper.HandleQuotes(naam)}'";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            if (dataTable == null)
            {
                return null;
            }
            else
            {
                if (dataTable.Rows.Count > 0)
                {
                    return new Auteur(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["naam"].ToString());
                }
                else
                {
                    return null;
                }
            }
        }
        public Auteur ZoekAuteurViaId(string auteurId)
        {
            string sql = $"select * from auteurs where id = '{auteurId}'";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            if (dataTable == null)
            {
                return null;
            }
            else
            {
                if (dataTable.Rows.Count > 0)
                {
                    return new Auteur(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["naam"].ToString());
                }
                else
                {
                    return null;
                }
            }
        }

        // ==============
        // CRUD UITGEVERS
        // ==============
        public List<Uitgever> GetUitgevers()
        {
            List<Uitgever> uitgevers = new List<Uitgever>();
            string sql = $"select id, naam from uitgevers order by naam";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            if (dataTable == null)
            {
                return null;
            }
            else
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    uitgevers.Add(new Uitgever(dr["id"].ToString(), dr["naam"].ToString()));
                }
                return uitgevers;
            }
        }
        public bool UitgeverToevoegen(Uitgever uitgever)
        {
            string sql = $"insert into uitgevers (id, naam) values ('{uitgever.Id}', '{Helper.HandleQuotes(uitgever.Naam)}')";
            return DBService.ExecuteCommand(sql);
        }
        public bool UitgeverWijzigen(Uitgever uitgever)
        {
            string sql = $"update uitgevers set naam = '{Helper.HandleQuotes(uitgever.Naam)}' where id = '{uitgever.Id}'";
            return DBService.ExecuteCommand(sql);
        }
        public bool UitgeverVerwijderen(Uitgever uitgever)
        {
            if (IsUitgeverInGebruik(uitgever))
                return false;
            string sql = $"delete from uitgevers where id = '{uitgever.Id}'";
            return DBService.ExecuteCommand(sql);
        }
        public bool IsUitgeverInGebruik(Uitgever uitgever)
        {
            string sql = $"select count(*) from boeken where uitgeverId = '{uitgever.Id}'";
            string count = DBService.ExecuteScalar(sql);
            if (count == null)
                return false;
            if (int.Parse(count) == 0)
                return false;
            else
                return true;
        }
        public bool BestaatUitgeverId(string uitgeverId)
        {
            string sql = $"select count(*) from uitgevers where id = '{uitgeverId}'";
            string count = DBService.ExecuteScalar(sql);
            if (count == null)
                return false;
            if (int.Parse(count) == 0)
                return false;
            else
                return true;
        }
        public Uitgever ZoekUitgeverViaNaam(string naam)
        {
            string sql = $"select * from uitgevers where naam = '{Helper.HandleQuotes(naam)}'";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            if (dataTable == null)
            {
                return null;
            }
            else
            {
                if (dataTable.Rows.Count > 0)
                {
                    return new Uitgever(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["naam"].ToString());
                }
                else
                {
                    return null;
                }
            }
        }
        public Uitgever ZoekUitgeverViaId(string uitgeverId)
        {
            string sql = $"select * from uitgevers where id = '{uitgeverId}'";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            if (dataTable == null)
            {
                return null;
            }
            else
            {
                if (dataTable.Rows.Count > 0)
                {
                    return new Uitgever(dataTable.Rows[0]["id"].ToString(), dataTable.Rows[0]["naam"].ToString());
                }
                else
                {
                    return null;
                }
            }
        }

        // ===========
        // CRUD BOEKEN
        // ===========
        public List<Boek> GetBoeken(Auteur auteur = null, Uitgever uitgever = null)
        {
            List<Boek> boeken = new List<Boek>();

            string filter = "";
            if (auteur != null)
                filter = "where auteurId = '" + auteur.Id + "'";
            if (uitgever != null)
            {
                if (filter != "")
                    filter += " and ";
                else
                    filter += " where ";
                filter = " uitgeverId = '" + uitgever.Id + "'";
            }
            string sql = "select * from boeken " + filter + " order by titel";
            DataTable dataTable = DBService.ExecuteSelect(sql);
            if (dataTable == null)
            {
                return null;
            }
            else
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    boeken.Add(new Boek(dr["id"].ToString(), dr["titel"].ToString(), dr["auteurId"].ToString(), dr["uitgeverId"].ToString(), int.Parse(dr["jaar"].ToString())));
                }
                return boeken;
            }
        }
        public bool BoekToevoegen(Boek boek)
        {
            if (!BestaatAuteurId(boek.AuteurId))
                return false;
            if (!BestaatUitgeverId(boek.UitgeverId))
                return false;

            string sql = $"insert into boeken (id, titel, auteurId, uitgeverId, year) values ('{boek.Id}', '{Helper.HandleQuotes(boek.Titel)}' ,'{boek.AuteurId}','{boek.UitgeverId}', {boek.Jaar})";
            return DBService.ExecuteCommand(sql);
        }
        public bool BoekWijzigen(Boek boek)
        {
            string sql = $"update boeken set titel = '{Helper.HandleQuotes(boek.Titel)}', auteurId = '{boek.AuteurId}', uitgeverId = '{boek.UitgeverId}', jaar = {boek.Jaar} where id = '{boek.Id}'";
            return DBService.ExecuteCommand(sql);
        }
        public bool BoekVerwijderen(Boek boek)
        {
            string sql = $"delete from boeken where id = '{boek.Id}'";
            return DBService.ExecuteCommand(sql);
        }

    }
}
