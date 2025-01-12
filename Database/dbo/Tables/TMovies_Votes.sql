CREATE TABLE [dbo].[TMovies_Votes] (
    [ID]      BIGINT IDENTITY (1, 1) NOT NULL,
    [IDMovie] BIGINT NOT NULL,
    [IDUser]  BIGINT NULL,
    CONSTRAINT [PK_TMovies_Votes] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_TMovies_Votes_TUsers] FOREIGN KEY ([IDUser]) REFERENCES [dbo].[TUsers] ([ID])
);



