# HMIS Microservices (scaffold)

This repository contains a scaffold for a Hospital Management Information System (HMIS) using .NET (net8.0) microservice architecture. It includes the following services:

- IdentityService (JWT auth, Dapper + Postgres)
- PatientService (CRUD, Dapper + Postgres)
- ClinicalService (skeleton)
- BillingService (skeleton)
- AbdmService (verification adapter) and abdm-mock for development

Included:
- Dockerfiles for services
- docker-compose.yml to run Postgres, RabbitMQ and abdm-mock
- SQL migration scripts (simple CREATE TABLE IF NOT EXISTS)

How to run (quick):
1. Install Docker & Docker Compose
2. From repo root: docker-compose up --build -d
3. Apply migrations (or services will create tables on startup if configured)
4. Use the IdentityService to register and request a token, then call protected endpoints.

This is a scaffold—fill in business rules, validations, and production hardening before using in production.
