using System;
using System.Collections.Generic;

namespace Chess_DB.Models;

public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = "";
    public int Elo { get; set; } = 1200;
}