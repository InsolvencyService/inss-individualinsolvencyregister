
CREATE TABLE [dbo].[find_ip](
	[IpNo] [int] NOT NULL,
	[Forenames] [varchar](8000) NULL,
	[Surname] [varchar](8000) NULL,
	[RegisteredFirmName] [varchar](8000) NULL,
	[RegisteredAddressLine3] [varchar](8000) NULL,
	[RegisteredAddressLine4] [varchar](8000) NULL,
	[RegisteredAddressLine5] [varchar](8000) NULL,
	[RegisteredPostCode] [varchar](8000) NULL,
	[RegisteredPhone] [varchar](8000) NULL,
	[IpEmailAddress] [varchar](8000) NULL,
	[IncludeOnInternet] [varchar](8000) NULL,
	[LicensingBody] [varchar](8000) NULL,
PRIMARY KEY CLUSTERED 
(
	[IpNo] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[find_ip] ADD  DEFAULT (' ') FOR [IncludeOnInternet]
