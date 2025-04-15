CREATE TABLE ������ (
    ID_������ INT IDENTITY PRIMARY KEY, 
    �������� NVARCHAR(255) NOT NULL, 
    ����������_��_������ INT NOT NULL, 
    ���� DECIMAL(10,2) NOT NULL, 
    �������_��������� NVARCHAR(50) NOT NULL 
);
GO


CREATE TABLE ������� (
    ID_������� INT IDENTITY PRIMARY KEY, 
    ������� NVARCHAR(100) NOT NULL, 
    ��� NVARCHAR(100) NOT NULL, 
    �������� NVARCHAR(100) NULL, 
    ����� NVARCHAR(255) NOT NULL, 
    ������� NVARCHAR(20) NOT NULL, 
    Email NVARCHAR(100) NOT NULL UNIQUE, 
    �������_������ BIT DEFAULT 0 -- 
);
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
);
GO
