using System;
using System.Collections.Generic;

namespace StatlerWaldorfCorp.TeamService.Models
{
    public class Team
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public ICollection<Member> Members { get; set; } = new List<Member>();

        public Team(){ }

        public Team(string name) : this()
        {
            this.Name = name;
        }

        public Team(string name, Guid id) : this(name)
        {
            this.Id = id;
        }

        public override string ToString() => this.Name;
    }
}