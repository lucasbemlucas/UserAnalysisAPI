# UserAnalysisAPI

Uma API REST construída com ASP.NET Core para análise de dados de usuários a partir de um arquivo JSON contendo milhares de registros.

---

## 📦 Descrição do Projeto

Este projeto faz parte de um desafio técnico que propõe:

- O carregamento de um arquivo JSON com 100.000 usuários (usando 1.000 no ambiente de testes)
- Processamento dos dados na memória
- Exposição de endpoints performáticos para análise
- Um endpoint de autoavaliação da própria API

---

## 🚀 Tecnologias Utilizadas

- ASP.NET Core 8
- C#
- System.Text.Json
- Swagger / Swashbuckle
- LINQ
- Injeção de Dependência (DI)
- HttpClientFactory

---

## 📁 Estrutura

```
UserAnalysisAPI/
├── Controllers/
├── Models/
├── Services/
├── Data/
└── Program.cs
```

---

## 🧠 Endpoints disponíveis

| Método | Rota                            | Descrição                                              |
| ------ | ------------------------------- | ------------------------------------------------------ |
| GET    | /api/users                      | Retorna todos os usuários                              |
| GET    | /api/users/superusers           | Retorna usuários com score >= 900 e ativo = true       |
| GET    | /api/users/top-countries        | Retorna os 5 países com mais superusuários             |
| GET    | /api/users/team-insights        | Estatísticas por equipe: membros, líderes, projetos    |
| GET    | /api/users/active-users-per-day | Retorna logins por data, com filtro opcional ?min=3000 |
| GET    | /api/evaluation                 | Executa testes internos em todos os endpoints          |

---

## 🧪 Teste automático da API

O endpoint `/api/evaluation` testa os principais endpoints da aplicação:

- Mede tempo de resposta (ms)
- Valida status HTTP
- Verifica se o retorno é JSON válido

---

## ⚙️ Como executar localmente

```bash
# Clone o repositório
git clone https://github.com/lbalmendra/UserAnalysisAPI.git
cd UserAnalysisAPI

# Execute a aplicação
dotnet run

# Acesse o Swagger:
http://localhost:5000/swagger
```

> 🔥 Certifique-se de que o arquivo `usuarios_1000.json` esteja em `./Data/` e com os campos corretos.

---

## 📂 Exemplo de JSON

```json
{
  "id": "abc123",
  "nome": "Lucas Bellucci",
  "score": 950,
  "ativo": true,
  "pais": "Brasil",
  "equipe": {
    "nome": "Frontend Avengers",
    "lider": true,
    "projetos": [{ "nome": "API", "concluido": true }]
  },
  "logs": [{ "data": "2025-03-25T00:00:00", "acao": "login" }]
}
```

---

## 🛠️ Autor

**Lucas Bellucci Almendra**  
🔗 GitHub: [@lbalmendra](https://github.com/lbalmendra)

---

## 📄 Licença

Este projeto foi criado para fins de estudo e demonstração de habilidades com .NET. Livre para forks e contribuições.
