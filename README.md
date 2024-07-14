
# Epic Collections

Epic Collections � uma plataforma de software que permite aos usu�rios registrar e catalogar qualquer tipo de cole��o (e.g., action figures, camisetas, instrumentos, t�nis, figurinhas). O sistema tamb�m possibilita a cria��o de concursos de itens espec�ficos ou de cole��es completas.

[V�deo no Youtube com a explica��o do projeto](https://youtu.be/w-SL_lmIRUQ)

## Funcionalidades

- Autentica��o e autoriza��o de usu�rios
- Opera��es CRUD para cole��es e itens de cole��o
- Seguindo os princ�pios SOLID e utilizando Entity Framework para manipula��o de dados

## 
Link para o documento de requisitos no Notion
[Requisitos Fase 4](https://resilient-cobalt-7ff.notion.site/Fase-4-Tech-Challenge-38907b4e33ad4ceba3eb8ad2e95f37c0)


## Come�ando

### Pr�-requisitos
- .NET 6 SDK
- Docker

### Executando o Projeto
1. Iniciar o Banco de Dados
O projeto utiliza um banco de dados que precisa ser iniciado usando Docker. Navegue at� a raiz do projeto e execute o seguinte comando:

```bash
 docker-compose up
```
2. Aplicar Migra��es
Ap�s iniciar o banco de dados, aplique as migra��es do Entity Framework para configurar o esquema do banco de dados. Execute o seguinte comando:

```bash
dotnet ef database update --project .\src\FIAP.TechChalenge.EpicCollections.Api\
```

3. Executar a Aplica��o

Agora voc� pode executar a aplica��o. Use o seguinte comando:
```bash
dotnet run --project .\src\FIAP.TechChalenge.EpicCollections.Api\
```


## Rodando os testes

O projeto possui teste de Unidade, Integra��o e E2E. Para rodar  os testes E2E, certifique-se de que os servi�os necess�rios est�o em execu��o. Navegue at� o diret�rio Net.SimpleBlog/tests/Net.SimpleBlog.E2ETests e execute o seguinte comando:

```bash
docker-compose up
```

Depois, execute os testes 

```bash
dotnet test
```

## Uso

### Registrar um Usu�rio

Para come�ar a usar a aplica��o, voc� precisa registrar um usu�rio. Use o endpoint /Users para criar um novo usu�rio.

Exemplo de payload:

```javascript
{
  "name": "John Doe",
  "email": "john.doe@example.com",
  "password": "SecurePassword123!",
  "phone": "(12) 98765-7890",
  "cpf": "123.456.789-00",
  "rg": "MG1234567",
  "dateOfBirth": "1990-01-01",
  "is_active": true
}

```

### Autentica��o

Autentique-se usando o endpoint /users/authenticate para obter um token JWT.

Exemplo de payload:

```javascript
{
  "email": "john.doe@example.com",
  "password": "SecurePassword123!"
}

```

A resposta conter� um token que voc� precisa incluir no cabe�alho Authorization para as requisi��es subsequentes.

### Opera��es CRUD para Cole��es e Itens

Uma vez autenticado, voc� pode usar os seguintes endpoints para gerenciar postagens:

- Criar Postagem: POST /posts
Exemplo de payload:

```javascript
{
  "name": "Minha Primeira Cole��o",
  "description": "Esta � a descri��o da minha primeira cole��o.",
  "category": 1
}
```

- Obter Postagem por ID: GET /posts/{id}


- Atualizar Cole��o: PUT /collections/{id}
Exemplo de payload:

```javascript
{
  "name": "Nome da Cole��o Atualizada",
  "description": "Descri��o atualizada.",
  "category": 1
}
```

Criar Item de Cole��o: POST /collections/{collectionId}/items

Exemplo de payload:

```javascript
{
  "name": "Nome do Item",
  "description": "Descri��o do Item",
  "acquisitionDate": "2024-01-01",
  "value": 100.0,
  "photoUrl": "http://example.com/photo.jpg"
}
```

Obter Item de Cole��o por ID: GET /collections/{collectionId}/items/{itemId}

Atualizar Item de Cole��o: PUT /collections/{collectionId}/items/{itemId}

Exemplo de payload:

```javascript
{
  "name": "Nome do Item Atualizado",
  "description": "Descri��o do Item Atualizada",
  "acquisitionDate": "2024-01-02",
  "value": 150.0,
  "photoUrl": "http://example.com/updated_photo.jpg"
}
```

Excluir Item de Cole��o: DELETE /collections/{collectionId}/items/{itemId}

- Visualiza��o de Cole��es
Usu�rios n�o autenticados podem visualizar cole��es  usando o endpoint GET /collections.


## Conclus�o
Este projeto demonstra uma plataforma abrangente para gerenciar cole��es e itens de cole��o com funcionalidades como autentica��o de usu�rios e opera��es CRUD. Siga as instru��es acima para configurar, executar e testar a aplica��o. Aproveite o uso do Epic Collections!