CREATE TABLE [dbo].[TMovies_Genres] (
    [ID]       BIGINT IDENTITY (1, 1) NOT NULL,
    [Movie_ID] BIGINT NOT NULL,
    [Genre_ID] BIGINT NOT NULL,
    CONSTRAINT [FK_TMovies_Genres_TGenres] FOREIGN KEY ([Genre_ID]) REFERENCES [dbo].[TGenres] ([ID]),
    CONSTRAINT [FK_TMovies_Genres_TMovies] FOREIGN KEY ([Movie_ID]) REFERENCES [dbo].[TMovies] ([ID])
);



