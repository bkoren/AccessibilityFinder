CREATE DATABASE AccessibilityDB
GO

USE AccessibilityDB
GO

CREATE TABLE [Type] 
(
    [ID] int PRIMARY KEY IDENTITY(1,1),
    [Name] nvarchar (50) NOT NULL
);

CREATE TABLE [Accessibility]
(
    [ID] int PRIMARY KEY IDENTITY(1,1),
    [Name] nvarchar(100) NOT NULL
);

CREATE TABLE [Activity]
(
    [ID] int PRIMARY KEY IDENTITY(1,1),
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(MAX),
    [Address] nvarchar(200),
    [Contact] nvarchar(50),
	[Email] nvarchar(100),
	[Views] int NOT NULL DEFAULT 0,
    [TypeID] int FOREIGN KEY REFERENCES [Type](ID),		
);

CREATE TABLE [Activity_Accessibility]
(
	[ActivityID] int NOT NULL FOREIGN KEY REFERENCES [Activity](ID),
	[AccessibilityID] int NOT NULL FOREIGN KEY REFERENCES [Accessibility](ID)
)

CREATE TABLE [User] 
(
    [ID] int PRIMARY KEY IDENTITY(1,1),
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
	[Username] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) UNIQUE NOT NULL,
    [PasswordHash] nvarchar(256) NOT NULL,
	[PasswordSalt] nvarchar(256) NOT NULL,
	[CreatedAt] datetime DEFAULT GETDATE(),
    [IsAdmin] nvarchar(10) CHECK ([IsAdmin] = 'true' OR [IsAdmin] = 'false') DEFAULT 'false',
);

CREATE TABLE [Review]
(
    [ID] int PRIMARY KEY IDENTITY(1,1),
    [Comment] nvarchar(MAX),
    [Grade] int CHECK ([Grade] >= 1 AND [Grade] <= 5),
    [Date] datetime DEFAULT GETDATE(),
	[UserID] int FOREIGN KEY REFERENCES [User](ID),
    [ActivityID] int FOREIGN KEY REFERENCES [Activity](ID),	
);

CREATE TABLE [Log] 
(
    [ID] int PRIMARY KEY IDENTITY(1,1),
    [Level] nvarchar(20), 
    [Message] nvarchar(MAX),
    [Timestamp] datetime DEFAULT GETDATE()
);

