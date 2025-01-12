CREATE TABLE [dbo].[TUsers] (
    [ID]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50) NULL,
    [Password]       VARCHAR (50) NOT NULL,
    [Role]           NCHAR (10)   NOT NULL,
    [IDSubscription] BIGINT       NULL,
    CONSTRAINT [PK_TUsers] PRIMARY KEY CLUSTERED ([ID] ASC)
);



