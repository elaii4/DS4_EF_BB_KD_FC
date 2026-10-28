CREATE DATABASE CrudWindowsForms;
GO
USE CrudWindowsForms;
GO

CREATE TABLE dbo.People (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Age INT NOT NULL
);
GO

INSERT INTO dbo.People (Name, Age)
VALUES ('Ana', 20), ('Luis', 25), ('Maria', 22);
GO

SELECT Id, Name, Age FROM dbo.People ORDER BY Id;
