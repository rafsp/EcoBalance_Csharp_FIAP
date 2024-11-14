# EcoBalance

API do projeto EcoBalance - Software que monitora a energia solar de pequenas empresas

# Representantes 

- Gabriel Ortiz Oliva Gil - RM: 98642 – 2TDSPK
- Rafael Noboru Watanabe Nasaha - RM:99948 – 2TDSPK
- João Pedro Kraide Máximo - RM:550974 – 2TDSPK
- Matheus de Andrade Ferreira - RM:99375 – 2TDSPK
- Larissa Pereira Biusse - RM:551062 - 2TDSPK

# Objetivos da aplicação

- API que monitora a produção e o consumo de energia solar de pequenas empresas manualmente ou através de um dispositivo ESP32 e utilizando a ferramenta Thinger IO
- A aplicação também faz previsão de qual consumo previsto baseado na temperatura e utilizando dados em CSV
- O Objetivo principal é a pequena empresa ter os dados de uso e poder gerenciar melhor o uso de sua energia

# Explicação do modelo da aplicação

Foi adotado a abordagem monolítica para a aplicação pelos seguintes motivos:

- Por se tratar de um projeto menor e sem muitas complexidades, uma abordagem monolítica permite um desenvolvimento mais rápido
- Simplicidade na arquitetura, manter tudo em um único projeto facilita a compreensão e manutenção inicial por estar tudo em um único código base
- A integração entre componentes é mais direta e isso facilita a implementação de testes
- A latência é reduzida como também a sobrecarga associada à comunicação entre serviços distribuídos 
- Mais fácil de gerenciar transições que envolvem múltiplos componentes 

# Recursos disponíveis na aplicação

- CRUD com banco de dados Oracle 
- Login com autenticação e criptografia de senhas
- Implementação do ML.NET para análise dos dados e previsão
- Possibilidade de verificar a saúde da aplicação através do HealthCheck
- Clean Code aplicado separando devidamente as bibliotecas de classes e implementando extensões 
- Serviço de API externa (Thinger IO)

# Como rodar a aplicação

- Clonar o projeto do repositório no GitHub
- Lembrar de alterar as informações do banco de dados no "appsettings.json" de acordo com seus dados
- Executar o projeto no VS Code ou outra IDE para linguagem C# e projeto API Web Core
- Caso queira verificar a saúde da aplicação, acesse com https://localhost:{port}/health
- Para execução do Machine Learning, dentro da IDE, abra dentro da Biblioteca de Classes do MLModel o arquivo "MlModel.mbconfig" e abrirá a interface do ModelBuilder e na parte "Avaliar" é possível experimentar o modelo treinado
- Ao executar, uma página web do Swagger irá abrir
- No Swagger, teremos o CRUD de todas as classes
- Necessário antes de utilizar os endpoints de /ecobalance/Empresas, em "Authorize" colocar seu Token para estar autenticado e fazer o CRUD
- Para testar os endpoints, basta clicar em "try it out" e preencher de acordo com cada um
- Utilizar os seguintes JSON de exemplo nos métodos POST:

-> POST para registrar uma empresa

{
    "nome": "Empresa Exemplo",
    "email": "contato@empresaexemplo.com",
    "telefone": "1234567890",
    "cnpj": "12345678000190",
    "senha": "senhaSegura123"
}

-> POST para consumo

{
    "data": "2024-11-13T10:30:00",
    "consumo": 150.75,
    "empresaId": 1
}

-> POST para produção

{
    "data": "2024-11-13T10:30:00",
    "consumo": 150.75,
    "empresaId": 1
}

	
