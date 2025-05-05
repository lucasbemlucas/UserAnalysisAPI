using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAnalysisAPI.Services;
using UserAnalysisAPI.Models;
using System.Diagnostics;

namespace UserAnalysisAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var stopwatch = Stopwatch.StartNew();
            var users = _userService.GetUsers();

            stopwatch.Stop();
            return Ok(new
            {
                timestamp = DateTime.Now,
                execution_time_ms = stopwatch.ElapsedMilliseconds,
                data = users
            });
        }

        [HttpGet("superusers")]
        public IActionResult GetSuperUsers()
        {
            var stopwatch = Stopwatch.StartNew();
            var superusers = _userService.GetUsers()
            .Where(u => u.Score >= 900 && u.Ativo)
            .ToList();

            stopwatch.Stop();
            return Ok(new
            {
                timestamp = DateTime.Now,
                execution_time_ms = stopwatch.ElapsedMilliseconds,
                data = superusers
            });
        }

        [HttpGet("top-countries")]
        public IActionResult GetTopCountries()
        {
            var stopwatch = Stopwatch.StartNew();

            //1- Pega todos os superusuarios
            var superusers = _userService.GetUsers()
            .Where(u => u.Score >= 900 && u.Ativo);

            //2- Agrupa por país e conta quantos superusuarios em cada pais
            var grouped = superusers
            .GroupBy(u => u.Pais)
            .Select(group => new
            {
                country = group.Key,
                total = group.Count()
            })
            .OrderByDescending(g => g.total) //3- Ordena pelos paises com mais superusuarios
            .Take(5) //4- top 5 paises
            .ToList();

            stopwatch.Stop();

            //5- resposta formatada
            return Ok(new
            {
                timestamp = DateTime.Now,
                execution_time_ms = stopwatch.ElapsedMilliseconds,
                countries = grouped
            });
        }

        [HttpGet("team-insights")]
        public IActionResult GetTeamInsights()
        {
            var stopwatch = Stopwatch.StartNew();

            var users = _userService.GetUsers();

            var insights = users
                .GroupBy(u => u.Equipe.Nome) // Agrupa por nome da equipe
                .Select(group =>
                {
                    var totalMembers = group.Count();
                    var activeMembers = group.Count(u => u.Ativo);
                    var leaders = group.Count(u => u.Equipe.Lider);
                    var completedProjects = group
                        .SelectMany(u => u.Equipe.Projetos)
                        .Count(p => p.Concluido);

                    return new
                    {
                        team = group.Key,
                        total_members = totalMembers,
                        leaders = leaders,
                        completed_projects = completedProjects,
                        active_percentage = totalMembers == 0 ? 0 : Math.Round((double)activeMembers / totalMembers * 100, 1)
                    };
                })
                .OrderByDescending(x => x.total_members) // opcional
                .ToList();

            stopwatch.Stop();

            return Ok(new
            {
                timestamp = DateTime.Now,
                execution_time_ms = stopwatch.ElapsedMilliseconds,
                teams = insights
            });
        }

        [HttpGet("active-users-per-day")]
        public IActionResult GetActiveUsersPerDay([FromQuery] int? min = null)
        {
            var stopwatch = Stopwatch.StartNew();

            var users = _userService.GetUsers();

            //1- Acha os logs de login
            var loginLogs = users
            .SelectMany(u => u.Logs)
            .Where(log => log.Acao.ToLower() == "login");

            //2- Agrupa por data e conta quantos logs teve em cada uma
            var grouped = loginLogs
            .GroupBy(log => log.Data.Date)
            .Select(group => new
            {
                date = group.Key.ToString("yyyy-MM-dd"),
                total = group.Count()
            });

            //3- Aplica filtro de minimo
            if (min.HasValue)
            {
                grouped = grouped.Where(g => g.total >= min.Value);
            }

            //4- Organiza por data crescente
            var result = grouped
            .OrderBy(g => g.date)
            .ToList();

            stopwatch.Stop();

            //5- formata e retorna a resposta
            return Ok(new
            {
                timestamp = DateTime.Now,
                execution_time_ms = stopwatch.ElapsedMilliseconds,
                logins = result
            });


        }


    }
}
