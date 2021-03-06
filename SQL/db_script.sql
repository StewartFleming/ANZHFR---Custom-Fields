USE [ANZHFR]
GO
/****** Object:  Table [dbo].[Anaesthesia]    Script Date: 5/13/2014 12:18:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Anaesthesia](
	[AnaesthesiaID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_Anaesthesia] PRIMARY KEY CLUSTERED 
(
	[AnaesthesiaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Analgesia]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Analgesia](
	[AnalgesiaID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_Analgesia] PRIMARY KEY CLUSTERED 
(
	[AnalgesiaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AtypicalFracture]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AtypicalFracture](
	[AtypicalFractureID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_AtypicalFracture] PRIMARY KEY CLUSTERED 
(
	[AtypicalFractureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BoneMed]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BoneMed](
	[BoneMedID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_BoneMed] PRIMARY KEY CLUSTERED 
(
	[BoneMedID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CognitiveState]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CognitiveState](
	[CognitiveStateID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_CognitiveState] PRIMARY KEY CLUSTERED 
(
	[CognitiveStateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConsultantPresent]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConsultantPresent](
	[ConsultantPresentID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_ConsultantPresent] PRIMARY KEY CLUSTERED 
(
	[ConsultantPresentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DischargeDest]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DischargeDest](
	[DischargeDestID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_DischargeDest] PRIMARY KEY CLUSTERED 
(
	[DischargeDestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ethnic]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ethnic](
	[EthnicID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_Ethnic] PRIMARY KEY CLUSTERED 
(
	[EthnicID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FallsAssessment]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FallsAssessment](
	[FallsAssessmentID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_FallsAssessment] PRIMARY KEY CLUSTERED 
(
	[FallsAssessmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FractureSide]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FractureSide](
	[FractureSideID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_FractureSide] PRIMARY KEY CLUSTERED 
(
	[FractureSideID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FractureType]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FractureType](
	[FractureTypeID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_FractureType] PRIMARY KEY CLUSTERED 
(
	[FractureTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GeriatricAssessment]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GeriatricAssessment](
	[GeriatricAssessmentID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_GeriatricAssessment] PRIMARY KEY CLUSTERED 
(
	[GeriatricAssessmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Hospital]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Hospital](
	[HospitalID] [bigint] IDENTITY(1,1) NOT NULL,
	[HName] [varchar](50) NULL,
	[HStreetAddress1] [varchar](50) NULL,
	[HStreetAddress2] [varchar](50) NULL,
	[HSuburb] [varchar](50) NULL,
	[HCity] [varchar](50) NULL,
	[HPostCode] [varchar](4) NULL,
	[HCountry] [varchar](11) NULL,
	[HPhone] [varchar](30) NULL,
	[HAdminEmail] [varchar](50) NULL,
 CONSTRAINT [PK_Hospital] PRIMARY KEY CLUSTERED 
(
	[HospitalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Indig]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Indig](
	[IndigID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_Indig] PRIMARY KEY CLUSTERED 
(
	[IndigID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InterOpFracture]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InterOpFracture](
	[InterOpFractureID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_InterOpFracture] PRIMARY KEY CLUSTERED 
(
	[InterOpFractureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LengthofStay]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LengthofStay](
	[LengthofStayID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_LengthofStay] PRIMARY KEY CLUSTERED 
(
	[LengthofStayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Operation]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Operation](
	[OperationID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_Operation] PRIMARY KEY CLUSTERED 
(
	[OperationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Patient]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Patient](
	[ANZHFRID] [int] IDENTITY(1,1) NOT NULL,
	[HospitalID] [bigint] NOT NULL,
	[Name] [varchar](50) NULL,
	[Surname] [varchar](50) NULL,
	[MRN] [varchar](10) NULL,
	[Phone] [varchar](50) NULL,
	[DOB] [smalldatetime] NULL,
	[Sex] [smallint] NULL,
	[Indig] [smallint] NULL,
	[Ethnic] [smallint] NULL,
	[PostCode] [nvarchar](10) NULL,
	[Medicare] [varchar](20) NULL,
	[PatientType] [smallint] NULL,
	[UResidence] [smallint] NULL,
	[TransferHospital] [varchar](10) NULL,
	[TransferDateTime] [smalldatetime] NULL,
	[ArrivalDateTime] [smalldatetime] NULL,
	[DepartureDateTime] [smalldatetime] NULL,
	[InHospFractureDateTime] [smalldatetime] NULL,
	[WardType] [smallint] NULL,
	[PreAdWalk] [smallint] NULL,
	[AMTS] [tinyint] NULL,
	[CognitiveState] [smallint] NULL,
	[BoneMed] [smallint] NULL,
	[PreOpMedAss] [smallint] NULL,
	[FractureSide] [smallint] NULL,
	[AtypicalFracture] [smallint] NULL,
	[FractureType] [smallint] NULL,
	[Surgery] [smallint] NULL,
	[SurgeryDateTime] [smalldatetime] NULL,
	[SurgeryDelay] [smallint] NULL,
	[SurgeryDelayOther] [varchar](250) NULL,
	[Anaesthesia] [smallint] NULL,
	[Analgesia] [smallint] NULL,
	[ConsultantPresent] [smallint] NULL,
	[Operation] [smallint] NULL,
	[InterOpFracture] [smallint] NULL,
	[FullWeightBear] [smallint] NULL,
	[PressureUlcers] [smallint] NULL,
	[GeriatricAssessment] [smallint] NULL,
	[GeriatricAssDateTime] [smalldatetime] NULL,
	[FallsAssessment] [smallint] NULL,
	[BoneMedDischarge] [smallint] NULL,
	[WardDischargeDate] [smalldatetime] NULL,
	[DischargeDest] [smallint] NULL,
	[HospitalDischargeDate] [smalldatetime] NULL,
	[OLengthofStay] [smallint] NULL,
	[HLengthofStay] [smallint] NULL,
	[DischargeResidence] [smallint] NULL,
	[FollowupDate30] [smalldatetime] NULL,
	[HealthServiceDischarge30] [smalldatetime] NULL,
	[Survival30] [smallint] NULL,
	[Residence30] [smallint] NULL,
	[WeightBear30] [smallint] NULL,
	[WalkingAbility30] [smallint] NULL,
	[BoneMed30] [smallint] NULL,
	[Reoperation30] [smallint] NULL,
	[FollowupDate120] [smalldatetime] NULL,
	[HealthServiceDischarge120] [smalldatetime] NULL,
	[Survival120] [smallint] NULL,
	[Residence120] [smallint] NULL,
	[WeightBear120] [smallint] NULL,
	[WalkingAbility120] [smallint] NULL,
	[BoneMed120] [smallint] NULL,
	[Reoperation120] [smallint] NULL,
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[ANZHFRID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientType]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PatientType](
	[PatientTypeID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_PatientType] PRIMARY KEY CLUSTERED 
(
	[PatientTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PreAdWalk]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PreAdWalk](
	[PreAdWalkID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_PreAdWalk] PRIMARY KEY CLUSTERED 
(
	[PreAdWalkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PreOpMedAss]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PreOpMedAss](
	[PreOpMedAssID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_PreOpMedAss] PRIMARY KEY CLUSTERED 
(
	[PreOpMedAssID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PressureUlcers]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PressureUlcers](
	[PressureUlcersID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_PressureUlcers] PRIMARY KEY CLUSTERED 
(
	[PressureUlcersID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Reoperation]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Reoperation](
	[ReoperationID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_Reoperation] PRIMARY KEY CLUSTERED 
(
	[ReoperationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Residence]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Residence](
	[ResidenceID] [smallint] IDENTITY(1,1) NOT NULL,
	[Address] [varchar](50) NULL,
 CONSTRAINT [PK_Residence] PRIMARY KEY CLUSTERED 
(
	[ResidenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sex]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sex](
	[SexID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_Sex] PRIMARY KEY CLUSTERED 
(
	[SexID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Surgery]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Surgery](
	[SurgeryID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_Surgery] PRIMARY KEY CLUSTERED 
(
	[SurgeryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SurgeryDelay]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SurgeryDelay](
	[SurgeryDelayID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_SurgeryDelay] PRIMARY KEY CLUSTERED 
(
	[SurgeryDelayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Survival]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Survival](
	[SurvivalID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_Survival] PRIMARY KEY CLUSTERED 
(
	[SurvivalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [bigint] IDENTITY(1,1) NOT NULL,
	[UHospitalID] [bigint] NOT NULL,
	[UAccessLevel] [int] NOT NULL,
	[UEmail] [varchar](80) NOT NULL,
	[UPassword] [char](128) NOT NULL,
	[UPosition] [varchar](50) NULL,
	[UTitle] [varchar](10) NULL,
	[UFirstName] [varchar](50) NULL,
	[USurname] [varchar](50) NULL,
	[UMobile] [varchar](50) NULL,
	[UWorkPhone] [varchar](50) NULL,
	[UDateCreated] [datetime] NULL,
	[ULastAccessed] [datetime] NULL,
	[UActive] [tinyint] NOT NULL,
	[AdminNotes] [varchar](250) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WalkingAbility]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WalkingAbility](
	[WalkingAbilityID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_WalkingAbility] PRIMARY KEY CLUSTERED 
(
	[WalkingAbilityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WardType]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WardType](
	[WardTypeID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_WardType] PRIMARY KEY CLUSTERED 
(
	[WardTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WeightBear]    Script Date: 5/13/2014 12:18:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WeightBear](
	[WeightBearID] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_WeightBear] PRIMARY KEY CLUSTERED 
(
	[WeightBearID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Anaesthesia] ON 

GO
INSERT [dbo].[Anaesthesia] ([AnaesthesiaID], [Name]) VALUES (1, N'Anaesthesia 1')
GO
INSERT [dbo].[Anaesthesia] ([AnaesthesiaID], [Name]) VALUES (2, N'Anaesthesia 2')
GO
SET IDENTITY_INSERT [dbo].[Anaesthesia] OFF
GO
SET IDENTITY_INSERT [dbo].[Analgesia] ON 

GO
INSERT [dbo].[Analgesia] ([AnalgesiaID], [Name]) VALUES (1, N'Analgesia 1')
GO
INSERT [dbo].[Analgesia] ([AnalgesiaID], [Name]) VALUES (2, N'Analgesia 2')
GO
SET IDENTITY_INSERT [dbo].[Analgesia] OFF
GO
SET IDENTITY_INSERT [dbo].[AtypicalFracture] ON 

GO
INSERT [dbo].[AtypicalFracture] ([AtypicalFractureID], [Name]) VALUES (1, N'AtypicalFracture 1')
GO
INSERT [dbo].[AtypicalFracture] ([AtypicalFractureID], [Name]) VALUES (2, N' AtypicalFracture 2')
GO
SET IDENTITY_INSERT [dbo].[AtypicalFracture] OFF
GO
SET IDENTITY_INSERT [dbo].[BoneMed] ON 

GO
INSERT [dbo].[BoneMed] ([BoneMedID], [Name]) VALUES (1, N'BoneMed 1')
GO
INSERT [dbo].[BoneMed] ([BoneMedID], [Name]) VALUES (2, N'BoneMed 2')
GO
INSERT [dbo].[BoneMed] ([BoneMedID], [Name]) VALUES (3, N'BoneMed 3')
GO
INSERT [dbo].[BoneMed] ([BoneMedID], [Name]) VALUES (4, N'BoneMed 4')
GO
SET IDENTITY_INSERT [dbo].[BoneMed] OFF
GO
SET IDENTITY_INSERT [dbo].[CognitiveState] ON 

GO
INSERT [dbo].[CognitiveState] ([CognitiveStateID], [Name]) VALUES (1, N'CognitiveState 1')
GO
INSERT [dbo].[CognitiveState] ([CognitiveStateID], [Name]) VALUES (2, N'CognitiveState 2')
GO
SET IDENTITY_INSERT [dbo].[CognitiveState] OFF
GO
SET IDENTITY_INSERT [dbo].[ConsultantPresent] ON 

GO
INSERT [dbo].[ConsultantPresent] ([ConsultantPresentID], [Name]) VALUES (1, N'ConsultantPresent 1')
GO
INSERT [dbo].[ConsultantPresent] ([ConsultantPresentID], [Name]) VALUES (2, N'ConsultantPresent 2')
GO
SET IDENTITY_INSERT [dbo].[ConsultantPresent] OFF
GO
SET IDENTITY_INSERT [dbo].[DischargeDest] ON 

GO
INSERT [dbo].[DischargeDest] ([DischargeDestID], [Name]) VALUES (1, N'DischargeDest 1')
GO
INSERT [dbo].[DischargeDest] ([DischargeDestID], [Name]) VALUES (2, N'DischargeDest 2')
GO
SET IDENTITY_INSERT [dbo].[DischargeDest] OFF
GO
SET IDENTITY_INSERT [dbo].[Ethnic] ON 

GO
INSERT [dbo].[Ethnic] ([EthnicID], [Name]) VALUES (1, N'Ethnic 1')
GO
INSERT [dbo].[Ethnic] ([EthnicID], [Name]) VALUES (2, N'Ethnic 2')
GO
SET IDENTITY_INSERT [dbo].[Ethnic] OFF
GO
SET IDENTITY_INSERT [dbo].[FallsAssessment] ON 

GO
INSERT [dbo].[FallsAssessment] ([FallsAssessmentID], [Name]) VALUES (1, N'FallsAssessment 1')
GO
INSERT [dbo].[FallsAssessment] ([FallsAssessmentID], [Name]) VALUES (2, N'FallsAssessment 2')
GO
SET IDENTITY_INSERT [dbo].[FallsAssessment] OFF
GO
SET IDENTITY_INSERT [dbo].[FractureSide] ON 

GO
INSERT [dbo].[FractureSide] ([FractureSideID], [Name]) VALUES (1, N'FractureSide 1')
GO
INSERT [dbo].[FractureSide] ([FractureSideID], [Name]) VALUES (2, N'FractureSide 2')
GO
SET IDENTITY_INSERT [dbo].[FractureSide] OFF
GO
SET IDENTITY_INSERT [dbo].[FractureType] ON 

GO
INSERT [dbo].[FractureType] ([FractureTypeID], [Name]) VALUES (1, N'FractureType 1')
GO
INSERT [dbo].[FractureType] ([FractureTypeID], [Name]) VALUES (2, N'FractureType 2')
GO
SET IDENTITY_INSERT [dbo].[FractureType] OFF
GO
SET IDENTITY_INSERT [dbo].[GeriatricAssessment] ON 

GO
INSERT [dbo].[GeriatricAssessment] ([GeriatricAssessmentID], [Name]) VALUES (1, N'GeriatricAssessment 1')
GO
INSERT [dbo].[GeriatricAssessment] ([GeriatricAssessmentID], [Name]) VALUES (2, N'GeriatricAssessment 2')
GO
SET IDENTITY_INSERT [dbo].[GeriatricAssessment] OFF
GO
SET IDENTITY_INSERT [dbo].[Hospital] ON 

GO
INSERT [dbo].[Hospital] ([HospitalID], [HName], [HStreetAddress1], [HStreetAddress2], [HSuburb], [HCity], [HPostCode], [HCountry], [HPhone], [HAdminEmail]) VALUES (1, N'My Hospital', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Hospital] ([HospitalID], [HName], [HStreetAddress1], [HStreetAddress2], [HSuburb], [HCity], [HPostCode], [HCountry], [HPhone], [HAdminEmail]) VALUES (2, N'Hospital 2', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Hospital] OFF
GO
SET IDENTITY_INSERT [dbo].[Indig] ON 

GO
INSERT [dbo].[Indig] ([IndigID], [Name]) VALUES (1, N'Indig 1')
GO
INSERT [dbo].[Indig] ([IndigID], [Name]) VALUES (2, N'Indig 2')
GO
SET IDENTITY_INSERT [dbo].[Indig] OFF
GO
SET IDENTITY_INSERT [dbo].[InterOpFracture] ON 

GO
INSERT [dbo].[InterOpFracture] ([InterOpFractureID], [Name]) VALUES (1, N'InterOpFracture 1')
GO
INSERT [dbo].[InterOpFracture] ([InterOpFractureID], [Name]) VALUES (2, N'InterOpFracture 2')
GO
SET IDENTITY_INSERT [dbo].[InterOpFracture] OFF
GO
SET IDENTITY_INSERT [dbo].[LengthofStay] ON 

GO
INSERT [dbo].[LengthofStay] ([LengthofStayID], [Name]) VALUES (1, N'LengthofStay 1')
GO
INSERT [dbo].[LengthofStay] ([LengthofStayID], [Name]) VALUES (2, N'LengthofStay 2')
GO
SET IDENTITY_INSERT [dbo].[LengthofStay] OFF
GO
SET IDENTITY_INSERT [dbo].[Operation] ON 

GO
INSERT [dbo].[Operation] ([OperationID], [Name]) VALUES (1, N'Operation 1')
GO
INSERT [dbo].[Operation] ([OperationID], [Name]) VALUES (2, N'Operation 2')
GO
INSERT [dbo].[Operation] ([OperationID], [Name]) VALUES (3, N'Operation 3')
GO
INSERT [dbo].[Operation] ([OperationID], [Name]) VALUES (4, N'Operation 4')
GO
SET IDENTITY_INSERT [dbo].[Operation] OFF
GO
SET IDENTITY_INSERT [dbo].[Patient] ON 

GO
INSERT [dbo].[Patient] ([ANZHFRID], [HospitalID], [Name], [Surname], [MRN], [Phone], [DOB], [Sex], [Indig], [Ethnic], [PostCode], [Medicare], [PatientType], [UResidence], [TransferHospital], [TransferDateTime], [ArrivalDateTime], [DepartureDateTime], [InHospFractureDateTime], [WardType], [PreAdWalk], [AMTS], [CognitiveState], [BoneMed], [PreOpMedAss], [FractureSide], [AtypicalFracture], [FractureType], [Surgery], [SurgeryDateTime], [SurgeryDelay], [SurgeryDelayOther], [Anaesthesia], [Analgesia], [ConsultantPresent], [Operation], [InterOpFracture], [FullWeightBear], [PressureUlcers], [GeriatricAssessment], [GeriatricAssDateTime], [FallsAssessment], [BoneMedDischarge], [WardDischargeDate], [DischargeDest], [HospitalDischargeDate], [OLengthofStay], [HLengthofStay], [DischargeResidence], [FollowupDate30], [HealthServiceDischarge30], [Survival30], [Residence30], [WeightBear30], [WalkingAbility30], [BoneMed30], [Reoperation30], [FollowupDate120], [HealthServiceDischarge120], [Survival120], [Residence120], [WeightBear120], [WalkingAbility120], [BoneMed120], [Reoperation120]) VALUES (5, 1, N'John', N'Bradman', N'00001', N'01712824016', CAST(0x56180000 AS SmallDateTime), 1, 1, 1, N'4455', N'0456546545', 1, 1, N'Hospital2', CAST(0xA3260004 AS SmallDateTime), CAST(0xA3180433 AS SmallDateTime), CAST(0xA32C0433 AS SmallDateTime), CAST(0xA3170433 AS SmallDateTime), 1, 1, 6, 1, 1, 1, 1, 1, 1, 1, CAST(0xA32D0434 AS SmallDateTime), 1, NULL, 1, 1, 1, 1, 1, 1, 1, 1, CAST(0xA3240434 AS SmallDateTime), 1, 1, NULL, 2, NULL, 1, 1, 2, NULL, NULL, 3, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Patient] ([ANZHFRID], [HospitalID], [Name], [Surname], [MRN], [Phone], [DOB], [Sex], [Indig], [Ethnic], [PostCode], [Medicare], [PatientType], [UResidence], [TransferHospital], [TransferDateTime], [ArrivalDateTime], [DepartureDateTime], [InHospFractureDateTime], [WardType], [PreAdWalk], [AMTS], [CognitiveState], [BoneMed], [PreOpMedAss], [FractureSide], [AtypicalFracture], [FractureType], [Surgery], [SurgeryDateTime], [SurgeryDelay], [SurgeryDelayOther], [Anaesthesia], [Analgesia], [ConsultantPresent], [Operation], [InterOpFracture], [FullWeightBear], [PressureUlcers], [GeriatricAssessment], [GeriatricAssDateTime], [FallsAssessment], [BoneMedDischarge], [WardDischargeDate], [DischargeDest], [HospitalDischargeDate], [OLengthofStay], [HLengthofStay], [DischargeResidence], [FollowupDate30], [HealthServiceDischarge30], [Survival30], [Residence30], [WeightBear30], [WalkingAbility30], [BoneMed30], [Reoperation30], [FollowupDate120], [HealthServiceDischarge120], [Survival120], [Residence120], [WeightBear120], [WalkingAbility120], [BoneMed120], [Reoperation120]) VALUES (6, 1, N'Stewart', N'Fleming', N'12345', N'61499949494', NULL, 1, NULL, NULL, N'4034', NULL, NULL, NULL, NULL, CAST(0xA31E0230 AS SmallDateTime), CAST(0xA3080230 AS SmallDateTime), NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Patient] OFF
GO
SET IDENTITY_INSERT [dbo].[PatientType] ON 

GO
INSERT [dbo].[PatientType] ([PatientTypeID], [Name]) VALUES (1, N'Patient Type 1')
GO
INSERT [dbo].[PatientType] ([PatientTypeID], [Name]) VALUES (2, N'PatientType  2')
GO
SET IDENTITY_INSERT [dbo].[PatientType] OFF
GO
SET IDENTITY_INSERT [dbo].[PreAdWalk] ON 

GO
INSERT [dbo].[PreAdWalk] ([PreAdWalkID], [Name]) VALUES (1, N'PreAdWalk 1')
GO
INSERT [dbo].[PreAdWalk] ([PreAdWalkID], [Name]) VALUES (2, N'PreAdWalk 2')
GO
SET IDENTITY_INSERT [dbo].[PreAdWalk] OFF
GO
SET IDENTITY_INSERT [dbo].[PreOpMedAss] ON 

GO
INSERT [dbo].[PreOpMedAss] ([PreOpMedAssID], [Name]) VALUES (1, N'PreOpMedAss 1')
GO
INSERT [dbo].[PreOpMedAss] ([PreOpMedAssID], [Name]) VALUES (2, N'PreOpMedAss 2')
GO
SET IDENTITY_INSERT [dbo].[PreOpMedAss] OFF
GO
SET IDENTITY_INSERT [dbo].[PressureUlcers] ON 

GO
INSERT [dbo].[PressureUlcers] ([PressureUlcersID], [Name]) VALUES (1, N'PressureUlcers 1')
GO
INSERT [dbo].[PressureUlcers] ([PressureUlcersID], [Name]) VALUES (2, N'PressureUlcers 2')
GO
SET IDENTITY_INSERT [dbo].[PressureUlcers] OFF
GO
SET IDENTITY_INSERT [dbo].[Reoperation] ON 

GO
INSERT [dbo].[Reoperation] ([ReoperationID], [Name]) VALUES (1, N'Reoperation 1')
GO
INSERT [dbo].[Reoperation] ([ReoperationID], [Name]) VALUES (2, N'Reoperation 2')
GO
INSERT [dbo].[Reoperation] ([ReoperationID], [Name]) VALUES (3, N'Reoperation 3')
GO
INSERT [dbo].[Reoperation] ([ReoperationID], [Name]) VALUES (4, N'Reoperation 4')
GO
SET IDENTITY_INSERT [dbo].[Reoperation] OFF
GO
SET IDENTITY_INSERT [dbo].[Residence] ON 

GO
INSERT [dbo].[Residence] ([ResidenceID], [Address]) VALUES (1, N'Residence 1')
GO
INSERT [dbo].[Residence] ([ResidenceID], [Address]) VALUES (2, N'Residence 2')
GO
INSERT [dbo].[Residence] ([ResidenceID], [Address]) VALUES (3, N'Residence 3')
GO
INSERT [dbo].[Residence] ([ResidenceID], [Address]) VALUES (4, N'Residence 4')
GO
SET IDENTITY_INSERT [dbo].[Residence] OFF
GO
SET IDENTITY_INSERT [dbo].[Sex] ON 

GO
INSERT [dbo].[Sex] ([SexID], [Name]) VALUES (1, N'Male')
GO
INSERT [dbo].[Sex] ([SexID], [Name]) VALUES (2, N'Female')
GO
SET IDENTITY_INSERT [dbo].[Sex] OFF
GO
SET IDENTITY_INSERT [dbo].[Surgery] ON 

GO
INSERT [dbo].[Surgery] ([SurgeryID], [Name]) VALUES (1, N'Surgery 1')
GO
INSERT [dbo].[Surgery] ([SurgeryID], [Name]) VALUES (2, N'Surgery 2')
GO
SET IDENTITY_INSERT [dbo].[Surgery] OFF
GO
SET IDENTITY_INSERT [dbo].[SurgeryDelay] ON 

GO
INSERT [dbo].[SurgeryDelay] ([SurgeryDelayID], [Name]) VALUES (1, N'SurgeryDelay 1')
GO
INSERT [dbo].[SurgeryDelay] ([SurgeryDelayID], [Name]) VALUES (2, N'SurgeryDelay 2')
GO
SET IDENTITY_INSERT [dbo].[SurgeryDelay] OFF
GO
SET IDENTITY_INSERT [dbo].[Survival] ON 

GO
INSERT [dbo].[Survival] ([SurvivalID], [Name]) VALUES (1, N'Survival 1')
GO
INSERT [dbo].[Survival] ([SurvivalID], [Name]) VALUES (2, N'Survival 2')
GO
INSERT [dbo].[Survival] ([SurvivalID], [Name]) VALUES (3, N'Survival 3')
GO
INSERT [dbo].[Survival] ([SurvivalID], [Name]) VALUES (4, N'Survival 4')
GO
SET IDENTITY_INSERT [dbo].[Survival] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([UserID], [UHospitalID], [UAccessLevel], [UEmail], [UPassword], [UPosition], [UTitle], [UFirstName], [USurname], [UMobile], [UWorkPhone], [UDateCreated], [ULastAccessed], [UActive], [AdminNotes]) VALUES (1, 1, 0, N'mail.parvezalam@gmail.com', N'7C4A8D09CA3762AF61E59520943DC26494F8941B                                                                                        ', NULL, NULL, N'Parvez', N'Alam', NULL, NULL, NULL, NULL, 0, NULL)
GO
INSERT [dbo].[User] ([UserID], [UHospitalID], [UAccessLevel], [UEmail], [UPassword], [UPosition], [UTitle], [UFirstName], [USurname], [UMobile], [UWorkPhone], [UDateCreated], [ULastAccessed], [UActive], [AdminNotes]) VALUES (3, 1, 0, N'ashrafwebreaktor@gmail.com', N'7C4A8D09CA3762AF61E59520943DC26494F8941B                                                                                        ', NULL, NULL, N'Ashraf', N'Mr', NULL, NULL, NULL, NULL, 0, NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[WalkingAbility] ON 

GO
INSERT [dbo].[WalkingAbility] ([WalkingAbilityID], [Name]) VALUES (1, N'WalkingAbility 1')
GO
INSERT [dbo].[WalkingAbility] ([WalkingAbilityID], [Name]) VALUES (2, N'WalkingAbility 2')
GO
INSERT [dbo].[WalkingAbility] ([WalkingAbilityID], [Name]) VALUES (3, N'WalkingAbility 3')
GO
INSERT [dbo].[WalkingAbility] ([WalkingAbilityID], [Name]) VALUES (4, N'WalkingAbility 4')
GO
SET IDENTITY_INSERT [dbo].[WalkingAbility] OFF
GO
SET IDENTITY_INSERT [dbo].[WardType] ON 

GO
INSERT [dbo].[WardType] ([WardTypeID], [Name]) VALUES (1, N'Ward Type 1')
GO
INSERT [dbo].[WardType] ([WardTypeID], [Name]) VALUES (2, N'Ward Type 2')
GO
SET IDENTITY_INSERT [dbo].[WardType] OFF
GO
SET IDENTITY_INSERT [dbo].[WeightBear] ON 

GO
INSERT [dbo].[WeightBear] ([WeightBearID], [Name]) VALUES (1, N'WeightBear 1')
GO
INSERT [dbo].[WeightBear] ([WeightBearID], [Name]) VALUES (2, N'WeightBear 2')
GO
INSERT [dbo].[WeightBear] ([WeightBearID], [Name]) VALUES (3, N'WeightBear 3')
GO
INSERT [dbo].[WeightBear] ([WeightBearID], [Name]) VALUES (4, N'WeightBear 4')
GO
SET IDENTITY_INSERT [dbo].[WeightBear] OFF
GO
ALTER TABLE [dbo].[Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_Hospital] FOREIGN KEY([HospitalID])
REFERENCES [dbo].[Hospital] ([HospitalID])
GO
ALTER TABLE [dbo].[Patient] CHECK CONSTRAINT [FK_Patient_Hospital]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Hospital] FOREIGN KEY([UHospitalID])
REFERENCES [dbo].[Hospital] ([HospitalID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Hospital]
GO
