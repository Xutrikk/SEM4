USE master;
GO

CREATE DATABASE [HUT1-MyBase]
ON PRIMARY
(
    NAME = N'HUT1-MyBase_mdf',
    FILENAME = N'C:\BSTU\SEM4\BD\lab3bd\HUT1-MyBase_mdf.mdf',
    SIZE = 10MB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 1MB
),
FILEGROUP FG1
(
    NAME = N'HUT1-MyBase_fg1_1',
    FILENAME = N'C:\BSTU\SEM4\BD\lab3bd\HUT1-MyBase_fg1_1.ndf',
    SIZE = 10MB,
    MAXSIZE = 1GB,
    FILEGROWTH = 25%
),
(
    NAME = N'HUT1-MyBase_fg1_2',
    FILENAME = N'C:\BSTU\SEM4\BD\lab3bd\HUT1-MyBase_fg1_2.ndf',
    SIZE = 10MB,
    MAXSIZE = 1GB,
    FILEGROWTH = 25%
),
(
    NAME = N'HUT1-MyBase_fg1_3',
    FILENAME = N'C:\BSTU\SEM4\BD\lab3bd\HUT1-MyBase_fg1_3.ndf',
    SIZE = 10MB,
    MAXSIZE = 1GB,
    FILEGROWTH = 25%
)
LOG ON
(
    NAME = N'HUT1-MyBase_log',
    FILENAME = N'C:\BSTU\SEM4\BD\lab3bd\HUT1-MyBase_log.ldf',
    SIZE = 10MB,
    MAXSIZE = 10GB,
    FILEGROWTH = 10%
);
GO
USE [HUT1-MyBase];
GO

CREATE TABLE Товары (
    ID_Товара INT IDENTITY PRIMARY KEY, 
    Название NVARCHAR(255) NOT NULL, 
    Количество_на_складе INT NOT NULL CHECK (Количество_на_складе >= 0), 
    Цена DECIMAL(10,2) NOT NULL CHECK (Цена > 0), 
    Единица_измерения NVARCHAR(50) NOT NULL 
) ON FG1;
GO
CREATE TABLE Клиенты (
    ID_Клиента INT IDENTITY PRIMARY KEY, 
    Фамилия NVARCHAR(100) NOT NULL, 
    Имя NVARCHAR(100) NOT NULL, 
    Отчество NVARCHAR(100) NULL, 
    Адрес NVARCHAR(255) NOT NULL, 
    Телефон NVARCHAR(20) NOT NULL, 
    Email NVARCHAR(100) NOT NULL UNIQUE, 
    Признак_скидки BIT DEFAULT 0 
) ON FG1;
GO
CREATE TABLE Заказы (
    ID_Заказа INT IDENTITY PRIMARY KEY, 
    ID_Клиента INT NOT NULL, 
    ID_Товара INT NOT NULL, 
    Количество_заказанного INT NOT NULL CHECK (Количество_заказанного > 0), 
    Дата_продажи DATE NOT NULL, 
    Общая_стоимость DECIMAL(10,2) NOT NULL, 
    FOREIGN KEY (ID_Клиента) REFERENCES Клиенты(ID_Клиента),
    FOREIGN KEY (ID_Товара) REFERENCES Товары(ID_Товара)
) ON FG1;
GO

