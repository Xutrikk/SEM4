UPDATE �������
SET �������_������ = 1
WHERE ID_������� IN (
    SELECT ID_������� FROM ������
    GROUP BY ID_�������
    HAVING SUM(�����_���������) > 50000
);

UPDATE ������
SET �����_��������� = �����_��������� * 0.98
WHERE ID_������� IN (
    SELECT ID_������� FROM ������� WHERE �������_������ = 1
);
