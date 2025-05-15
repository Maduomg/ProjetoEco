create database Ecommerce;
use Ecommerce;

create table tbUsuarios(
Id int primary key auto_increment,
Nome varchar (30) not null,
Email  varchar (30) not null,
Senha  varchar (30) not null);

create table tbProdutos(
Id int primary key auto_increment,
Nome varchar (30) not null,
Descricao varchar (30) not null,
Preco decimal (10,2) not null,
Quantidade int not null);

