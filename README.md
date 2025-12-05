# 🔧 BuddyTechAPI — Backend .NET 8 para Gestão Inteligente de Leads

BuddyTechAPI é o backend oficial do ecossistema BuddyTech — uma plataforma focada em potencializar resultados de vendas através de gestão de leads, análise de interações e insights de IA.

Ele se integra diretamente a um microsserviço Python (FastAPI + Gemini) que faz a análise contextual de interações e devolve **scores e sugestões táticas** para ajudar vendedores a priorizar oportunidades.

---

## ⭐ Funcionalidades Principais

* CRUD completo de Sellers, Leads, Interações e Missões
* Integração com IA para gerar:

  * Lead Score (0–100)
  * Sugestões de próximos passos
  * Registro contextualizado de interações do lead
  * Agrupamento de missões por vendedor (Seller)
* Estrutura modular e escalável com Services e DTOs
* Migrations habilitadas (EF Core)
* Documentação automática via Swagger

### 🤖 Integração com IA (FastAPI + Gemini)

O backend faz uma requisição ao microsserviço de IA:

**POST [http://127.0.0.1:8000/analyze](http://127.0.0.1:8000/analyze)**

**Enviando:**

* histórico do lead
* dados do lead
* informações de funil
* interações
* recomendações

**Recebendo:**

* score
* notes
* next_step
* next_step_type

Esses dados são armazenados em PostgreSQL e exibidos no app Flutter.

---

## 🛠 Tecnologias

* **.NET 8**
* **Entity Framework Core**
* **PostgreSQL (Supabase)**
* **AutoMapper**
* **Swagger / OpenAPI**
* **FastAPI (IA externa)**

Autenticação é feita pelo Supabase, mas **não dentro desta API**.

---

## 🧩 Estrutura Completa do Projeto

```
BuddyTechAPI
 ├── Controllers/
 │    ├── LeadControllers.cs
 │    ├── MissionControllers.cs
 │    └── SellerControllers.cs
 ├── DTOs/
 │    ├── LeadDTOs.cs
 │    ├── LeadAnalysisDTOs.cs
 │    ├── LeadCreateRequestDTO.cs
 │    └── ...
 ├── Interactions/
 ├── Lead/
 │    ├── LeadResponseDTO.cs
 │    ├── LeadUpdateRequestDTO.cs
 │    ├── Mission/
 │    ├── MissionResponseDTO.cs
 │    ├── Seller/
 │    ├── SellerCreateRequestDTO.cs
 │    └── SellerResponseDTO.cs
 ├── Services/
 │    ├── ILeadService.cs
 │    ├── ISellerService.cs
 │    ├── IMissionService.cs
 │    ├── IScoringService.cs
 │    ├── LeadService.cs
 │    ├── SellerService.cs
 │    └── MissionService.cs
 ├── Migrations/
 ├── Models/
 │    ├── Company.cs
 │    ├── Leads.cs
 │    ├── Interactions.cs
 │    ├── LeadScores.cs
 │    ├── Mission.cs
 │    ├── Priorities.cs
 │    ├── Suggestions.cs
 │    └── ...
 └── appsettings.json
```

---

## 🚀 Endpoints da API (baseados no seu Swagger)

### **📌 Lead**

* POST `/api/Lead` – Criar novo lead
* GET `/api/Lead` – Listar todos
* GET `/api/Lead/seller/{sellerId}` – Leads por vendedor
* GET `/api/Lead/{id}` – Buscar por ID
* PUT `/api/Lead/{id}` – Atualizar
* DELETE `/api/Lead/{id}` – Deletar
* POST `/api/Lead/{id}/interact` – Criar interação

### **🧑‍💼 Seller**

* GET `/api/Seller` – Listar
* POST `/api/Seller` – Criar
* GET `/api/Seller/{id}` – Buscar por ID
* PUT `/api/Seller/{id}` – Atualizar
* DELETE `/api/Seller/{id}` – Deletar

### **🎯 Mission**

* GET `/api/Mission/seller/{sellerId}` – Listar missões por vendedor

---
