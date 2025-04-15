CREATE DATABASE [HUT-MyBase];
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
    FOREIGN KEY (ID_�������) REFERENCES �������(ID_�������),
    FOREIGN KEY (ID_������) REFERENCES ������(ID_������)
);
GO
INSERT INTO ������ (��������, ����������_��_������, ����, �������_���������)
VALUES 
('������� Lenovo', 10, 75000.00, '��'),
('���������� Logitech', 50, 3500.00, '��'),
('���� Razer', 30, 5500.00, '��');

INSERT INTO ������� (�������, ���, ��������, �����, �������, Email)
VALUES 
('������', '�������', '��������', '������, ��. ������, 10', '+79001112233', 'ivanov@mail.ru'),
('������', '�����', '�������������', '�����-���������, ������� ��., 25', '+79112223344', 'petrov@mail.ru');

INSERT INTO ������ (ID_�������, ID_������, ����������_�����������, ����_�������)
VALUES 
(1, 1, 1, '2025-02-18'),
(2, 2, 2, '2025-02-18'),
(2, 3, 1, '2025-02-18');
/*lab5*/
SELECT 
    �.�������� AS �����, 
    �.����_������� AS ����_�������, 
    �.���� AS ����
FROM ������ �, ������ �
WHERE 
    �.ID_������ = �.ID_������ 
    AND 
    �.ID_������� IN (
        SELECT �.ID_������� 
        FROM ������� � 
        WHERE �.����� LIKE '%������%'
    );

	SELECT 
    �.�������� AS �����, 
    �.����_������� AS ����_�������, 
    �.���� AS ����
FROM ������ �
INNER JOIN ������ � ON �.ID_������ = �.ID_������
WHERE 
    �.ID_������� IN (
        SELECT �.ID_������� 
        FROM ������� � 
        WHERE �.����� LIKE '%������%'
    );

	SELECT 
    �.�������� AS �����, 
    �.����_������� AS ����_�������, 
    �.���� AS ����
FROM ������ �
INNER JOIN ������ � ON �.ID_������ = �.ID_������
INNER JOIN ������� � ON �.ID_������� = �.ID_�������
WHERE 
    �.����� LIKE '%������%';

	SELECT 
    �1.��������, 
    �1.����, 
    �1.�������_���������
FROM ������ �1
WHERE �1.���� = (
    SELECT TOP 1 �2.���� 
    FROM ������ �2 
    WHERE �2.�������_��������� = �1.�������_��������� 
    ORDER BY �2.���� DESC
)
ORDER BY �1.���� DESC;

SELECT 
    �.������� + ' ' + �.��� AS ������
FROM ������� �
WHERE NOT EXISTS (
    SELECT 1 
    FROM ������ � 
    WHERE �.ID_������� = �.ID_�������
);

SELECT 
    (SELECT AVG(����) FROM ������ WHERE �������� LIKE '%�������%') AS �������_����_���������,
    (SELECT AVG(����) FROM ������ WHERE �������� LIKE '%����������%') AS �������_����_���������,
    (SELECT AVG(����) FROM ������ WHERE �������� LIKE '%����%') AS �������_����_�����;

	SELECT 
    ��������, 
    ����
FROM ������
WHERE ���� >= ALL (
    SELECT ���� 
    FROM ������ 
    WHERE �������� LIKE '�%'
);

SELECT 
    ��������, 
    ����
FROM ������
WHERE ���� > ANY (
    SELECT ���� 
    FROM ������ 
    WHERE �������� LIKE '�%'
);