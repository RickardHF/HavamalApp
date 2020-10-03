--
-- File generated with SQLiteStudio v3.2.1 on lør. sep 19 11:55:52 2020
--
-- Text encoding used: System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Language
CREATE TABLE Language (Id INTEGER PRIMARY KEY, Name TEXT, Translator TEXT, Description TEXT);

-- Table: Verses
CREATE TABLE Verses(
    VerseId integer not null,
    LanguageId integer not null,
    Content Text not null,
    Primary key (VerseId, LanguageId)
);
INSERT INTO Verses (VerseId, LanguageId, Content) VALUES (2, 2, '1');
INSERT INTO Verses (VerseId, LanguageId, Content) VALUES (3, 3, '2');
INSERT INTO Verses (VerseId, LanguageId, Content) VALUES (4, 4, '3');
INSERT INTO Verses (VerseId, LanguageId, Content) VALUES (5, 5, '4');

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
