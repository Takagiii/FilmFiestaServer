CREATE TABLE [dbo].[TSubscriptions] (
    [ID]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [StartDate] DATETIME     NULL,
    [EndDate]   DATETIME     NOT NULL,
    CONSTRAINT [PK_TSubscriptions] PRIMARY KEY CLUSTERED ([ID] ASC)
);

