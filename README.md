# CarRenting
Licenciaturas em Engenharia Informática Programação Web Trabalho Prático 1º Semestre - 2020/2021 

Requisitos:

1.Âmbito O trabalho prático tem como objetivo o desenvolvimento de uma aplicação web em ASP.NET MVC 5 (com a framework 4.7/4.8) recorrendo à linguagem de programação C# e ao SQLServer (localdb). Para implementar o trabalho proposto, cada grupo deve aplicar os conhecimentos lecionados nas aulas de Programação  Web  e  reutilizar  os  conhecimentos  lecionados  noutras  unidades  curriculares  da licenciatura, com o objetivo de desenvolver uma solução web coerente. 

2.Temas Existem dois temas passíveis de serem escolhidos. Ambos os temas procuram fomentar a criação de  aplicações  web  modernas  e  que  possam  representar  situações  reais,  sendo  também equivalentes no que diz respeito ao seu grau e dificuldade/complexidade. 

Tema A - eRenting Objetivo: Criação de uma aplicação web para gestão de aluguer de veículos. Descrição: Pretende-se com esta aplicação disponibilizar uma plataforma web no modelo SaaS (Software como serviço) que permita a empresas de Renting disponibilizar aos seus clientes uma forma simples e eficaz de alugarem os seus veículos, bem como de gerirem o seu parque de veículos. A plataforma está dividida em 3 áreas distintas: 

Área de administração da plataforma; 
Área para empresas de Renting; 
Área para os clientes. 

As principais funcionalidades/características da referida plataforma, e para cada tipo de utilizador são: 

Utilizadores anónimos 
Ver catálogo que produtos (veículos e preçários) 
Registar como cliente; 
Registar como empresa de Renting; 

Clientes:
Login / Recuperar password; 
Ver catálogo que produtos (veículos e preçários) oEfetuar uma reserva (escolher veículo(s) e datas da reserva). Só deve ser possível reservar o veículo caso este esteja disponível nas datas que o cliente escolhe; 
Ver histórico de reservas Funcionários das empresas de Renting: 
Gerir Reservas (ver lista de reservas, confirmar uma reserva, ver detalhes de uma reserva) da sua empresa; 
Entregar veículo ao cliente (Marcar o veículo como entregue indicando os km, estado do depósito e se tem defeitos); 
Receber veículo (Indicar km, nível do depósito e proceder à validação de uma checklist de verificações a fazer no veículo e upload de fotos com danos); 

Administração da empresa de Renting: 
Gestão de Funcionários oGestão do catálogo que veículos 
Definição da checklist de verificações a realizar para cada tipo de viatura 

Administração da plataforma: 
Gestão de empresas de Renting; 
Gestão de clientes (listar, visualizar, editar); 
Gestão de Categorias de veículos a disponibilizar; 
Ver catálogo de veículos disponibilizados pelas empresas de Renting;

NOTA: as soluções a construir devem representar uma situação real, ainda que dentro do contexto em que são desenvolvidas. Para cada um dos temas, as funcionalidades apresentadas são consideradas uma amostra primária de um conjunto de funcionalidades, no âmbito do problema, podendo ser adicionadas outras funcionalidades que sejam consideras necessárias e úteis à definição da solução.  

3.Implementação  Autenticação. É uma condição necessária, na concretização do trabalho prático, que a gestão dos processos de autenticação e de autorização seja realizada com base na ASP.NET Identity. Note-se que o módulo de autenticação de utilizadores (que inclui os diversos controlos de autenticação e a base de dados de autenticação) fica pré-definido na criação de um “ASP.NET MVC Project, com autenticação individual”. Nesse sentido, pode optar por iniciar o desenvolvimento da aplicação web com base neste tipo de projeto ASP.NET e, de seguida, efetuar as configurações/implementações que considere serem necessárias para o tipo de solução que está a desenvolver.  Base de Dados. Na aplicação web, as bases de dados devem ser concretizadas através do SQLServer (localdb). As bases de dados devem estar bem estruturadas, completas e consistentes, de forma a simplificar o desenvolvimento (atual e futuro) da aplicação web. A edição/manipulação dos dados, relativamente às bases de dados, deve ser efetuada através da Entity Framework 6.  
As bases de  dados, que forem incluídas  na  aplicação web (na diretoria App_Data), devem  estar preenchidas com uma quantidade suficiente de informação, de modo a permitir a demonstração e a avaliação das funcionalidades implementadas.  Modelos Visuais. Fica à responsabilidade de cada grupo a especificação da interface visual da aplicação web e da estrutura de “navegação” entre as suas diversas partes constituintes. Note-se que deve existir uma preocupação no sentido de desenvolver uma aplicação web visualmente agradável, com uma imagem consistente, “responsiva” e adequada a quem se destina. Cada grupo de trabalho pode desenvolver a interface visual na sua totalidade ou recorrer a templates, adaptando-os à solução web pretendida.  

4.Avaliação  Critérios gerais para a avaliação do trabalho prático:  
  1. A adequação e a abrangência da solução desenvolvida relativamente aos objetivos enunciados. A quantidade do trabalho realizado deve ser relevante e o âmbito do problema não deve estar reduzido, de forma excessiva.  
  2. A qualidade do código desenvolvido e a consistência entre as diversas “páginas” da aplicação web. Note-se que a estrutura de classes e a estrutura de dados devem representar convenientemente o domínio do problema e definir uma solução com qualidade. 
  3. A completude e a consistência dos dados de demonstração das funcionalidades da aplicação. Os dados  de  demonstração  devem  mostrar,  convenientemente,  todas  as  funcionalidades  da aplicação no domínio do problema. 
  4. Prazo de Entrega A entrega do trabalho prático tem de ser realizada até ao dia 8 de Janeiro de 2021. Após a entrega não é possível alterar o trabalho entregue. Aquando da entrega o grupo de alunos terá de escolher um dos “slots” disponível para a realização das defesas. E defesa dos dois elementos do grupo será feita no mesmo dia e hora correspondente ao “slot” escolhido para a defesa.  
  5. Modo de Entrega  A entrega do trabalho prático é realizada em formato digital no Moodle (um ficheiro ZIP com a seguinte designação: Tema_Grupo.zip).  Para além do projeto, que inclui o código fonte, a base de dados, as imagens e outros elementos que tenham sido utilizados, o ficheiro ZIP deve incluir um ficheiro em formato PDF com a seguinte informação:  1.Elementos do grupo de trabalho (Nome completo + Número de aluno).  2.Dados de acesso à aplicação web, tais como o login e a password atribuídos aos diversos “utilizadores exemplo”. 

 ## Manual
 
Palavra passe para todos os utilizadores: **1234**

Administradores do site

• admin@admin
• tobias@tobias

Clientes

•	 ricardojvsilva@hotmail.com
•	 antonio@antonio
•	 jose@jose
•	 maria@maria

Administradores de empresas

•	 ric@ric
•	 alberto@alberto
•	 jorge@jorge

Utilizadores de empresas

•	 sid@sid
•	 manel@manel
•	 carlo@carlos
•	 marco@marco
•	 marco@paulo
•	 zeca@zeca
•	 carlota@carlota

