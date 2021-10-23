﻿CREATE TABLE [dbo].[Sections]
(
	[ID] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [PK_Sections] PRIMARY KEY,
    [Owner] UNIQUEIDENTIFIER NOT NULL,
    [Page] UNIQUEIDENTIFIER NOT NULL,
    [Title] NVARCHAR(256) NOT NULL,
    [Type] INT NOT NULL,
    [Order] INT NOT NULL,
    CONSTRAINT [FK_Sections_Pages] FOREIGN KEY ([Page]) REFERENCES [Pages] ([ID]),
    CONSTRAINT [FK_Sections_Users] FOREIGN KEY ([Owner]) REFERENCES [Users]([ID]),
    CONSTRAINT [FK_Sections_SectionTypes] FOREIGN KEY ([Type]) REFERENCES [SectionTypes]([ID])
)
