CREATE TABLE [dbo].[TMovies] (
    [ID]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [Titre]          VARCHAR (50)  NOT NULL,
    [Réalisateur]    VARCHAR(50) NULL, 
    [Description]    VARCHAR (MAX) NULL,
    [ENDescription]  VARCHAR (MAX) NULL,
    [Durée]          DECIMAL (18)  NULL,
    [Date de sortie] DATE          NULL,
    [Vidéo]          VARCHAR (MAX) NULL,
    [Affiche]        VARCHAR (MAX) NULL,
    [Statut]         NCHAR (10)    NOT NULL,
    CONSTRAINT [PK_TMovies] PRIMARY KEY CLUSTERED ([ID] ASC)
);





