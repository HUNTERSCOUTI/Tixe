Spiller spawner ind
Ingen anomaly

Spiller vælger korrekt vej
- Spiller rammer "CorrectTrigger" og Program spawner næste sektion og giver point
- Program venter på spiller rammer "LevelChangeTrigger"
- Program sletter tidligere sektion + level signs
- Program spawner level signs

Spiller vælger forkert vej
- Spiller rammer "WrongTrigger" og program spawner næste sektion (bagved) og sætter point til 0
- Program venter på spiller rammer "LevelChangeTrigger"
- Program sletter tidligere sektion + level signs
- Program spawner level signs
