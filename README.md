# SUPERSAD Game-Client

## CZ3003 Software Systems Analysis & Design
**SE Party (Game)** is an educational game for students learning Software Engineering that is developed on the Unity Engine for Android, iOS and Windows platforms. Students can engage in quiz matches of 1-4 players and conquer the most tiles on the board while answering questions correctly to win.

**SE Party - Admin** is an administrative application developed on the Unity Engine for the Windows platform. Teachers and lecturers may use the application to create new quizzes as assignments, and view their students' proficiency in Software Engineering topics.


## Gameplay
Goal: Conquer the most tiles on the board to win. Answering questions correctly will provide rewards for players to be able to conquer more tiles.

Game Flow is as follows:
- Answer a quiz question at the start of a turn cycle.
- A player gets 5 moves for their next turn and an item if they have answered the question correctly. Otherwise, they get 2 moves for their next turn.
- Players take turns to have their turn to conquer tiles on the game board.
- During a player's turn, they can use their moves to either take a step or use an item in any combination they like. Items range from helping the player conquer more tiles to temporarily modifying the state of the board.
- The game ends when the number of turn cycles are up. A player's scores are tabulated by the number of tiles owned by the player when the game ends, and the number of questions the player has answered correctly.
- Each player gains Experience Points for their Mastery level for the questions they have answered correctly.


## Game Features
- Player's actions (commands) are sent and responses (ack) are received as packets to and from a dedicated in-house game server through the Transmission Control Protocol (TCP), which ensures reliable transmission.
- Players can register an account and login with their school e-mail to save player progress.
- Create or join a room with customisable settings to play with up to 4 players at a time.
- Players can unlock more costumes from opening Loot Boxes earned from playing in game sessions.
- Players can accumulate scores from games to be on the monthly Leaderboards, or max out all Mastery levels to earn a permanent spot on the Gloryboard.
- Share your game results, Mastery levels and Leaderboard rankings on social media.


## Admin App Features
- Create new questions to be added in custom quizzes for assignments.
- Host custom game sessions for students when the quiz is ready to be released.
- View statistics and analytics of the students.

**Admin Account Credentials**  
username: demoteacher  
password: s123456S!

## Requirements
- An Internet Connection is required to play the game.


## Contributions
- **Boon Hing**: Login/Register, Avatar Customisation, Player Mastery, Loot Box
- **Shing Ling**: Lobby, Room
- **Jing Ting**: Networking, Gameplay, Room Overworld, Event Announcements
- **Steffi**: World Select, Admin App Data Requests
- **Lin Yue**: World Select, Admin App Interfaces
- **Timothy**: Leaderboards, Hall of Fame
- **Yi De**: Character Textures, Audio, Settings, Automated UI Testing