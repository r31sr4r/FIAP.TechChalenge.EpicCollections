
# Epic Collections

Epic Collections é uma plataforma de software que permite aos usuários registrar e catalogar qualquer tipo de coleção (e.g., action figures, camisetas, instrumentos, tênis, figurinhas). O sistema também possibilita a criação de concursos de itens específicos ou de coleções completas.

[Vídeo no Youtube com a explicação do projeto](https://youtu.be/w-SL_lmIRUQ)

## Funcionalidades

- Autenticação e autorização de usuários
- Operações CRUD para coleções e itens de coleção
- Seguindo os princípios SOLID e utilizando Entity Framework para manipulação de dados

## 
Link para o documento de requisitos no Notion
[Requisitos Fase 4](https://resilient-cobalt-7ff.notion.site/Fase-4-Tech-Challenge-38907b4e33ad4ceba3eb8ad2e95f37c0)


## Começando

### Pré-requisitos
- .NET 6 SDK
- Docker

### Executando o Projeto
1. Iniciar o Banco de Dados
O projeto utiliza um banco de dados que precisa ser iniciado usando Docker. Navegue até a raiz do projeto e execute o seguinte comando:

```bash
 docker-compose up
```
2. Aplicar Migrações
Após iniciar o banco de dados, aplique as migrações do Entity Framework para configurar o esquema do banco de dados. Execute o seguinte comando:

```bash
dotnet ef database update --project .\src\FIAP.TechChalenge.EpicCollections.Api\
```

3. Executar a Aplicação

Agora você pode executar a aplicação. Use o seguinte comando:
```bash
dotnet run --project .\src\FIAP.TechChalenge.EpicCollections.Api\
```


## Rodando os testes

O projeto possui teste de Unidade, Integração e E2E. Para rodar  os testes E2E, certifique-se de que os serviços necessários estão em execução. Navegue até o diretório Net.SimpleBlog/tests/Net.SimpleBlog.E2ETests e execute o seguinte comando:

```bash
docker-compose up
```

Depois, execute os testes 

```bash
dotnet test
```

## Uso

### Registrar um Usuário

Para começar a usar a aplicação, você precisa registrar um usuário. Use o endpoint /Users para criar um novo usuário.

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

### Autenticação

Autentique-se usando o endpoint /users/authenticate para obter um token JWT.

Exemplo de payload:

```javascript
{
  "email": "john.doe@example.com",
  "password": "SecurePassword123!"
}

```

A resposta conterá um token que você precisa incluir no cabeçalho Authorization para as requisições subsequentes.

### Operações CRUD para Coleções e Itens

Uma vez autenticado, você pode usar os seguintes endpoints para gerenciar postagens:

- Criar Postagem: POST /posts
Exemplo de payload:

```javascript
{
  "name": "Minha Primeira Coleção",
  "description": "Esta é a descrição da minha primeira coleção.",
  "category": 1
}
```

- Obter Postagem por ID: GET /posts/{id}


- Atualizar Coleção: PUT /collections/{id}
Exemplo de payload:

```javascript
{
  "name": "Nome da Coleção Atualizada",
  "description": "Descrição atualizada.",
  "category": 1
}
```

Criar Item de Coleção: POST /collections/{collectionId}/items

Exemplo de payload:

```javascript
{
  "name": "Nome do Item",
  "description": "Descrição do Item",
  "acquisitionDate": "2024-01-01",
  "value": 100.0,
  "photoUrl": "http://example.com/photo.jpg"
}
```

Obter Item de Coleção por ID: GET /collections/{collectionId}/items/{itemId}

Atualizar Item de Coleção: PUT /collections/{collectionId}/items/{itemId}

Exemplo de payload:

```javascript
{
  "name": "Nome do Item Atualizado",
  "description": "Descrição do Item Atualizada",
  "acquisitionDate": "2024-01-02",
  "value": 150.0,
  "photoUrl": "http://example.com/updated_photo.jpg"
}
```

Excluir Item de Coleção: DELETE /collections/{collectionId}/items/{itemId}

- Visualização de Coleções
Usuários não autenticados podem visualizar coleções  usando o endpoint GET /collections.


## Conclusão
Este projeto demonstra uma plataforma abrangente para gerenciar coleções e itens de coleção com funcionalidades como autenticação de usuários e operações CRUD. Siga as instruções acima para configurar, executar e testar a aplicação. Aproveite o uso do Epic Collections!