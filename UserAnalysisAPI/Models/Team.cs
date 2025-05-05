using System;
using Microsoft.AspNetCore.SignalR;

namespace UserAnalysisAPI.Models;

public class Team
{
    public string Nome { get; set; } = string.Empty;
    public bool Lider { get; set; }
    public List<Project> Projetos { get; set; } = new List<Project>();
}
