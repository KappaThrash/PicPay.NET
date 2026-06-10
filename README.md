# PicPay.NET
PicPay desafio em .NET

Repo original do desafio: https://github.com/PicPay/picpay-desafio-backend<div align="center"> [![C#](https://custom-icon-badges.demolab.com/badge/C%23-%23239120.svg?logo=cshrp&logoColor=white)](#) [![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff)](#)	[![Microsoft SQL Server](https://custom-icon-badges.demolab.com/badge/Microsoft%20SQL%20Server-CC2927?logo=mssqlserver-white&logoColor=white)](#) [![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=fff)](#) [![AWS](https://custom-icon-badges.demolab.com/badge/AWS-%23FF9900.svg?logo=aws&logoColor=white)](#)
[![Terraform](https://img.shields.io/badge/Terraform-844FBA?logo=terraform&logoColor=fff)](#) [![Bash](https://img.shields.io/badge/Bash-4EAA25?logo=gnubash&logoColor=fff)](#)
![RabbitMQ](https://img.shields.io/badge/Rabbitmq-FF6600?logo=rabbitmq&logoColor=white)	[![GitHub Actions](https://img.shields.io/badge/GitHub_Actions-2088FF?logo=github-actions&logoColor=white)](#) </div>

## DOCKER

Subir o containers:
```bash
#SQL Server e a Network
docker compose up -d
```

## ENDPOINTS

## Usuario

### `POST /usuario` - Criar conta Usuario
### `GET /usuario/{id}` - Retornar conta de Usario existente

### Validação
- CPF ou CNPJ devem ser validos, serão recusados caso não sejam
- Email formatado certamente
- CPF/CNPJ e Email são unicos no sistema

#### Exemplo:
```json
{
  "nome": "João Lima",
  "tipo": "USUARIO",
  "cpf": "546.471.429-49",
  "email": "teste@exemplo.com",
  "senha": "senha123"
}
```

### `POST /lojista` - Criar conta Lojista

#### Exemplo:
```json
{
  "nome": "João Lima",
  "tipo": "LOJISTA",
  "cpnpj": "66.838.061/0001-30",
  "email": "teste@exemplo.com",
  "senha": "senha123"
}
```

## Carteira

### `POST /carteira{user_id}` - Criar Carteira apartir de um Usuario existente

#### Exemplo:

/carteira/550e8400-e29b-41d4-a716-446655440000

Retorno:
```json
{
  "id": "f81d4fae-7dec-11d0-a765-00a0c91e6bf6",
  "user_id": "550e8400-e29b-41d4-a716-446655440000",
  "balance": "100"
}
```

### `GET /carteira/{UUID}` - Retorna Carteira existente
### `GET /carteira/user/{User_Id}` - Retorna Carteira existente
### `DELETE /carteira/{User_Id}` - Deleta carteira e retorna a deletada

#### Exemplo:
```json
{
  "id": "f81d4fae-7dec-11d0-a765-00a0c91e6bf6",
  "user_id": "550e8400-e29b-41d4-a716-446655440000",
  "balance": "100"
}
```

## Transação

### `POST /transacao` - Efetuar transação apartir de duas carteiras existentes (Rollback em caso de falhas)
### `GET /transacao{id}` - Retorna transações apartir do ID salvo
### `GET /transacao/payer/{id}` - Retorna transações de todos Payers com tal ID
### `GET /transacao/payee/{id}` - Retorna transações de todos Payees com tal ID
### `GET /transacao/any/{id}` - Retorna transações de qualquer Payee ou Payer com tal ID

### Regras de negocio:
- Carteira payer deve pertencer a Usuario do tipo USUARIO, LOJISTAS não podem fazer transferencias, apenas receber
- Saldo deve ser suficiente

#### Exemplo:
```json
{
  "amount": "150",
  "payer": "f81d4fae-7dec-11d0-a765-00a0c91e6bf6",
  "payee": "7d0c10e7-972a-4774-8b51-7a0335de610a"
}
```
Retorno:
```json
{
  "id": "e02e3bf3-f748-434b-a72c-86034e7b8b7a",
  "amount": "150",
  "payer": "f81d4fae-7dec-11d0-a765-00a0c91e6bf6",
  "payee": "7d0c10e7-972a-4774-8b51-7a0335de610a"
}
