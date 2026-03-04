USE [ContestSystemDb]
GO
/****** Object:  Table [dbo].[ContestParticipants]    Script Date: 27-02-2026 04:45:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContestParticipants](
	[Id] [uniqueidentifier] NOT NULL,
	[ContestId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Score] [int] NOT NULL,
	[IsCompleted] [bit] NOT NULL,
	[SubmittedAt] [datetime2](7) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contests]    Script Date: 27-02-2026 04:45:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contests](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[AccessLevel] [int] NOT NULL,
	[StartTime] [datetime2](7) NOT NULL,
	[EndTime] [datetime2](7) NOT NULL,
	[Prize] [nvarchar](500) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Options]    Script Date: 27-02-2026 04:45:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Options](
	[Id] [uniqueidentifier] NOT NULL,
	[QuestionId] [uniqueidentifier] NOT NULL,
	[Text] [nvarchar](500) NOT NULL,
	[IsCorrect] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 27-02-2026 04:45:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[Id] [uniqueidentifier] NOT NULL,
	[ContestId] [uniqueidentifier] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[Type] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAnswers]    Script Date: 27-02-2026 04:45:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAnswers](
	[Id] [uniqueidentifier] NOT NULL,
	[ContestParticipantId] [uniqueidentifier] NOT NULL,
	[QuestionId] [uniqueidentifier] NOT NULL,
	[SelectedOptionIdsJson] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 27-02-2026 04:45:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[PasswordHash] [nvarchar](500) NOT NULL,
	[Role] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ContestParticipants] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[ContestParticipants] ADD  DEFAULT ((0)) FOR [Score]
GO
ALTER TABLE [dbo].[ContestParticipants] ADD  DEFAULT ((0)) FOR [IsCompleted]
GO
ALTER TABLE [dbo].[ContestParticipants] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Contests] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Contests] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Options] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Options] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Questions] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Questions] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[UserAnswers] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserAnswers] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ContestParticipants]  WITH CHECK ADD  CONSTRAINT [FK_Participants_Contest] FOREIGN KEY([ContestId])
REFERENCES [dbo].[Contests] ([Id])
GO
ALTER TABLE [dbo].[ContestParticipants] CHECK CONSTRAINT [FK_Participants_Contest]
GO
ALTER TABLE [dbo].[ContestParticipants]  WITH CHECK ADD  CONSTRAINT [FK_Participants_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ContestParticipants] CHECK CONSTRAINT [FK_Participants_User]
GO
ALTER TABLE [dbo].[Options]  WITH CHECK ADD  CONSTRAINT [FK_Options_Questions] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Questions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Options] CHECK CONSTRAINT [FK_Options_Questions]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_Contests] FOREIGN KEY([ContestId])
REFERENCES [dbo].[Contests] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_Contests]
GO
ALTER TABLE [dbo].[UserAnswers]  WITH CHECK ADD  CONSTRAINT [FK_UserAnswers_Participant] FOREIGN KEY([ContestParticipantId])
REFERENCES [dbo].[ContestParticipants] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserAnswers] CHECK CONSTRAINT [FK_UserAnswers_Participant]
GO
ALTER TABLE [dbo].[UserAnswers]  WITH CHECK ADD  CONSTRAINT [FK_UserAnswers_Question] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Questions] ([Id])
GO
ALTER TABLE [dbo].[UserAnswers] CHECK CONSTRAINT [FK_UserAnswers_Question]
GO
