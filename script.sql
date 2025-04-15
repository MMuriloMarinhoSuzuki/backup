CREATE DATABASE bdEcommerce;

USE bdEcommerce;

CREATE TABLE Usuario(
	Id int primary key auto_increment,
    Nome varchar(50) not null,
    Email varchar(50) not null,
    Senha varchar(50) not null
);

CREATE TABLE Cliente(
		CodCli int primary key auto_increment,
        NomeCli varchar(50) not null,
        TeleCli varchar(20) not null,
        EmailCli varchar(50) not null
);

CREATE TABLE Produto(
		CodProd int primary key auto_increment,
        NomeProd varchar(50),
        DescProd varchar(60),
        QuantProd int,
        PrecoProd float
);

SELECT * FROM Usuario;
SELECT * FROM Cliente;
SELECT * FROM Produto;

insert into Usuario(Nome, Email, Senha)VALUES('admin','admin@gmail.com','12345678');