INSERT INTO [User] ([FirstName], [LastName], [Username], [Email], [PasswordHash], [PasswordSalt], [IsAdmin]) VALUES 
('Michael', 'Scott', 'prisonmike', 'm.scott@dundermifflin.com',  'ef92b778ba7157c70335032545f445037e96b52750e50f38b255e2e4e6f47702', '8f4e2c39d01b', 'false'),
('Dwight', 'Schrute', 'beetsbears', 'd.schrute@schrutefarms.com', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', '2a9b4f8c1d3e', 'true'),
('Pam', 'Beesly', 'pambeesly', 'pam.b@art.com', 'a7e06a3a9b1c78e6f1f50a8d8e5f8f9d8c7b6a543210fedcba9876543210abcd', '7c8d9e0f1a2b', 'false'),
('Jim', 'Halpert', 'tuna', 'j.halpert@pranks.com', 'f1d2c3b4a5968778695041322314059687786950413223140596877869504132', '0e1f2a3b4c5d', 'false'),
('Oscar', 'Martinez', 'actually_oscar', 'o.martinez@facts.com', '6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b', '9f8e7d6c5b4a', 'false'),
('Bruno', 'Koren', 'bkoren', 'bkoren@example.com', 'MRGJwmd5xrgWNY98/TTQAH3zfVY0lOL4n2JRz5IwJdw=', 'l5L/Nd+li3AT+lUT0h4AqA==', 'true');

INSERT INTO [Accessibility] ([Name]) VALUES 
('Induction Loop Systems'), ('Large Print Menus'), ('Step-Free Access'), 
('Assistance Dog Water Bowls'), ('Changing Places Toilets'), ('Strobe-Free Lighting'), 
('Calm Hours'), ('Lowered Counters'), ('Touch Tours'), ('Sign Language Interpretation');

INSERT INTO [Type] ([Name]) VALUES 
('Co-working Space'),    
('Art Gallery'),         
('Nightclub'),           
('Community Center'),    
('Water Park'),          
('Historical Landmark'); 

INSERT INTO [Activity] ([Name], [Description], [Address], [Contact], [Email], [Views], [TypeID]) VALUES 
('The Jazz Attic', 'Intimate basement jazz club.', '55 Saxophone Ave', '555-4444', 'bookings@jazzattic.com', 850, 3),
('Summit Rock', 'Advanced climbing wall and bouldering gym.', '10 Peak St', '555-5555', 'info@summitrock.com', 120, 4),
('Old Town Bakery', 'Fresh sourdough and artisanal jams.', '33 Flour Rd', '555-6666', 'orders@oldtown.com', 430, 4),
('Neon Bowling', 'Glow-in-the-dark bowling alley.', '77 Strike Blvd', '555-7777', 'party@neonbowling.com', 210, 3),
('Gamer Guild Cafe', 'Board games and e-sports arena.', '90 Pixel Dr', '555-9999', 'admin@gamerguild.com', 15, 4),
('The Hive Labs', 'High-speed fiber and bottomless coffee for tech teams.', '101 Silicon Way', '555-0101', 'hello@thehivelabs.com', 450, 1),
('Writer''s Retreat', 'Quiet zone dedicated to novelists and academics.', '42 Ink St', NULL, 'quiet@writers.com', 120, 1),
('Neon Visions', 'A gallery dedicated to contemporary light and neon art.', '500 Lumens Blvd', '555-2020', 'curator@neonvisions.io', 1200, 2),
('The Sculpture Garden', 'Outdoor walk featuring large-scale steel installations.', 'Parkside Plaza', '555-3030', NULL, 890, 2),
('Basement 44', 'Underground techno club with a world-class sound system.', '44 Industrial Rd', '555-4400', 'vip@b44.com', 3200, 3),
('Pulse Rooftop', 'Open-air house music with views of the skyline.', 'Top Floor, Sky Tower', '555-9000', 'bookings@pulserooftop.com', 1500, 3),
('Eastside Hub', 'Hosting weekly farmers markets and youth sports.', '200 Community Cir', '555-5555', 'events@eastsidehub.org', 210, 4),
('Silver Linings', 'Senior activity center offering yoga and chess.', '12 Wisdom Ln', '555-6677', 'contact@silverlinings.org', 95, 4),
('Tidal Wave Bay', 'Massive wave pool and 10-story high drop slides.', '1 Splash Dr', '555-7788', 'info@tidalwave.com', 5600, 5),
('Lazy River Resort', 'Gentle floating channels and tropical cocktails.', '99 Island Pkwy', '555-8899', 'stay@lazyriver.com', 2300, 5),
('Old Foundry Mill', 'Restored 19th-century mill showing industrial history.', 'Historical Mile 1', '555-0011', 'heritage@foundry.org', 410, 3),
('The Clock Tower', 'City landmark built in 1892 with guided bell tours.', 'Central Square', NULL, NULL, 780, 2);

INSERT INTO [Activity_Accessibility] ([ActivityID], [AccessibilityID])
SELECT ID, 1 FROM [Activity] WHERE [Name] = 'The Hive Labs'; 

INSERT INTO [Activity_Accessibility] ([ActivityID], [AccessibilityID])
SELECT ID, 4 FROM [Activity] WHERE [Name] = 'The Hive Labs'; 

INSERT INTO [Activity_Accessibility] ([ActivityID], [AccessibilityID])
SELECT ID, 1 FROM [Activity] WHERE [Name] = 'Neon Visions'; 

INSERT INTO [Activity_Accessibility] ([ActivityID], [AccessibilityID])
SELECT ID, 7 FROM [Activity] WHERE [Name] = 'Basement 44'; 

INSERT INTO [Activity_Accessibility] ([ActivityID], [AccessibilityID])
SELECT ID, 4 FROM [Activity] WHERE [Name] = 'Tidal Wave Bay'; -- Parking

INSERT INTO [Activity_Accessibility] ([ActivityID], [AccessibilityID])
SELECT ID, 2 FROM [Activity] WHERE [Name] = 'The Clock Tower'; -- Braille

INSERT INTO [Review] ([Comment], [Grade], [UserID], [ActivityID])
SELECT 'Incredible fiber internet!', 5, 5, ID 
FROM [Activity] WHERE [Name] = 'The Hive Labs';

INSERT INTO [Review] ([Comment], [Grade], [UserID], [ActivityID])
SELECT 'Way too loud for my taste.', 2, 1, ID 
FROM [Activity] WHERE [Name] = 'Basement 44';

INSERT INTO [Review] ([Comment], [Grade], [UserID], [ActivityID])
SELECT 'Great historical tour.', 4, 4, ID 
FROM [Activity] WHERE [Name] = 'The Clock Tower';

INSERT INTO [Log] ([Level], [Message], [Timestamp]) VALUES
('Info', 'Activity "Riverside Stadium Tour" created',                '2026-07-04 03:12:35.650'),
('Info', 'Activity "Riverside Stadium Tour" with id=18 updated',     '2026-07-04 03:13:28.760'),
('Info', 'Type Historical Landmark successfully deleted!',           '2026-07-04 03:14:25.020'),
('Info', 'Accessibility with id=7 deleted',                          '2026-07-04 03:14:45.650'),
('Info', 'Type Museum, successfully added.',                         '2026-07-04 03:14:59.373'),
('Info', 'Activity "Riverside Stadium Tour" with id=18 deleted',     '2026-07-04 03:17:21.460'),
('Info', 'Type Museum successfully deleted!',                        '2026-07-04 03:18:05.733'),
('Info', 'Activity "Karting Circuit" created',                       '2026-07-04 03:19:05.470'),
('Info', 'Type Park, successfully added.',                           '2026-07-04 03:21:17.487'),
('Info', 'Activity "Karting Circuit" with id=19 updated',            '2026-07-04 03:42:33.730'),
('Info', 'Activity "Karting Circuit" with id=19 updated',            '2026-07-04 03:52:10.947'),
('Info', 'New user was created, username: Dsojic.',                  '2026-07-04 03:53:58.063'),
('Info', 'Activity "Karting Circuit" with id=19 deleted',            '2026-07-04 03:58:09.470'),
('Info', 'Type Park successfully deleted!',                          '2026-07-04 19:36:15.393'),
('Info', 'Activity "The Clock Tower" with id=17 updated',            '2026-07-04 19:37:05.100'),
('Info', 'Activity "The Clock Tower" with id=17 updated',            '2026-07-04 19:37:29.240'),
('Info', 'Activity "The Clock Tower" with id=17 updated',            '2026-07-04 19:45:24.723'),
('Info', 'Accessibility with id=10 deleted',                         '2026-07-04 19:48:44.697'),
('Info', 'Activity "The Clock Tower" with id=17 updated',            '2026-07-04 19:51:39.463'),
('Info', 'Activity "Botanical Garden" created',                      '2026-07-04 21:15:21.080'),
('Info', 'Activity "Botanical Garden" with id=20 deleted',           '2026-07-04 21:20:26.400'),
('Info', 'Activity "City Aquarium" created',                         '2026-07-04 21:23:46.700'),
('Info', 'Activity "City Aquarium" with id=21 deleted',              '2026-07-04 21:25:26.623'),
('Info', 'Activity "Maritime Museum" created',                       '2026-07-04 21:25:52.040'),
('Info', 'Activity "Maritime Museum" with id=22 deleted',            '2026-07-04 21:28:52.777'),
('Info', 'Activity "Old Town Walking Tour" created',                 '2026-07-04 21:46:24.290'),
('Info', 'Activity "Old Town Walking Tour" with id=23 deleted',      '2026-07-05 01:01:53.497'),
('Info', 'Activity "Science Center" created',                        '2026-07-05 01:02:48.163'),
('Info', 'Activity "Science Center" with id=24 deleted',             '2026-07-05 01:06:25.553'),
('Info', 'Accessibility "Wheelchair Ramp" created',                  '2026-07-05 01:23:25.753'),
('Info', 'Accessibility "Audio Guide" created',                      '2026-07-05 01:25:59.843'),
('Info', 'Accessibility with id=12 deleted',                         '2026-07-05 01:30:30.583'),
('Info', 'Accessibility with id=11 deleted',                         '2026-07-05 01:30:31.783'),
('Info', 'Accessibility "Braille Signage" created',                  '2026-07-05 01:30:39.123'),
('Info', 'Accessibility with id=13 deleted',                         '2026-07-05 01:35:33.193'),
('Info', 'Accessibility "Sign Language Guide" created',              '2026-07-05 01:35:35.823'),
('Info', 'Accessibility with id=14 deleted',                         '2026-07-05 02:00:22.730'),
('Info', 'Accessibility "Step-Free Access" created',                 '2026-07-05 03:36:30.777'),
('Info', 'Type Adventure Park, successfully added.',                 '2026-07-05 03:38:35.820'),
('Info', 'Activity "The Hive Labs" with id=6 updated',              '2026-07-05 04:31:57.243'),
('Info', 'Activity "The Clock Tower" with id=17 updated',            '2026-07-05 04:32:33.277'),
('Info', 'Activity "Clock Tower" with id=17 updated',                '2026-07-05 04:33:15.830'),
('Info', 'Activity "The Hive Labs" with id=6 updated',              '2026-07-05 04:38:04.873'),
('Info', 'Activity "Guided Nature Walk" created',                    '2026-07-05 04:53:35.703'),
('Info', 'Activity "Great Historical Tour" created',                 '2026-07-05 04:53:54.237'),
('Info', 'Activity "Guided Nature Walk" with id=25 deleted',         '2026-07-05 04:56:38.277'),
('Info', 'Activity "Great Historical Tour" with id=26 deleted',      '2026-07-05 04:56:40.350'),
('Info', 'Activity "The Sculpture Garden" with id=9 updated',        '2026-07-05 04:57:21.257'),
('Info', 'Activity "The Sculpture Garden" with id=9 deleted',        '2026-07-05 04:59:54.813'),
('Info', 'Activity "Clock Tower" with id=17 updated',                '2026-07-05 05:00:22.460'),
('Info', 'Activity "Clock Tower" with id=17 updated',                '2026-07-05 05:03:01.213'),
('Info', 'Activity "Riverside Cycling Path" created',                '2026-07-05 05:03:48.570'),
('Info', 'Accessibility with id=15 deleted',                         '2026-07-05 05:04:33.747'),
('Info', 'Accessibility "Accessible Parking" created',              '2026-07-05 05:21:24.167'),
('Info', 'Type Museum, successfully added.',                         '2026-07-05 05:21:50.937'),
('Info', 'Type Museum successfully deleted!',                        '2026-07-05 05:21:54.730'),
('Info', 'Type Adventure Park successfully deleted!',                '2026-07-05 05:21:57.517'),
('Info', 'New user was created, username: mkovac.',                  '2026-07-05 05:40:01.613'),
('Info', 'Accessibility with id=1 deleted',                          '2026-07-05 05:41:20.257'),
('Info', 'Activity "Riverside Cycling Path" with id=27 deleted',     '2026-07-06 01:54:15.147'),
('Info', 'Activity "Lakeside Picnic Area" created',                  '2026-07-06 02:35:42.697'),
('Info', 'Type Cafe, successfully added.',                           '2026-07-06 03:28:04.467'),
('Info', 'Type Cafe successfully deleted!',                          '2026-07-06 03:28:06.530'),
('Info', 'Type Theater, successfully added.',                        '2026-07-06 03:28:52.710'),
('Info', 'Type Theater successfully deleted!',                       '2026-07-06 03:28:54.380'),
('Info', 'Accessibility with id=2 deleted',                          '2026-07-06 03:33:40.300'),
('Info', 'Accessibility with id=3 deleted',                          '2026-07-06 03:33:51.417'),
('Info', 'Accessibility with id=4 deleted',                          '2026-07-06 03:33:57.030'),
('Info', 'Accessibility with id=5 deleted',                          '2026-07-06 03:33:59.360'),
('Info', 'Accessibility with id=6 deleted',                          '2026-07-06 03:34:00.510'),
('Info', 'Accessibility with id=8 deleted',                          '2026-07-06 03:34:00.867'),
('Info', 'Accessibility with id=9 deleted',                          '2026-07-06 03:34:01.213'),
('Info', 'Accessibility with id=16 deleted',                         '2026-07-06 03:34:01.573');













