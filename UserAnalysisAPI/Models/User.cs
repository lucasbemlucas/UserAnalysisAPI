using UserAnalysisAPI.Models;

namespace UserAnalysisAPI.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
    public int Score { get; set; }
    public bool Ativo { get; set; }
    public string Pais { get; set; } = string.Empty;
    public Team Equipe { get; set; } = new Team();
    public List<Log> Logs { get; set; } = new List<Log>();
}
