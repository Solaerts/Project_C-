using System;
using System.Collections.Generic;
namespace Chess_DB.Models;

public class Competition
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public List<Guid> PlayerIds { get; set; } = new();
}