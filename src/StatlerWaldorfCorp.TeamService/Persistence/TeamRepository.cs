using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using StatlerWaldorfCorp.TeamService.Models;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace StatlerWaldorfCorp.TeamService.Persistence
{
    public class TeamRepository : ITeamRepository
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Team> _collection;
        private readonly ILogger<TeamRepository> _logger;

        public TeamRepository(IOptions<MongoDbSettings> settings, ILogger<TeamRepository> logger)
        {

            _logger = logger;
            _client = new MongoClient(settings.Value.ConnectionString);
            _database = _client.GetDatabase(settings.Value.Database);
            _collection = _database.GetCollection<Team>("Teams");
        }

        public void AddTeam(Team team)
        {
            _collection.InsertOne(team);
        }

        public Team DeleteTeam(Guid id)
        {
            var teamToDelete = _collection.AsQueryable().FirstOrDefault(t => t.Id.Equals(id));
            if(teamToDelete != null)
            {
                _collection.DeleteOne(f => f.Id.Equals(id));
            }
            return teamToDelete;
        }

        public Team GetTeam(Guid id) => _collection.AsQueryable().FirstOrDefault(t => t.Id.Equals(id));

        public IEnumerable<Team> GetTeams() => _collection.AsQueryable();

        public void UpdateTeam(Team team)
        {
            var entity = _collection.AsQueryable().FirstOrDefault(t => t.Id.Equals(team.Id));
            if (entity == null)
            {
                return;                
            }
            _collection.ReplaceOne(f => f.Id.Equals(team.Id), team);
        }
    }
}