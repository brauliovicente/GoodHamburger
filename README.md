# 🍔 **GoodHamburger**

Sistema de pedidos com regras de negócio e descontos.

### **Backend**

O backend foi desenvolvido utilizando .NET e segue a arquitectura Clean Architecture com a implementação do padrão CQRS e MediatR para comunicação entre componentes.

1. Acesse o directório do backend:
   cd backend/GoodHamburger.Api
2. Rode a aplicação:
   dotnet run
   O sistema estará disponível na URL padrão do .NET, geralmente `http://localhost:5000`.

### **Frontend**

O frontend foi desenvolvido utilizando Blazor, para fornecer uma interface de utilizador interactiva com o backend.

1. Acesse o directório do frontend:

   cd frontend/GoodHamburger.Web

2. Rode a aplicação:

   dotnet run

   O frontend será iniciado no navegador, normalmente acessível em `http://localhost:5001`.

### **Testes**

Abaixo, estão os testes unitários da camada de domínio, que verificam a lógica do sistema e as regras de negócios.

1. Acesse o diretório dos testes:

   cd backend/GoodHamburger.Dominio.Tests

2. Rode os testes:

   dotnet test


---

## **Arquitectura**

O **GoodHamburger** segue a **Clean Architecture**, com o uso do padrão **CQRS** (Command Query Responsibility Segregation) e **MediatR** para orquestrar as comunicações entre os diferentes componentes do sistema.

### **Componentes principais:**

* **Dominio**: Contém a lógica de negócios do sistema, incluindo regras e entidades principais (como `Pedido`, `Item`).
* **Aplicação**: Define os casos de uso e coordena as operações de leitura e escrita, com validações e execução de comandos.
* **Infraestrutura**: Implementa os detalhes técnicos, como o repositórios, etc.
* **API**: Expondo as interfaces de comunicação RESTful para o frontend e consumidores externos.

O sistema foi desenvolvido para ser **escável**, **manutenível** e **flexível**, utilizando práticas de boas arquitecturas de software.



## **Tecnologias utilizadas**

* **Backend**: .NET 8, Clean Architecture, CQRS, MediatR
* **Frontend**: Blazor, C#
* **Armazenamento**: Armazenamento na memória cache
* **Testes**: xUnit, Moq, Entity Framework InMemory para testes de repositórios