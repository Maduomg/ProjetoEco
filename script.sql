create database Ecommerce;
use Ecommerce;

create table tbUsuarios(
Id int primary key auto_increment,
Nome varchar(50),
Email  varchar(50),
Senha  varchar(50)
);

create table tbProdutos(
Id int primary key auto_increment,
Nome varchar(50),
Descricao varchar(50),
Preco decimal(10,2),
Quantidade int);

