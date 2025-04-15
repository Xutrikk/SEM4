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

CREATE TABLE ������ (
    ID_������ INT IDENTITY PRIMARY KEY, 
    �������� NVARCHAR(255) NOT NULL, 
    ����������_��_������ INT NOT NULL CHECK (����������_��_������ >= 0), 
    ���� DECIMAL(10,2) NOT NULL CHECK (���� > 0), 
    �������_��������� NVARCHAR(50) NOT NULL 
) ON FG1;
GO
CREATE TABLE ������� (
    ID_������� INT IDENTITY PRIMARY KEY, 
    ������� NVARCHAR(100) NOT NULL, 
    ��� NVARCHAR(100) NOT NULL, 
    �������� NVARCHAR(100) NULL, 
    ����� NVARCHAR(255) NOT NULL, 
    ������� NVARCHAR(20) NOT NULL, 
    Email NVARCHAR(100) NOT NULL UNIQUE, 
    �������_������ BIT DEFAULT 0 
) ON FG1;
GO
CREATE TABLE ������ (
    ID_������ INT IDENTITY PRIMARY KEY, 
    ID_������� INT NOT NULL, 
    ID_������ INT NOT NULL, 
    ����������_����������� INT NOT NULL CHECK (����������_����������� > 0), 
    ����_������� DATE NOT NULL, 
    �����_��������� DECIMAL(10,2) NOT NULL, 
    FOREIGN KEY (ID_�������) REFERENCES �������(ID_�������),
    FOREIGN KEY (ID_������) REFERENCES ������(ID_������)
) ON FG1;
GO

