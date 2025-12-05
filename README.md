🚀 BuddyTech.API — Backend .NET 8 para Gestão Inteligente de Leads

BuddyTech.API é o backend oficial do ecossistema BuddyTech — uma plataforma focada em potencializar resultados de vendas através de gestão de leads, análise de interações e insights de IA.

Ele se integra diretamente a um microserviço Python (FastAPI + Gemini) que faz a análise contextual de interações e devolve scores e sugestões táticas para ajudar vendedores a priorizar oportunidades.

📌 Funcionalidades Principais

CRUD completo de Sellers, Leads, Interações e Missões

Integração com IA para gerar:

Lead Score (0–100)

Sugestões de próximos passos

Registro contextualizado de interações do lead

Agrupamento de missões por vendedor (Seller)

Estrutura modular e escalável com Services e DTOs

Migrations habilitadas (EF Core)

Documentação automática via Swagger

🧠 Integração com IA (FastAPI + Gemini)

O backend faz uma requisição ao microserviço de IA:

POST http://127.0.0.1:8000/analyze


Enviando:

histórico de interações

dados do lead

informações de funil

Recebendo:

score

notes

next_step_type

Esses dados são armazenados no PostgreSQL e exibidos no aplicativo Flutter.

🛠️ Tecnologias

.NET 8

Entity Framework Core

PostgreSQL (Supabase)

AutoMapper

Swagger / OpenAPI

FastAPI (IA externa)

Autenticação é feita pelo Supabase, mas não dentro desta API.

📁 Estrutura Completa do Projeto
BuddyTech.API/
│
├── Controllers/
│   ├── LeadController.cs
│   ├── MissionController.cs
│   └── SellerController.cs
│
├── DTOs/
│   ├── Leads/
│   │   ├── LeadAnalysisDto.cs
│   │   ├── LeadCreateRequestDto.cs
│   │   ├── LeadInteractionRequestDto.cs
│   │   ├── LeadResponseDto.cs
│   │   └── LeadUpdateRequestDto.cs
│   ├── Mission/
│   │   └── MissionResponseDto.cs
│   └── Seller/
│       ├── SellerCreateRequestDto.cs
│       ├── SellerResponseDto.cs
│       └── SellerUpdateRequestDto.cs
│
├── Enums/
│   ├── LeadStatus.cs
│   ├── MissionDescriptions.cs
│   ├── MissionTitles.cs
│   ├── Priorities.cs
│   ├── RevenueRanges.cs
│   ├── Roles.cs
│   └── TypesOfContact.cs
│
├── Infra/
│   ├── ApplicationDbContext.cs
│   └── ApplicationDbContextFactory.cs
│
├── Mappers/
│   └── SellerProfile.cs
│
├── Migrations/
│
├── Models/
│   ├── Company.cs
│   ├── Lead.cs
│   ├── LeadInteraction.cs
│   ├── LeadScore.cs
│   ├── Mission.cs
│   ├── ScoreResponseModel.cs
│   ├── Seller.cs
│   ├── SellerMission.cs
│   └── Suggestion.cs
│
└── Services/
    ├── ILeadService.cs
    ├── IMissionService.cs
    ├── IScoringService.cs
    ├── ISellerService.cs
    ├── LeadService.cs
    ├── MissionService.cs
    ├── ScoringService.cs
    └── SellerService.cs

🔗 Endpoints da API

Abaixo a tabela oficial com todos endpoints reais, baseado no seu Swagger.

🟦 Lead
Método	Rota	Descrição
POST	/api/Lead	Criar novo lead
GET	/api/Lead	Listar todos os leads
GET	/api/Lead/seller/{sellerId}	Listar leads por seller
GET	/api/Lead/{id}	Buscar lead por ID
PUT	/api/Lead/{id}	Atualizar lead
DELETE	/api/Lead/{id}	Deletar lead
POST	/api/Lead/{id}/interact	Registrar interação para o lead
🟩 Mission
Método	Rota	Descrição
GET	/api/Mission/seller/{sellerId}	Lista missões atribuídas a um vendedor
🟧 Seller
Método	Rota	Descrição
GET	/api/Seller	Listar todos os sellers
POST	/api/Seller	Criar seller
GET	/api/Seller/{id}	Buscar seller por ID
PUT	/api/Seller/{id}	Atualizar seller
DELETE	/api/Seller/{id}	Deletar seller
⚡ Serviços Internos (Domain Services)

A API utiliza services para centralizar regras:

Service	Função
LeadService	CRUD + interação + integração com scoring
SellerService	Gestão de vendedores
MissionService	Missões automáticas por vendedor
ScoringService	Comunicação com microserviço Python de IA
📚 Documentação Interativa

Swagger disponível em:

http://localhost:5099/swagger

🏗️ Como Rodar Localmente
1. Configure a conexão no appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=buddytech;User Id=postgres;Password=..."
}

2. Rodar migrations
dotnet ef database update

3. Rodar a API
dotnet run

🔮 Roadmap

 Envio automático de missões diárias por IA

 Webhooks entre API e app Flutter

 Cache de análises já realizadas

 Motor de workflows para leads

 Painel de métricas

📌 Status

✔️ Em desenvolvimento — praticamente pronto para uso
