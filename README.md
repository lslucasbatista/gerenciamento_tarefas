# Desafio Técnico .NET (C#) - Gerenciamento de Tarefas

## Instruções de Gerais

A API ao inicializar pela primeira vez cria os usuários, status, prioridades e cargos

## Prioridade
{ Descricao = "Baixa", Id = 1 }
{ Descricao = "Média", Id = 2 }
{ Descricao = "Alta",  Id = 3 }

## Status
{ Descricao = "Pendente", Id = 1 }
{ Descricao = "Em Andamento", Id = 2 }
{ Descricao = "Concluída", Id = 3 }

## Cargo
{ Descricao = "Gerente", Id = 1 }
{ Descricao = "Colaborador", Id = 2 }

## Usuário
{ Descricao = "Usuário Gerente", Email = "gerente@gmail.com", Id = 1  }
{ Descricao = "Usuário Colaborador", Email = "gerente@gmail.com", Id = 2 }

## Instruções de Execução

### Subir o ambiente
docker compose up -d --build

### Remover os containers mantendo o banco
docker compose down

### Remover os containers limpando o banco
docker compose down -v

### Acesso a API
http://localhost:5000/swagger


## Fase 2: Refinamento

Existe planos para autenticação?  
Seria interessante exportar os relatorios em varios formatos? exemplo, excel, pdf, csv?  
Haverá controle de usuários e perfis?   
Haverá notificações para os usuários quando ocorrer alguma alteração em suas tarefas?  
Caso a tarefa atinja a data de vencimento, o que deve acontecer?  
Tem interesse em incluir anexos nas tarefas?


## Fase 2: Final

Implementação de testes de integração automatizados  
Implementação de logs  
Migração para microserviços com o crescimento da aplicação, exemplo a implementação de notificações,   Geração de relatorio..  
Implementar a comunicação entre serviços via mensageria (RabbitMQ, Kafka)  
Migrar para containerização individual para cada serviço com a migração para microsserviços e orquestração via Kubernetes.  
Realizar também a separação dos repositórios por serviço para permitir deploys independes com suas próprias pipelines  

